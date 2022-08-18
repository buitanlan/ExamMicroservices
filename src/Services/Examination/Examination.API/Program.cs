using System.Net.Mime;
using System.Text.Json;
using Examination.API.Filters;
using Examination.Application.Commands.V1.Exams.StartExam;
using Examination.Application.Mapping;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure;
using Examination.Infrastructure.Repositories;
using Examination.Infrastructure.SeedWork;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));
var user = builder.Configuration.GetValue<string>("DatabaseSettings:User");
var password = builder.Configuration.GetValue<string>("DatabaseSettings:Password");
var server = builder.Configuration.GetValue<string>("DatabaseSettings:Server");
var databaseName = builder.Configuration.GetValue<string>("DatabaseSettings:DatabaseName");
var mongodbConnectionString = $@"mongodb://{user}:{password}@{server}/{databaseName}?authSource=admin";
var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongodbConnectionString));
builder.Services.AddScoped(c => c.GetService<IMongoClient>()?.StartSession());
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile(new MappingProfile()); });
builder.Services.AddMediatR(typeof(StartExamCommandHandler).Assembly);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Examination.API V1", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Examination.API V2", Version = "v2" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri($"{identityUrl}/connect/authorize"),
                TokenUrl = new Uri($"{identityUrl}/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"exam_api", "exam_api"},
                }
            }
        }
    });
    c.OperationFilter<AuthorizeCheckOperationFilter>();

});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults
        .AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults
        .AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = identityUrl;
    options.RequireHttpsMetadata = false;
    options.Audience = "exam_api";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.Configure<ExamSettings>(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policyBuilder => policyBuilder
            .SetIsOriginAllowed(_ => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddTransient<IExamRepository, ExamRepository>();
builder.Services.AddTransient<IExamResultRepository, ExamResultRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IQuestionRepository, QuestionRepository>();


builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddMongoDb(mongodbConnectionString: mongodbConnectionString,
        name: "mongo",
        failureStatus: HealthStatus.Unhealthy);

builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
    opt.SetApiMaxActiveRequests(1); //api requests concurrency

    opt.AddHealthCheckEndpoint("Exam API", "/hc"); //map health check api
})
.AddInMemoryStorage();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var settings = services.GetRequiredService<IOptions<ExamSettings>>();
    var mongoClient = services.GetRequiredService<IMongoClient>();
    await ExamMongoDbSeeding.SeedAsync(mongoClient, settings);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId("exam_api_swaggerui");
        c.OAuthAppName("exam_api_swaggerui");
        c.OAuthUsePkce();
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Examination.API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Examination.API v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapHealthChecks("/hc", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options => options.UIPath = "/hc-ui");
app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});
app.MapHealthChecks("/hc-details",
    new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                new
                {
                    status = report.Status.ToString(),
                    monitors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                });
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    }
);

app.MapControllers();

app.Run();
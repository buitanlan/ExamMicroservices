FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Identity/IdentityServer.API/IdentityServer.API.csproj", "src/Services/Identity/IdentityServer.API/"]
RUN dotnet restore "src/Services/Identity/IdentityServer.API/IdentityServer.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Identity/IdentityServer.API"
RUN dotnet build "IdentityServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityServer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityServer.API.dll"]

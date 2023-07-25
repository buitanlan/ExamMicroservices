using System.Net;
using Examination.Application.Commands.V1.Categories.CreateCategory;
using Examination.Application.Commands.V1.Categories.DeleteCategory;
using Examination.Application.Commands.V1.Categories.UpdateCategory;
using Examination.Application.Queries.V1.Categories.GetAllCategories;
using Examination.Application.Queries.V1.Categories.GetCategoriesPaging;
using Examination.Application.Queries.V1.Categories.GetCategoryById;
using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Examination.API.Controllers.V1;

public class CategoriesController(IMediator mediator) : BaseController
    {
        [HttpGet("paging")]
        [ProducesResponseType(typeof(PagedList<CategoryDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCategoriesPagingAsync([FromQuery] GetCategoriesPagingQuery query)
        {
            Log.Information("BEGIN: GetCategoriesPagingAsync");

            var result  = await mediator.Send(query);

            Log.Information("END: GetCategoriesPagingAsync");

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategoriesByIdAsync(string id)
        {
            Log.Information("BEGIN: GetCategoriesByIdAsync");

            var result = await mediator.Send(new GetCategoryByIdQuery(id));

            Log.Information("END: GetCategoriesByIdAsync");
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryRequest request)
        {
            Log.Information("BEGIN: UpdateCategoryAsync");

            var result = await mediator.Send(new UpdateCategoryCommand()
            {
                Id = request.Id,
                Name = request.Name,
                UrlPath = request.UrlPath
            });

            Log.Information("END: UpdateCategoryAsync");
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryRequest request)
        {
            Log.Information("BEGIN: CreateCategoryAsync");

            var result = await mediator.Send(new CreateCategoryCommand
            {
                Name = request.Name,
                UrlPath = request.UrlPath
            });
            if (result is null)
            {
                return BadRequest();
            }
            Log.Information("END: CreateCategoryAsync");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteCategoryAsync(string id)
        {
            Log.Information("BEGIN: GetExamList");

            var result = await mediator.Send(new DeleteCategoryCommand(id));

            Log.Information("END: GetExamList");
            return Ok(result);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiSuccessResult<List<CategoryDto>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            Log.Information("BEGIN: GetAllCategoriesAsync");

            var result = await mediator.Send(new GetAllCategoriesQuery());

            Log.Information("END: GetAllCategoriesAsync");

            return Ok(result);
        }
    }
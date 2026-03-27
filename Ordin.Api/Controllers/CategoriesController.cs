using Microsoft.AspNetCore.Mvc;
using Ordin.Api.Contracts;
using Ordin.Application.Commands.Categories.CreateCategory;
using Ordin.Application.Commands.Categories.DeleteCategory;
using Ordin.Application.Commands.Categories.UpdateCategory;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Application.Queries.Categories.GetAllCategories;
using Ordin.Application.Queries.Categories.GetCategoryById;

namespace Ordin.Api.Controllers;

[Route("v1/[controller]")]
public class CategoriesController(IDispatcher dispatcher) : BaseController(dispatcher)
{
    /// <summary>
    /// Retrieves all categories belonging to the authenticated user.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of CategoryDto objects.</returns>
    [HttpGet(Name = nameof(GetAllCategories))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<CategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCategories(CancellationToken ct)
    {
        var query = new GetAllCategoriesQuery();
        var result = await _dispatcher.QueryAsync(query, ct);

        if (result.IsError)
            return Problem(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Retrieves a category by its unique identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The CategoryDto if found.</returns>
    [HttpGet("{id:guid}", Name = nameof(GetCategoryById))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken ct)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await _dispatcher.QueryAsync(query, ct);

        if (result.IsError)
            return Problem(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new category for the authenticated user.
    /// </summary>
    /// <param name="dto">Request body with the category name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created CategoryDto with status 201.</returns>
    [HttpPost(Name = nameof(CreateCategory))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto dto, CancellationToken ct)
    {
        var command = new CreateCategoryCommand(dto.Name);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtAction(nameof(GetCategoryById), new { id = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="dto">Request body with the new category name.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The updated CategoryDto.</returns>
    [HttpPut("{id:guid}", Name = nameof(UpdateCategory))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CategoryRequestDto dto, CancellationToken ct)
    {
        var command = new UpdateCategoryCommand(id, dto.Name);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
            return Problem(result.Errors);

        return Ok(result.Value);
    }

    /// <summary>
    /// Soft-deletes a category by its unique identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>204 No Content on success.</returns>
    [HttpDelete("{id:guid}", Name = nameof(DeleteCategory))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
            return Problem(result.Errors);

        return NoContent();
    }
}

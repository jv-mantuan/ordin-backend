using Ordin.Api.Contracts;
using Ordin.Application.Commands.Transactions.CreateTransaction;
using Ordin.Application.Commands.Transactions.DeleteTransaction;
using Ordin.Application.Commands.Transactions.UpdateTransaction;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Application.Queries.Transactions.GetAllTransactions;
using Ordin.Application.Queries.Transactions.GetTransactionById;
using Microsoft.AspNetCore.Mvc;

namespace Ordin.Api.Controllers;

[Route("v1/[controller]")]
public class TransactionsController(IDispatcher dispatcher) : BaseController(dispatcher)
{
    /// <summary>
    /// Retrieves all transactions created by the user.
    /// </summary>
    /// <param name="ct">The cancellation token that can be used by the caller to cancel the operation.</param>
    /// <returns>An IActionResult containing an ApiResponse with a read-only list of TransactionDto objects. Returns an empty
    /// list if no transactions are found.</returns>
    [HttpGet(Name = nameof(GetAllTransactions))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TransactionWithCategoryNameDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTransactions(CancellationToken ct)
    {
        var query = new GetAllTransactionsQuery();
        var result = await _dispatcher.QueryAsync(query, ct);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }
        
        return Ok(result.Value);
    }

    /// <summary>
    /// Retrieves the details of a transaction by its unique identifier.
    /// </summary>
    /// <remarks>This method will return a 400 Bad Request if the provided identifier is invalid, a 404 Not
    /// Found if the transaction does not exist, and a 500 Internal Server Error for unexpected issues.</remarks>
    /// <param name="id">The unique identifier of the transaction to retrieve.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult containing the transaction details if found; otherwise, a response indicating the error.</returns>
    [HttpGet("{id:guid}", Name = nameof(GetById))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<TransactionWithCategoryNameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var query = new GetTransactionByIdQuery(id);

        var result = await _dispatcher.QueryAsync(query, ct);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates a new transaction using the specified transaction request data.
    /// </summary>
    /// <remarks>Returns a 400 Bad Request response if the input data is invalid, or a 500 Internal Server
    /// Error if an unexpected error occurs. The operation is performed asynchronously.</remarks>
    /// <param name="dto">The transaction request data containing the details of the transaction to create. This includes the transaction
    /// name, amount, type, date, and associated category identifier. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult that represents the result of the create operation. Returns a 201 Created response with the
    /// created transaction details if successful; otherwise, returns an error response.</returns>
    [HttpPost(Name = nameof(Create))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<TransactionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] TransactionRequestDto dto, CancellationToken ct)
    {
        var command = new CreateTransactionCommand(dto.Name, dto.Amount, dto.Type, dto.Date, dto.CategoryId);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return CreatedAtAction(nameof(GetById), new { }, result);
    }

    /// <summary>
    /// Updates a transaction by its unique identifier and using the specified transaction request data.
    /// </summary>
    /// <remarks>Returns a 400 Bad Request response if the input data is invalid, or a 500 Internal Server
    /// Error if an unexpected error occurs. The operation is performed asynchronously.</remarks>
    /// <param name="id">The unique identifier of the transaction to update.</param>
    /// <param name="dto">The transaction request data containing the details of the transaction to create. This includes the transaction
    /// name, amount, type, date, and associated category identifier. Cannot be null.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult that represents the result of the create operation. Returns a 201 Created response with the
    /// created transaction details if successful; otherwise, returns an error response.</returns>
    [HttpPut("{id:guid}", Name = nameof(Update))]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse<TransactionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] TransactionRequestDto dto, CancellationToken ct)
    {
        var command = new UpdateTransactionCommand(id, dto.Name, dto.Amount, dto.Type, dto.Date, dto.CategoryId);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes the transaction identified by its unique identifier.
    /// </summary>
    /// <remarks>This method will return a 400 Bad Request if the provided identifier is invalid, a 404 Not
    /// Found if the transaction does not exist, and a 500 Internal Server Error for unexpected issues.</remarks>
    /// <param name="id">The unique identifier of the transaction to be deleted.</param>
    /// <param name="ct">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>An IActionResult indicating the result of the delete operation. Returns 204 No Content if the deletion is
    /// successful; otherwise, returns an appropriate error response.</returns>
    [HttpDelete("{id:guid}", Name = nameof(Delete))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var command = new DeleteTransactionCommand(id);
        var result = await _dispatcher.SendAsync(command, ct);

        if (result.IsError)
        {
            return Problem(result.Errors);
        }

        return NoContent();
    }
}


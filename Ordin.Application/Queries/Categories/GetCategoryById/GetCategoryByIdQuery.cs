using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Categories.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>;
}

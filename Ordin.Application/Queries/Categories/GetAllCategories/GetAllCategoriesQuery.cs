using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Categories.GetAllCategories
{
    public record GetAllCategoriesQuery() : IQuery<IReadOnlyList<CategoryDto>>;
}

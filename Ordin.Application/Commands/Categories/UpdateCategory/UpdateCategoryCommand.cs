using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(Guid Id, string Name) : ICommand<CategoryDto>;
}

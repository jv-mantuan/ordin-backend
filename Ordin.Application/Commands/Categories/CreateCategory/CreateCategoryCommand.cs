using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Categories.CreateCategory
{
    public record CreateCategoryCommand(string Name) : ICommand<CategoryDto>;
}

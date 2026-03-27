using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid Id) : ICommand<Deleted>;
}

using ErrorOr;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Categories.DeleteCategory
{
    public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand, Deleted>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<Deleted>> HandleAsync(DeleteCategoryCommand command, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(command.Id, ct);

            if (category is null)
                return Error.NotFound("Category.NotFound", "The category was not found.");

            if (category.UserId != _currentUserService.UserId)
                return Error.Forbidden("Category.Forbidden", "You do not have permission to delete this category.");

            category.Delete();

            return Result.Deleted;
        }
    }
}

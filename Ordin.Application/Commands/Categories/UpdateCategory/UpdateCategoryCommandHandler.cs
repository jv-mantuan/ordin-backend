using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryCommandHandler : CommandHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<CategoryDto>> HandleAsync(UpdateCategoryCommand command, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(command.Id, ct);

            if (category is null || category.IsDeleted)
                return Error.NotFound("Category.NotFound", "The category was not found.");

            if (category.UserId != _currentUserService.UserId)
                return Error.Forbidden("Category.Forbidden", "You do not have permission to update this category.");

            category.Update(command.Name);

            _categoryRepository.Update(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId
            };
        }
    }
}

using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;
using Ordin.Domain.Entities;

namespace Ordin.Application.Commands.Categories.CreateCategory
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<CategoryDto>> HandleAsync(CreateCategoryCommand command, CancellationToken ct)
        {
            var category = Category.Create(command.Name, _currentUserService.UserId);

            await _categoryRepository.Add(category, ct);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId
            };
        }
    }
}

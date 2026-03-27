using ErrorOr;
using Ordin.Application.DTOs;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Categories.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<ErrorOr<CategoryDto>> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct)
        {
            var category = await _categoryRepository.GetByIdAsync(query.Id, ct);

            if (category is null)
                return Error.NotFound("Category.NotFound", "The category was not found.");

            if (category.UserId != _currentUserService.UserId)
                return Error.Forbidden("Category.Forbidden", "You do not have permission to access this category.");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UserId = category.UserId
            };
        }
    }
}

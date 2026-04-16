using ErrorOr;
using Ordin.Application.Attributes;
using Ordin.Application.DTOs;
using Ordin.Application.Enums;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Categories.GetCategoryById
{
    [CacheableQuery(CacheKey = CacheKeys.Categories, DurationInSeconds = 86400)]
    public class GetCategoryByIdQueryHandler : QueryHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<CategoryDto>> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct)
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

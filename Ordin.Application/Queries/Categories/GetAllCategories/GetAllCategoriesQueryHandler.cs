using ErrorOr;
using Ordin.Application.Attributes;
using Ordin.Application.DTOs;
using Ordin.Application.Enums;
using Ordin.Application.Handlers;
using Ordin.Application.Interfaces;

namespace Ordin.Application.Queries.Categories.GetAllCategories
{
    [CacheableQuery(CacheKey = CacheKeys.Categories, DurationInSeconds = 86400)]

    public class GetAllCategoriesQueryHandler : QueryHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public override async Task<ErrorOr<IReadOnlyList<CategoryDto>>> HandleAsync(GetAllCategoriesQuery query, CancellationToken ct)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(_currentUserService.UserId, ct);

            var dto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                UserId = c.UserId
            }).ToList();

            return dto;
        }
    }
}

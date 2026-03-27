namespace Ordin.Application.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public Guid UserId { get; init; }
    }
}
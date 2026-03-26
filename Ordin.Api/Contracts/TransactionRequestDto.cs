using Ordin.Domain.Enums;

namespace Ordin.Application.DTOs
{
    public class TransactionRequestDto
    {
        public string Name { get; set; } = null!;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTimeOffset Date { get; set; }
        public Guid CategoryId { get; set; }
    }
}
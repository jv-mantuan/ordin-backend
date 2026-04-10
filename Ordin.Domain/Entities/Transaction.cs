using Ordin.Domain.Enums;
using Ordin.Domain.ValueObjects;

namespace Ordin.Domain.Entities
{
    /// <summary>
    /// Represents a financial transaction, including details such as the transaction name, amount, type, date,
    /// category, and associated user.
    /// </summary>
    public class Transaction : BaseEntity
    {
        public Transaction(string name, Money amount, TransactionType type, DateTimeOffset date, Guid categoryId,
            Guid userId, Guid id = default) : base(id)
        {
            Name = name;
            Amount = amount;
            Type = type;
            Date = date;
            CategoryId = categoryId;
            UserId = userId;
        }

        // Parameterless constructor required by EF
        private Transaction()
        {
        }

        public string Name { get; private set; } = null!;
        public Money Amount { get; private set; } = null!;
        public TransactionType Type { get; private set; }
        public DateTimeOffset Date { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;

        public bool IsDeleted { get; private set; } = false;

        public void Update(string name, Money amount, TransactionType type, DateTimeOffset date, Guid categoryId)
        {
            Name = name;
            Amount = amount;
            Type = type;
            Date = date;
            CategoryId = categoryId;
        }
        
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
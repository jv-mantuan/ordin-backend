namespace Ordin.Domain.Entities;

/// <summary>
/// Represents a user-defined category that groups related transactions for a specific user.
/// </summary>
public class Category : BaseEntity
{
    public static Category Create(string name, Guid userId) => new(name, userId);

    protected Category(string name, Guid userId, Guid id = default) : base(id)
    {
        Name = name;
        UserId = userId;
    }

    // Parameterless constructor required by EF
    private Category() { }

    public string Name { get; private set; }

    public User User { get; private set; } = null!;
    public Guid UserId { get; private set; }

    public bool IsDeleted { get; private set; } = false;

    private readonly List<Transaction> _transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    public void Update(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Marks the category as deleted and also marks all associated transactions as deleted.<br/>
    /// Be sure that the transactions should be loaded by EF before calling this method, otherwise the transactions won't be marked as deleted.
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;

        foreach (var transaction in _transactions)
            transaction.Delete();
    }
}
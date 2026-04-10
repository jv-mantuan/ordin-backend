namespace Ordin.Domain.Entities;

/// <summary>
/// Represents a user of the financial management system, including their unique external identifier, email, name,
/// </summary>
public class User : BaseEntity
{
    protected User(string externalId, string email, string name, Guid id = default) : base(id)
    {
        ExternalId = externalId;
        Email = email;
        Name = name;
    }

    public string ExternalId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }

    public bool IsDeleted { get; private set; } = false;

    private readonly List<Category> _categories = [];
    public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

    private readonly List<Transaction> _transactions = [];
    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    /// <summary>
    /// Marks the user as deleted and also marks all associated categories and transactions as deleted.<br/>
    /// Be sure that the categories and transactions should be loaded by EF before calling this method, otherwise the transactions won't be marked as deleted.
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;

        foreach (var category in _categories)
            category.Delete();
    }
}
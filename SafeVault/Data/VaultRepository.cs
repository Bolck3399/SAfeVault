public interface IVaultRepository
{
    List<VaultItem> GetAll();
    List<VaultItem> GetByOwner(string owner);
}

public class VaultRepository : IVaultRepository
{
    private readonly AppDbContext _db;

    public VaultRepository(AppDbContext db)
    {
        _db = db;
    }

    public List<VaultItem> GetAll()
    {
        return _db.VaultItems.ToList();
    }

    public List<VaultItem> GetByOwner(string owner)
    {
        return _db.VaultItems
            .Where(x => x.Owner == owner)
            .ToList();
    }
}
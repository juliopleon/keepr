namespace keepr.Repositories;

public class ProfilesRepository
{
    private readonly IDbConnection _db;

    public ProfilesRepository(IDbConnection db)
    {
        _db = db;
    }


    internal Account GetProfileById(string id)
    {
        string sql = @"
        SELECT 
        *
        FROM accounts
        WHERE id = @id;
        ";
        return _db.Query<Account>(sql, new { id }).FirstOrDefault();
    }

    internal List<Keep> GetProfileKeeps(string id)
    {
        string sql = @"
        SELECT
        kp.*,
        COUNT(vk.id) AS kept,
        ac.*
        FROM keeps kp
        JOIN accounts ac ON ac.id = kp.creatorId
        LEFT JOIN vaultkeeps vk ON vk.keepId = kp.id
        WHERE kp.creatorId = @id
        GROUP BY kp.id
        ;";
        return _db.Query<Keep, Account, Keep>(sql, (keep, account) =>
        {
            keep.Creator = account;
            return keep;
        }, new { id }).ToList();
    }

    internal List<Vault> GetProfileVaults(string id)
    {
        string sql = @"
        SELECT
        vt.*,
        ac.*
        FROM vaults vt
        JOIN accounts ac ON ac.id = vt.creatorId
        WHERE vt.creatorId = @id;
        ";
        return _db.Query<Vault, Account, Vault>(sql, (vault, account) =>
        {
            vault.Creator = account;
            return vault;
        }, new { id }).ToList();
    }

}

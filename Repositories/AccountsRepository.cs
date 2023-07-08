namespace keepr.Repositories;

public class AccountsRepository
{
    private readonly IDbConnection _db;

    public AccountsRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Account GetByEmail(string userEmail)
    {
        string sql = "SELECT * FROM accounts WHERE email = @userEmail";
        return _db.QueryFirstOrDefault<Account>(sql, new { userEmail });
    }

    internal Account GetById(string id)
    {
        string sql = "SELECT * FROM accounts WHERE id = @id";
        return _db.QueryFirstOrDefault<Account>(sql, new { id });
    }

    internal Account Create(Account newAccount)
    {
        string sql = @"
            INSERT INTO accounts
              (name, picture, email, id)
            VALUES
              (@Name, @Picture, @Email, @Id)";
        _db.Execute(sql, newAccount);
        return newAccount;
    }

    internal Account Edit(Account update)
    {
        string sql = @"
            UPDATE accounts
            SET 
              name = @Name,
              picture = @Picture,
              coverImg = @CoverImg
            WHERE id = @Id;";
        _db.Execute(sql, update);
        return update;
    }

    internal List<Keep> GetMyKeeps(string accountId)
    {
        string sql = @"
        SELECT
        kp.*,
        ac.*
        FROM keeps kp
        JOIN accounts ac ON ac.id = kp.creatorId
        LEFT JOIN vaultkeeps vt ON vk.keepId = kp.Id
        WHERE kp.creatorId = @accountId
        GROUP BY kp.id;
        ";
        return _db.Query<Keep, Account, Keep>(sql, (keep, account) =>
        {
            keep.Creator = account;
            return keep;
        }, new { accountId }).ToList();
    }



    internal List<Vault> GetMyVaults(string accountId)
    {
        string sql = @"
        SELECT
        ac.*,
        vt.*
        FROM vaults vt
        JOIN accounts ac ON ac.id = vt.creatorId
        WHERE vt.creatorId = @accountId;
        ";
        return _db.Query<Account, Vault, Vault>(sql, (account, vault) =>
        {
            vault.Creator = account;
            return vault;
        }, new { accountId }).ToList();
    }

}


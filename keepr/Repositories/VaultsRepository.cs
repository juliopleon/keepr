namespace keepr.Repositories;

public class VaultsRepository
{
    private readonly IDbConnection _db;

    public VaultsRepository(IDbConnection db)
    {
        _db = db;
    }

    internal Vault Create(Vault vaultData)
    {
        string sql = @"
        INSERT INTO vaults
        (creatorId, name, description, img, isPrivate)
        VALUES
        (@creatorId,@name, @description, @img, @isPrivate);
        SELECT LAST_INSERT_ID();
        ";
        int id = _db.ExecuteScalar<int>(sql, vaultData);
        vaultData.Id = id;
        return vaultData;

    }

    internal Vault GetOne(int id)
    {
        string sql = @"
        SELECT
        vt.*,
        ac.*
        FROM vaults vt
        JOIN accounts ac ON ac.id = vt.creatorId
        WHERE vt.id = @id;
        ";

        return _db.Query<Vault, Account, Vault>(sql, (vault, account) =>
        {
            vault.Creator = account;
            return vault;
        }, new { id }).FirstOrDefault();
    }

    public bool Update(Vault update)
    {
        string sql = @"
        UPDATE vaults
        SET
        name = @name,
        description = @description,
        img = @img,
        isPrivate = @isPrivate
        WHERE id = @id;
        ";
        int rows = _db.Execute(sql, update);
        return rows > 0;
    }

    internal bool Remove(int id)
    {
        string sql = @"
        DELETE FROM vaults
        WHERE id = @id;
        ";
        int rows = _db.Execute(sql, new { id });
        return rows > 0;

    }
}

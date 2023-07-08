
namespace keepr.Repositories;

public class VaultKeepsRepository
{
    private readonly IDbConnection _db;

    public VaultKeepsRepository(IDbConnection db)
    {
        _db = db;
    }

    internal VaultKeep GetOne(int id)
    {
        string sql = @"
        SELECT 
        *
        FROM vaultkeeps
        WHERE id = @id;
        ";
        return _db.Query<VaultKeep>(sql, new { id }).FirstOrDefault();
    }

    internal VaultKeep GetVaultKeepById(int vaultKeepId)
    {
        string sql = @"
    SELECT *
    FROM vaultkeeps
    WHERE id = @vaultKeepId
    ;";

        return _db.Query<VaultKeep>(sql, new { vaultKeepId }).FirstOrDefault();
    }

    internal VaultKeep Create(VaultKeep vaultKeepData)
    {
        string sql = @"
        INSERT INTO vaultkeeps
        (creatorId, vaultId, keepId)
        VALUES
        (@creatorId, @vaultId, @keepId);
        SELECT LAST_INSERT_ID();";
        int id = _db.ExecuteScalar<int>(sql, vaultKeepData);
        vaultKeepData.Id = id;

        return vaultKeepData;
    }


    internal bool Remove(int id)
    {
        string sql = @"
        DELETE FROM vaultkeeps
        WHERE id = @id;
        ";
        int rows = _db.Execute(sql, new { id });

        return rows > 0;
    }

    internal void DeleteVaultKeep(int id)
    {
        var sql = @"
              DELETE FROM vaultkeeps WHERE id = @id
                  ; ";

        var rows = _db.Execute(sql, new { id });


    }

    internal List<KeepInVault> GetKeepsByVaultId(int vaultId)
    {
        var sql = @"
          SELECT 
            vk.*,
            vk.id AS VaultKeepId,
            vk.creatorId AS VaultKeepCreatorId,
            kp.*,
            ac.*
          FROM vaultkeeps vk
          JOIN keeps kp On kp.id = vk.keepId
          JOIN accounts ac ON ac.id = kp.creatorId
          WHERE vk.vaultId = @vaultId
          GROUP BY vk.id
          ;";
        return _db.Query<KeepInVault, Account, KeepInVault>(sql, (keep, account) =>
        {
            keep.Creator = account;
            return keep;
        }, new { vaultId }).ToList();
    }
}

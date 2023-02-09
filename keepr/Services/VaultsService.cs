namespace keepr.Services;

public class VaultsService
{
    private readonly VaultsRepository _repo;

    private readonly KeepsService _keepsService;

    public VaultsService(VaultsRepository repo, KeepsService keepsService)
    {
        _repo = repo;
        _keepsService = keepsService;
    }

    internal Vault Create(Vault vaultData)
    {
        Vault vault = _repo.Create(vaultData);
        return vault;
    }

    internal Vault GetOne(int id, string userId)
    {
        Vault vault = _repo.GetOne(id);
        if (vault == null)
        {
            throw new Exception($"No Vault at ID:{id}");
        }
        if (vault.IsPrivate == true && vault.CreatorId != userId)
        {
            throw new Exception("Private Vaults are Private!");

        }
        return vault;
    }

    internal Vault Update(Vault vaultUpdate)
    {
        Vault original = GetOne(vaultUpdate.Id, vaultUpdate.CreatorId);
        if (original.CreatorId != vaultUpdate.CreatorId) throw new Exception("Not your vault!");
        original.Name = vaultUpdate.Name ?? original.Name;
        original.Description = vaultUpdate.Description ?? original.Description;
        original.Img = vaultUpdate.Img ?? original.Img;
        original.IsPrivate = vaultUpdate.IsPrivate ?? original.IsPrivate;

        _repo.Update(original);
        return original;
    }

    internal string Remove(int id, string userId)
    {
        Vault original = GetOne(id, userId);
        if (original.CreatorId != userId)
        {
            throw new Exception("Not your Vault to delete!");
        }

        _repo.Remove(id);
        return $"{original.Name} has been Deleted";
    }


}

namespace keepr.Services;

public class VaultKeepsService
{
    private readonly VaultKeepsRepository _repo;

    private readonly KeepsRepository _kRepo;


    private readonly KeepsService _keepsService;

    private readonly VaultsService _vaultsService;

    public VaultKeepsService(VaultKeepsRepository repo, KeepsService keepsService, KeepsRepository kRepo, VaultsService vaultsService)
    {
        _repo = repo;
        _keepsService = keepsService;
        _kRepo = kRepo;
        _vaultsService = vaultsService;
    }

    // internal VaultKeep Create(VaultKeep vaultKeepData, string userId)
    // {
    //     Keep keep = _keepsService.GetOne(vaultKeepData.KeepId, vaultKeepData.CreatorId);
    //     VaultKeep vaultKeep = _repo.Create(vaultKeepData);


    //     if (keep.CreatorId != userId)
    //     {
    //         keep.Kept++;
    //         UpdateKeep(keep);
    //     }

    //     return vaultKeep;

    // }
    // NOTE - this will work better last method did not pass create vault keep method
    internal VaultKeep Create(VaultKeep keepData, string userId)
    {
        Vault vault = _vaultsService.GetOne(keepData.VaultId, keepData.CreatorId);
        if (vault.CreatorId != userId)
        {
            throw new Exception("not on my watch!");
        }
        // NOTE - this is how to get something and bring it into the method and then use another method 
        VaultKeep keep = _repo.Create(keepData);
        Keep vaultKeep = _kRepo.GetOne(keep.KeepId);
        vaultKeep.Kept++;
        _kRepo.Update(vaultKeep);

        return keep;

    }



    public void UpdateKeep(Keep k)
    {
        _kRepo.Update(k);
    }

    // internal string Remove(int id, string userId)
    // {

    //     _repo.Remove(id);
    //     return $"{id} has been removed";
    // }

    // internal void DeleteVaultKeep(int vaultKeepId, string accountId)
    // {
    //     var vk = GetVaultKeepById(vaultKeepId);
    //     if (vk.CreatorId != accountId)
    //     {
    //         throw new Exception("Unauthorized to delete VaultKeep");
    //     }
    //     _repo.DeleteVaultKeep(vaultKeepId);
    // }

    internal string DeleteVaultKeep(int id, string userId)
    {
        VaultKeep original = _repo.GetOne(id);
        Keep keep = _kRepo.GetOne(original.KeepId);
        keep.Kept--;
        _kRepo.Update(keep);
        if (original == null)
        {
            throw new Exception("No Keep at that Id");
        }

        if (userId == null)
        {
            throw new Exception("NO ACCESS!!");

        }

        if (original.CreatorId != userId)
        {
            throw new Exception("Not your keep to delete!");
        }

        _repo.Remove(id);
        return $"Removed";
    }

    internal VaultKeep GetVaultKeepById(int vaultKeepId)
    {
        var vaultKeep = _repo.GetVaultKeepById(vaultKeepId);
        if (vaultKeep == null)
        {
            throw new Exception("Bad VaultKeep Id");
        }
        return vaultKeep;
    }
}

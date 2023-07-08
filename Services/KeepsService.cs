namespace keepr.Services;

public class KeepsService
{
    private readonly KeepsRepository _repo;

    private readonly VaultsRepository _vRepo;
    public KeepsService(KeepsRepository repo, VaultsRepository vRepo)
    {
        _repo = repo;

        _vRepo = vRepo;
    }

    internal Keep Create(Keep keepData)
    {
        Keep keep = _repo.Create(keepData);

        return keep;
    }

    internal List<Keep> Get(string userId)
    {
        List<Keep> keeps = _repo.Get();
        List<Keep> filtered = keeps.FindAll(k => k.CreatorId == userId);

        return keeps;
    }

    internal Keep GetOne(int id, string userId)
    {
        Keep keep = _repo.GetOne(id);
        if (keep == null)
        {
            throw new Exception($"No Keep found at ID: {id}");
        }

        if (keep.CreatorId != userId)
        {
            keep.Views++;
            UpdateKeep(keep);
        }
        return keep;
    }

    public void UpdateKeep(Keep k)
    {
        _repo.Update(k);
    }

    internal Keep Update(Keep keepUpdate, int id, string userId)
    {
        Keep original = GetOne(id, userId);
        if (original.CreatorId != userId)
        {
            throw new Exception("not your keep to edit");
        }
        original.Name = keepUpdate.Name ?? original.Name;
        original.Description = keepUpdate.Description ?? original.Description;
        original.Img = keepUpdate.Img ?? original.Img;

        bool edited = _repo.Update(original);
        if (edited == false)
        {
            throw new Exception("Your Keep was not edited");
        }

        return original;
    }

    internal string Remove(int id, string userId)
    {
        Keep original = GetOne(id, userId);
        if (original.CreatorId != userId)
        {
            throw new Exception("Not your Keep to Delete!");
        }

        _repo.Remove(id);
        return $"{original.Name} has been deleted";
    }

    // NOTE come back and put a getvaultkeeps() to get keeps in a vault

    // internal Keep KeepCount(int id)
    // {
    //     Keep keep = _repo.GetOne(id);
    //     keep.Kept++;
    //     _repo.Update(keep);
    //     return keep;
    // }

    // internal Keep ViewCount(int id)
    // {
    //     Keep keep = _repo.GetOne(id);
    //     keep.Views++;
    //     _repo.Update(keep);
    //     return keep;
    // }

    internal List<KeepInVault> GetKeeps(int id, string userId)
    {
        Vault vault = _vRepo.GetOne(id);

        if (vault.CreatorId != userId && vault.IsPrivate == true)
        {
            throw new Exception("Don't have access!");
        }
        List<KeepInVault> keepInVaults = _repo.GetVaultKeeps(id);
        return keepInVaults;
    }




}

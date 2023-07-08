namespace keepr.Services;

public class ProfilesService
{
    private readonly ProfilesRepository _repo;

    public ProfilesService(ProfilesRepository repo)
    {
        _repo = repo;
    }

    internal Account GetProfileById(string id)
    {
        return _repo.GetProfileById(id);
    }

    internal List<Keep> GetProfileKeeps(string id)
    {
        return _repo.GetProfileKeeps(id);
    }

    internal List<Vault> GetProfileVaults(string id, string userId)
    {

        // NOTE Alias this out so I can work dont return
        // Albums service line 20 to 25 Good referecen
        // Return based on the similar logic that we wrote in the getone

        List<Vault> vaults = _repo.GetProfileVaults(id);

        List<Vault> filtered = vaults.FindAll(v => v.IsPrivate == false && v.CreatorId != userId);

        return filtered;


        //  _repo.GetProfileVaults(id);
    }
}

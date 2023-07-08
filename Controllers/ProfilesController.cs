namespace keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly ProfilesService _profilesService;
    private readonly Auth0Provider _auth0Provider;

    public ProfilesController(ProfilesService profilesService, Auth0Provider auth0Provider)
    {
        _profilesService = profilesService;
        _auth0Provider = auth0Provider;
    }

    [HttpGet("{id}")]
    public ActionResult<Account> GetProfileById(string id)
    {
        try
        {

            Account profile = _profilesService.GetProfileById(id);
            return Ok(profile);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/keeps")]
    public ActionResult<List<Keep>> GetProfileKeeps(string id)
    {
        try
        {
            List<Keep> keeps = _profilesService.GetProfileKeeps(id);
            return Ok(keeps);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/vaults")]
    public ActionResult<List<Vault>> GetProfileVaults(string id, string userId)
    {
        try
        {
            List<Vault> vaults = _profilesService.GetProfileVaults(id, userId);
            return Ok(vaults);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }



}

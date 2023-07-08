namespace keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeepsController : ControllerBase
{
    private readonly KeepsService _keepsService;
    private readonly Auth0Provider _auth0provider;
    private readonly VaultsService _vaultsService;

    public KeepsController(KeepsService keepsService, Auth0Provider auth0provider, VaultsService vaultsService)
    {
        _keepsService = keepsService;
        _auth0provider = auth0provider;
        _vaultsService = vaultsService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Keep>> Create([FromBody] Keep keepData)
    {
        try
        {
            Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
            keepData.CreatorId = userInfo.Id;

            Keep keep = _keepsService.Create(keepData);
            keep.Creator = userInfo;

            return Ok(keep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Keep>>> Get()
    {
        try
        {
            Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
            List<Keep> keep = _keepsService.Get(userInfo?.Id);
            return Ok(keep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Keep>> GetOne(int id)
    {
        try
        {
            Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);

            Keep keep = _keepsService.GetOne(id, userInfo?.Id);
            return Ok(keep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Keep>> Update([FromBody] Keep keepData, int id)
    {
        try
        {
            Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
            Keep keep = _keepsService.Update(keepData, id, userInfo.Id);
            return Ok(keep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<string>> Remove(int id)
    {
        try
        {
            Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _keepsService.Remove(id, userInfo.Id);
            return Ok(message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // [HttpGet("{id}/vaults")]
    // public async Task<ActionResult<List<Vault>>> GetVaultKeeps(int id)
    // {
    //     try
    //     {
    //         Account userInfo = await _auth0provider.GetUserInfoAsync<Account>(HttpContext);
    //         List<Vault> vaults = _keepsService.GetVaultKeeps(id, userInfo?.Id);
    //         return Ok(vaults);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }
}

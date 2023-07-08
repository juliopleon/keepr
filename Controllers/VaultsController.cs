namespace keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultsController : ControllerBase
{
    private readonly VaultsService _vaultsService;
    private readonly Auth0Provider _auth0Provider;

    private readonly KeepsService _keepsService;



    public VaultsController(VaultsService vaultsService, Auth0Provider auth0Provider, KeepsService keepsService)
    {
        _vaultsService = vaultsService;
        _auth0Provider = auth0Provider;
        _keepsService = keepsService;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Vault>> Create([FromBody] Vault vaultData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            vaultData.CreatorId = userInfo.Id;

            Vault vault = _vaultsService.Create(vaultData);
            vault.Creator = userInfo;

            return Ok(vault);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vault>> GetOne(int id)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            Vault vault = _vaultsService.GetOne(id, userInfo?.Id);
            return Ok(vault);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Vault>> Update(int id, [FromBody] Vault vaultUpdate)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            vaultUpdate.CreatorId = userInfo.Id;
            vaultUpdate.Id = id;
            Vault vault = _vaultsService.Update(vaultUpdate);
            return Ok(vault);

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
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _vaultsService.Remove(id, userInfo.Id);
            return Ok(message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}/keeps")]
    public async Task<ActionResult<List<KeepInVault>>> GetVaultKeepsAsync(int id)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            List<KeepInVault> keepInVaults = _keepsService.GetKeeps(id, userInfo?.Id);
            return Ok(keepInVaults);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}

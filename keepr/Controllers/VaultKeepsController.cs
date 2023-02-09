namespace keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultKeepsController : ControllerBase
{
    private readonly VaultKeepsService _vaultKeepsService;
    private readonly Auth0Provider _auth0Provider;

    public VaultKeepsController(VaultKeepsService vaultKeepsService, Auth0Provider auth0Provider)
    {
        _vaultKeepsService = vaultKeepsService;
        _auth0Provider = auth0Provider;
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<VaultKeep>> Create([FromBody] VaultKeep vaultKeepData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            vaultKeepData.CreatorId = userInfo.Id;

            VaultKeep vaultKeep = _vaultKeepsService.Create(vaultKeepData, userInfo?.Id);

            return Ok(vaultKeep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    // [HttpDelete("{id}")]
    // [Authorize]
    // public async Task<ActionResult<string>> Remove(int id)
    // {
    //     try
    //     {
    //         Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
    //         string message = _vaultKeepsService.deleteVaultKeep(id, userInfo.Id);
    //         return Ok(message);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }

    [HttpDelete("{vaultKeepId}")]
    [Authorize]
    public async Task<ActionResult<string>> DeleteVaultKeep(int vaultKeepId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            _vaultKeepsService.DeleteVaultKeep(vaultKeepId, userInfo?.Id);
            return Ok("VaultKeep deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}

namespace keepr.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly Auth0Provider _auth0Provider;

    public AccountController(AccountService accountService, Auth0Provider auth0Provider)
    {
        _accountService = accountService;
        _auth0Provider = auth0Provider;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<Account>> Get()
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            return Ok(_accountService.GetOrCreateProfile(userInfo));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{vaults}")]
    [Authorize]
    public async Task<ActionResult> GetVaultsByAccountId()
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            List<Vault> myVaults = _accountService.GetMyVaults(userInfo?.Id);
            return Ok(myVaults);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult<Account>> Edit([FromBody] Account accountData)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            accountData.Email = userInfo.Email;
            Account keep = _accountService.Edit(accountData, accountData?.Email);
            return Ok(keep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}

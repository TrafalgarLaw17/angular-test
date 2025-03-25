using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Controllers
{
    [Route("api/[controller]")]
    [Tags("Account")]
    // [ApiController]
    // [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class AccountController : ControllerBase
    {
        private readonly AccountContext _accountContext;
        private readonly ResponseHttpContext _responseHttpContext;


        public AccountController(DBContext dbContext, ILogger<AccountContext> accountContextLogger)
        {
            _accountContext = new AccountContext(dbContext.GetConnection().ConnectionString);
            _responseHttpContext = new ResponseHttpContext(dbContext.GetConnection().ConnectionString);
            TodoLogger.ConfigureLogger(accountContextLogger);
        }

        #region (1) ACCOUNT [HttpGet("list")]
        [HttpGet("list")]
        [SwaggerOperation(Summary = "Get all account records", Description = "This API retrieves all registered accounts.")]
        public IActionResult RetrieveAllAccounts()
        {
            try
            {
                var accounts = _accountContext.RetrieveAllAccounts();
                TodoLogger.LogSuccessRetrieveAll();
                return ResponseBuilderUtil.BuildResponse(_responseHttpContext, StatusCodes.Status200OK, accounts);
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailRetrieveAll(ex.Message);
                return ResponseBuilderUtil.BuildResponse(_accountContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        #endregion
    }
}

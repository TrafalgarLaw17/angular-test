using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Controllers
{
    [Route("api/[controller]")]
    [Tags("HTTP Response")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class HttpResponseController : ControllerBase
    {
        private readonly ResponseHttpContext _responseHttpContext;

        public HttpResponseController (DBContext dbContext, ILogger<ResponseHttpContext> todoContextLogger)
        {
            _responseHttpContext = new ResponseHttpContext(dbContext.GetConnection().ConnectionString);
            TodoLogger.ConfigureLogger(todoContextLogger);
        }

        #region (1) HTTP RESPONSE [HttpGet("list")]
        [HttpGet("list")]
        [SwaggerOperation(Summary = "Get all response HTTP messages", Description = "This API retrieves all predefined response HTTP messages from the database.")]
        public IActionResult RetrieveAllResponsesHttp()
        {
            try
            {
                var responseHttp = _responseHttpContext.RetrieveAllResponsesHttp();
                TodoLogger.LogSuccessRetrieveAll();
                return ResponseBuilderUtil.BuildResponse(_responseHttpContext, StatusCodes.Status200OK, responseHttp);
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailRetrieveAll(ex.Message);
                return ResponseBuilderUtil.BuildResponse(_responseHttpContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        #endregion
    }
}

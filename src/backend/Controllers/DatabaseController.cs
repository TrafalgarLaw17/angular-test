using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Controllers
{
    [Route("api/[controller]")]
    [Tags("Database")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class DatabaseController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public DatabaseController(DBContext dbContext, ILogger<TodoContext> todoContextLogger)
        {
            _todoContext = new TodoContext(dbContext.GetConnection().ConnectionString);
            TodoLogger.ConfigureLogger(todoContextLogger);
        }

            #region (1) GET [HttpGet] (Exception Handling for Database Errors)
            [HttpGet("error")]
            [SwaggerOperation(Summary = "Simulated database error", Description = "This API simulates a database error for testing exception handling.")]
            public IActionResult GetDatabaseError()
            {
                try
                {
                    throw new Exception("Simulated database error");
                }
                catch (Exception ex)
                {
                    TodoLogger.LogExceptionDatabase(ex.Message);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
                }
            }
            #endregion

    }
}

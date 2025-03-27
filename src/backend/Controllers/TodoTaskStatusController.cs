using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Controllers
{
    [Route("api/[controller]")]
    [Tags("Todo Task Status")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class TodoTaskStatusController : ControllerBase
    {
        private readonly TodoContext _todoContext;

        public TodoTaskStatusController (DBContext dbContext, ILogger<TodoContext> todoContextLogger)
        {
            _todoContext = new TodoContext(dbContext.GetConnection().ConnectionString);
            TodoLogger.ConfigureLogger(todoContextLogger);
        }

        #region (1) TODOTASKSTATUS [HttpGet("task-status-list")]
        [HttpGet("task-status-list")]
        [SwaggerOperation(Summary = "Get all todo status records", Description = "This API retrieves all possible statuses for todos.")]
        public IActionResult RetrieveAllTodoTaskStatus()
        {
            try
            {
                var todoTaskStatuses = _todoContext.RetrieveAllTodoTaskStatus();
                TodoLogger.LogSuccessRetrieveAll();
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status200OK, todoTaskStatuses);
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailRetrieveAll(ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        #endregion
    }
}

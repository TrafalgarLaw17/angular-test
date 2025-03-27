using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using FRA_Todolist_prj.Models.Contexts.Services;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Controllers
{
    [Route("api/[controller]")]
    [Tags("Todo List")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _todoContext;
        public TodoController(DBContext dbContext, ILogger<TodoContext> todoContextLogger)
        {
            _todoContext = new TodoContext(dbContext.GetConnection().ConnectionString);
            TodoLogger.ConfigureLogger(todoContextLogger);
        }


        #region (1) CREATE [HttpPost]
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Todo item", Description = "This API allows you to create a new Todo item.")]
        public IActionResult CreateTodo([FromBody] Todo todo)
        {
            try
            {
                if (todo == null || string.IsNullOrWhiteSpace(todo.TodoTitle))
                {
                    TodoLogger.LogFailCreate(0);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest, 
                        new { error = new ArgumentNullException(nameof(todo.TodoTitle)).Message });
                }

                if (todo.TodoTaskStatusId == 0)
                {
                    TodoLogger.LogFailInvalidStatusId(todo.TodoTaskStatusId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status422UnprocessableEntity, 
                        new { error = new ArgumentNullException(nameof(todo.TodoTaskStatusId)).Message });
                }

                todo.TodoCreatedAt = DateTime.UtcNow;
                todo.TodoUpdatedAt = DateTime.UtcNow;

                var (createdTodo, isOk, isException) = _todoContext.CreateTodo(todo);

                if (isException)
                {
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError,
                        new { error = "An error occurred while creating the Todo." });
                }

                if (!isOk || createdTodo == null || createdTodo.TodoId == 0)
                {
                    TodoLogger.LogFailCreate(0);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest, 
                         new { error = "Invalid data: One or more fields failed validation." });
                }

                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status201Created,
                    new { title = createdTodo.TodoTitle });
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailCreate(0, ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, 
                    new { error = ex.Message });
            }
        }
        #endregion


        #region (2.1) RETRIEVE [HttpGet]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all item records", Description = "This API allows you to get all records.")]
        public IActionResult RetrieveAllTodo()
        {
            try
            {
                var (retrievedAllTodo, isOk, isException) = _todoContext.RetrieveAllTodo();

                if (isException)
                {
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
                }

                if (!isOk || retrievedAllTodo == null || !retrievedAllTodo.Any())
                {
                    TodoLogger.LogFailRetrieveAll();
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status404NotFound);
                }

                var response = retrievedAllTodo.Select(todo => new
                {
                    todo.TodoId,
                    todo.TodoTitle,
                    todo.TodoDescription,
                    todo.TodoTaskStatusId,
                    todo.TodoTaskStatusName,
                    todo.TodoCreatedAt,
                    todo.TodoUpdatedAt
                }).ToList();

                TodoLogger.LogSuccessRetrieveAll();
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                TodoLogger.LogException(ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        #endregion


        #region (2.2) RETRIEVE [HttpGet("{todoId}")]
        [HttpGet("{todoId}")]
        [SwaggerOperation(Summary = "Get item record by ID", Description = "This API retrieves a specific todo record based on the provided ID.")]
        public IActionResult RetrievedByIdTodo(int todoId)
        {
            try
            {
                var (retrievedByIdTodo, isOk, isException) = _todoContext.RetrievedByIdTodo(todoId);

                if (isException)
                {
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest, 
                        new { error = "Invalid data: One or more fields failed validation." });
                }


                if (!isOk || retrievedByIdTodo == null || retrievedByIdTodo.TodoId == 0)
                {
                    TodoLogger.LogFailRetrieveById(todoId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status404NotFound);
                }

                var response = new
                {
                    retrievedByIdTodo.TodoId,
                    retrievedByIdTodo.TodoTitle,
                    retrievedByIdTodo.TodoDescription,
                    retrievedByIdTodo.TodoTaskStatusId,
                    retrievedByIdTodo.TodoTaskStatusName,
                    retrievedByIdTodo.TodoCreatedAt,
                    retrievedByIdTodo.TodoUpdatedAt
                };

                TodoLogger.LogSuccessRetrieveById(todoId);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailRetrieveById(todoId, ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, 
                    new { error = ex.Message });
            }
        }
        #endregion


        #region (3) UPDATE [HttpPut("{todoId}")]
        [HttpPut("{todoId}")]
        [SwaggerOperation(Summary = "Update existing item by ID", Description = "This API updates an existing todo record based on the provided ID.")]
        public IActionResult UpdateByIdTodo(int todoId, [FromBody] Todo? todo)
        {
            if (todo == null)
            {
                TodoLogger.LogExceptionIsException(todoId);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest,
                    new { error = "Invalid data: One or more fields failed validation." });
            }

            try
            {
                if (string.IsNullOrWhiteSpace(todo.TodoTitle))
                {
                    TodoLogger.LogFailUpdateNoChanges(todoId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
                }

                if (todo.TodoTaskStatusId <= 0)
                {
                    TodoLogger.LogFailInvalidStatusId(todo.TodoTaskStatusId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
                }

                var updatedTodo = _todoContext.UpdateByIdTodo(todoId, todo);

                if (updatedTodo.isException)
                {
                    TodoLogger.LogException($"Failed to update Todo ID {todoId}");
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
                }

                if (!updatedTodo.isOk || updatedTodo.data == null)
                {
                    TodoLogger.LogFailRetrieveById(todoId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
                }

                TodoLogger.LogSuccessUpdate(todoId);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status200OK, updatedTodo.data);
            }
            catch (Exception ex)
            {
                TodoLogger.LogException(ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status400BadRequest);
            }
        }
        #endregion


        #region (4) ARCHIVE [HttpDelete("{todoId}")]
        [HttpDelete("{todoId}")]
        [SwaggerOperation(Summary = "Archive existing item by ID", Description = "This API archives an existing todo record based on the provided ID.")]
        public IActionResult ArchiveTodo(int todoId)
        {
            try
            {
                var archivedTodo = _todoContext.ArchiveByIdTodo(todoId);

                if (archivedTodo.data == null || !archivedTodo.isOk)
                {
                    TodoLogger.LogFailRetrieveById(todoId);
                    return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status404NotFound);
                }

                var response = new
                {
                    archivedTodo.data.TodoId,
                    archivedTodo.data.TodoTitle,
                    archivedTodo.data.TodoDescription,
                    archivedTodo.data.TodoTaskStatusId,
                    archivedTodo.data.TodoTaskStatusName,
                    archivedTodo.data.TodoCreatedAt,
                    archivedTodo.data.TodoUpdatedAt
                };

                TodoLogger.LogSuccessDelete(todoId);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status200OK,
                    new { message = "The Todo item has been successfully deleted." });
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailDelete(todoId, ex.Message);
                return ResponseBuilderUtil.BuildResponse(_todoContext, StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        #endregion
    }
}

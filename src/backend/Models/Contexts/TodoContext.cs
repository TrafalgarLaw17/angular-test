using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Models.Contexts
{
    public class TodoContext : BaseContext
    {
        private readonly ResponseHttpContext _responseHttpContext;
        private readonly TodoTaskStatusContext _todoTaskStatusContext;
        private readonly TodoValidation _todoValidation;

        public TodoContext(string connectionString) : base(connectionString)
        {
            _responseHttpContext = new ResponseHttpContext(connectionString);
            _todoTaskStatusContext = new TodoTaskStatusContext(connectionString);
            _todoValidation = new TodoValidation(connectionString);
        }
        public ResponseHttp GetResponseHttpByCode(int statusCode) => _responseHttpContext.GetResponseHttpByCode(statusCode);
        public List<TodoTaskStatus> RetrieveAllTodoTaskStatus() => _todoTaskStatusContext.RetrieveAllTodoTaskStatus();


        #region (1) CREATE - POST method
        public (Todo? data, bool isOk, bool isException) CreateTodo(Todo todo)
        {
            Todo content = new Todo();
            bool isOk = false;
            bool isException = false;
            
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO TODO_TABLE (TODO_TITLE, TODO_DESCRIPTION, TODO_TASK_STATUS_ID, TODO_IS_ARCHIVE, TODO_CREATED_AT, TODO_UPDATED_AT) " +
                            "VALUES (@TodoTitle, @TodoDescription, @TodoTaskStatusId, @TodoIsArchive, @TodoCreatedAt, @TodoUpdatedAt); " +
                            "SELECT LAST_INSERT_ID();";
                
                MySqlCommand sql = new MySqlCommand(query, connection);
                try
                {
                    sql.Parameters.Add(new MySqlParameter("@TodoTitle", todo.TodoTitle));
                    sql.Parameters.Add(new MySqlParameter("@TodoDescription", todo.TodoDescription));
                    sql.Parameters.Add(new MySqlParameter("@TodoTaskStatusId", todo.TodoTaskStatusId));
                    sql.Parameters.Add(new MySqlParameter("@TodoIsArchive", todo.TodoIsArchive));
                    sql.Parameters.Add(new MySqlParameter("@TodoCreatedAt", DateTime.UtcNow));
                    sql.Parameters.Add(new MySqlParameter("@TodoUpdatedAt", DateTime.UtcNow));

                    if (!TodoIsColumnExistsUtil.CreateTable(query, sql))
                    {
                        return (null, false, false);
                    }

                    int lastInsertId = Convert.ToInt32(sql.ExecuteScalar());
                    if (lastInsertId > 0)
                    {
                        var retrievedData = RetrievedByIdTodo(lastInsertId).data;
                        if (retrievedData != null)
                        {
                            content = retrievedData;
                        }

                        isOk = true; 
                        TodoLogger.LogSuccessCreate(lastInsertId);
                    }
                }
                catch (Exception ex)
                {
                    isException = true; 
                    TodoLogger.LogFailCreate(0, ex.Message);
                }
                finally
                {
                    sql.Dispose();
                    connection.Close();
                }
            }
            
            return (content, isOk, isException);
        }
        #endregion


        #region (2.1) RETRIEVE - GET method - ALL (Only Active Items)
        public (List<Todo> data, bool isOk, bool isException) RetrieveAllTodo()
        {
            List<Todo> list = new List<Todo>();
            bool isOk = false;
            bool isException = false;

            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    string query = "SELECT t.TODO_ID, t.TODO_TITLE, t.TODO_DESCRIPTION, t.TODO_TASK_STATUS_ID, ts.TODO_TASK_STATUS_NAME, t.TODO_IS_ARCHIVE, t.TODO_CREATED_AT, t.TODO_UPDATED_AT " +
                                "FROM TODO_TABLE t " +
                                "JOIN TODO_TASK_STATUS_TABLE ts ON t.TODO_TASK_STATUS_ID = ts.TODO_TASK_STATUS_ID " +
                                "WHERE t.TODO_IS_ARCHIVE = FALSE " +
                                "ORDER BY t.TODO_ID ASC, t.TODO_CREATED_AT ASC";  

                    MySqlCommand sql = new MySqlCommand(query, connection, transaction);
                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            Todo content = new Todo();
                            content.TodoId = Convert.ToInt32(row["TODO_ID"]);
                            content.TodoTitle = row["TODO_TITLE"].ToString();
                            content.TodoDescription = row["TODO_DESCRIPTION"]?.ToString();
                            content.TodoTaskStatusId = Convert.ToInt32(row["TODO_TASK_STATUS_ID"]);
                            content.TodoTaskStatusName = row["TODO_TASK_STATUS_NAME"].ToString();
                            content.TodoIsArchive = Convert.ToBoolean(row["TODO_IS_ARCHIVE"]);
                            
                            DateTime originalCreatedAt = Convert.ToDateTime(row["TODO_CREATED_AT"]);
                            DateTime originalUpdatedAt = Convert.ToDateTime(row["TODO_UPDATED_AT"]);

                            content.TodoCreatedAt = DateTime.SpecifyKind(originalCreatedAt, DateTimeKind.Utc);
                            content.TodoUpdatedAt = DateTime.SpecifyKind(originalUpdatedAt, DateTimeKind.Utc);

                            bool validationFailed = _todoValidation.ValidateTodo(content, originalCreatedAt, originalUpdatedAt, transaction);

                            if (validationFailed)
                            {
                                isException = true;
                                return (new List<Todo>(), false, true);
                            }

                            list.Add(content);
                        }
                        isOk = true;
                        reader.Close();
                        sql.Dispose();
                        transaction.Commit();
                    } 
                }
                catch (Exception ex)
                {
                    isException = true;
                    TodoLogger.LogException(ex.Message);
                    return (new List<Todo>(), false, true);
                }
                finally
                {
                    connection.Close();
                }
            }
            
            return (list, isOk, isException);
        }
        #endregion


       #region (2.2) RETRIEVE - GET method - BY ID
        public (Todo? data, bool isOk, bool isException) RetrievedByIdTodo(int todoId)
        {
            Todo content = new Todo();
            bool isOk = false;
            bool isException = false;
            
            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();
                    
                    string query = "SELECT t.TODO_ID, t.TODO_TITLE, t.TODO_DESCRIPTION, t.TODO_TASK_STATUS_ID, " +
                                "ts.TODO_TASK_STATUS_NAME, t.TODO_IS_ARCHIVE, t.TODO_CREATED_AT, t.TODO_UPDATED_AT " +
                                "FROM TODO_TABLE t " +
                                "JOIN TODO_TASK_STATUS_TABLE ts ON t.TODO_TASK_STATUS_ID = ts.TODO_TASK_STATUS_ID " +
                                "WHERE t.TODO_ID = @TodoId AND t.TODO_IS_ARCHIVE = FALSE LIMIT 1";

                    MySqlCommand sql = new MySqlCommand(query, connection, transaction);
                    sql.Parameters.Add(new MySqlParameter("@TodoId", todoId));
                    MySqlDataReader reader = sql.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        content.TodoId = Convert.ToInt32(reader["TODO_ID"]);
                        content.TodoTitle = reader["TODO_TITLE"].ToString();
                        content.TodoDescription = reader["TODO_DESCRIPTION"]?.ToString();
                        content.TodoTaskStatusId = Convert.ToInt32(reader["TODO_TASK_STATUS_ID"]);
                        content.TodoTaskStatusName = reader["TODO_TASK_STATUS_NAME"].ToString();

                        DateTime originalCreatedAt = Convert.ToDateTime(reader["TODO_CREATED_AT"]);
                        DateTime originalUpdatedAt = Convert.ToDateTime(reader["TODO_UPDATED_AT"]);

                        content.TodoCreatedAt = DateTime.SpecifyKind(originalCreatedAt, DateTimeKind.Utc);
                        content.TodoUpdatedAt = DateTime.SpecifyKind(originalUpdatedAt, DateTimeKind.Utc);

                        bool validationFailed = _todoValidation.ValidateTodo(content, originalCreatedAt, originalUpdatedAt, transaction);
                        if (validationFailed)
                        {
                            isException = true;
                            return (null, false, true);
                        }

                        isOk = true;
                        reader.Close();
                        sql.Dispose();
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    isException = true;
                    TodoLogger.LogException(ex.Message);
                    return (null, false, true);
                }
                finally
                {
                    connection.Close();
                }
            }
            
            return (content, isOk, isException);
        }
        #endregion


        #region (3) UPDATE - PUT method - BY ID
        public (Todo? data, bool isOk, bool isException) UpdateByIdTodo(int todoId, Todo todo)
        {
            Todo? content = null;
            bool isOk = false;
            bool isException = false;

            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();

                    if (_todoValidation.IsRecordArchived("TODO_TABLE", "TODO_ID", todoId))
                    {
                        return (null, false, false);
                    }

                    string query = "UPDATE TODO_TABLE " +
                                "SET TODO_TITLE = @Title, TODO_DESCRIPTION = @Description, TODO_TASK_STATUS_ID = @StatusId, TODO_UPDATED_AT = UTC_TIMESTAMP() " +
                                "WHERE TODO_ID = @TodoId";

                    MySqlCommand sql = new MySqlCommand(query, connection);
                    sql.Parameters.Add(new MySqlParameter("@Title", todo.TodoTitle));
                    sql.Parameters.Add(new MySqlParameter("@Description", todo.TodoDescription));
                    sql.Parameters.Add(new MySqlParameter("@StatusId", todo.TodoTaskStatusId));
                    sql.Parameters.Add(new MySqlParameter("@TodoId", todoId));

                    if (!TodoIsColumnExistsUtil.UpdateTable(query, sql))
                    {
                        return (null, false, true); 
                    }

                    int rowsAffected = sql.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        content = RetrievedByIdTodo(todoId).data;
                        isOk = true;
                        TodoLogger.LogSuccessUpdate(todoId);
                    }

                    sql.Dispose();
                }
                catch (Exception ex)
                {
                    isException = true;
                    TodoLogger.LogExceptionDatabase(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        
            return (content, isOk, isException);
        }
        #endregion


        #region (4) DELETE (ARCHIVE) - UPDATE method (SET TODO_IS_ARCHIVE = TRUE)
        public (Todo? data, bool isOk, bool isException) ArchiveByIdTodo(int todoId)
        {
            Todo? content = new Todo();
            bool isOk = false;
            bool isException = false;

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "UPDATE TODO_TABLE SET TODO_IS_ARCHIVE = TRUE, TODO_UPDATED_AT = UTC_TIMESTAMP() " +
                            "WHERE TODO_ID = @TodoId AND TODO_IS_ARCHIVE = FALSE"; 

                MySqlCommand sql = new MySqlCommand(query, connection);
                try
                {
                    sql.Parameters.Add(new MySqlParameter("@TodoId", todoId));

                    int rowsAffected = sql.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        content = RetrievedByIdTodo(todoId).data;
                        isOk = true;
                        TodoLogger.LogSuccessDelete(todoId);
                    }
                }
                catch (Exception ex)
                {
                    isException = true;
                    TodoLogger.LogFailDelete(todoId, ex.Message);
                }
                finally
                {
                    sql.Dispose();
                    connection.Close();
                }
            }

            return (content, isOk, isException);
        }
        #endregion
    }
}

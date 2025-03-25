using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Models.Contexts
{
    public interface ITodoTaskStatusContext
    {
        List<TodoTaskStatus> RetrieveAllTodoTaskStatus();
    }
    public class TodoTaskStatusContext : BaseContext
    {
        public TodoTaskStatusContext(string connectionString) : base(connectionString){}

        #region (1) TODOTASKSTATUS - GET method - ALL
        public List<TodoTaskStatus> RetrieveAllTodoTaskStatus()
        {
            List<TodoTaskStatus> list = new List<TodoTaskStatus>();
            TodoTaskStatus content = new TodoTaskStatus();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT TODO_TASK_STATUS_ID, TODO_TASK_STATUS_NAME, TODO_TASK_STATUS_CREATED_AT, TODO_TASK_STATUS_UPDATED_AT FROM TODO_TASK_STATUS_TABLE";

                MySqlCommand sql = new MySqlCommand(query, connection);
                MySqlDataReader reader = sql.ExecuteReader();

                try
                {
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            content = new TodoTaskStatus();
                            content.TodoTaskStatusId = Convert.ToInt32(row["TODO_TASK_STATUS_ID"]);
                            content.TodoTaskStatusName = row["TODO_TASK_STATUS_NAME"].ToString();
                            content.TodoTaskStatusCreatedAt = Convert.ToDateTime(row["TODO_TASK_STATUS_CREATED_AT"]);
                            content.TodoTaskStatusUpdatedAt = Convert.ToDateTime(row["TODO_TASK_STATUS_UPDATED_AT"]);
                            list.Add(content);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TodoLogger.LogFailRetrieveAll(ex.Message);
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }
        #endregion
    }
}

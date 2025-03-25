using System;
using System.Data;
using MySql.Data.MySqlClient;
using FRA_Todolist_prj.Tools.Utils;
using FRA_Todolist_prj.Models.Request;

namespace FRA_Todolist_prj.Models.Contexts
{
    public interface ITodoValidation
    {
        bool IsRecordArchived(string tableName, string primaryKeyColumn, int recordId);
        bool ValidateTodo(Todo content, DateTime originalCreatedAt, DateTime originalUpdatedAt, MySqlTransaction transaction);
    }

    public class TodoValidation : BaseContext, ITodoValidation
    {
        public TodoValidation(string connectionString) : base(connectionString) {}

        #region (1) VALIDATION - Check if record is Archived
        public bool IsRecordArchived(string tableName, string primaryKeyColumn, int recordId)
        {
            bool isArchived = false;

            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT TODO_IS_ARCHIVE FROM {tableName} WHERE {primaryKeyColumn} = @RecordId LIMIT 1";

                    using (MySqlCommand sql = new MySqlCommand(query, connection))
                    {
                        sql.Parameters.Add(new MySqlParameter("@RecordId", recordId));
                        var result = sql.ExecuteScalar();
                        isArchived = result != null && Convert.ToBoolean(result);
                    }
                }
                catch (Exception ex)
                {
                    TodoLogger.LogExceptionDatabase(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return isArchived;
        }
        #endregion

        #region (2) VALIDATION - Validate Todo Data
        public bool ValidateTodo(Todo content, DateTime originalCreatedAt, DateTime originalUpdatedAt, MySqlTransaction transaction)
        {
            bool isCreatedAtMissing = content.TodoCreatedAt != originalCreatedAt;
            bool isUpdatedAtMissing = content.TodoUpdatedAt != originalUpdatedAt;

            if (string.IsNullOrEmpty(content.TodoTitle)) 
            {
                TodoLogger.LogFailTitle();
                return true; 
            }
            
            if (string.IsNullOrEmpty(content.TodoDescription))
            {
                TodoLogger.LogFailDescription();
                return true; 
            } 
            if (content.TodoTaskStatusId == 0) 
            {
                TodoLogger.LogFailTaskStatusId();
                return true; 
            }
            if (string.IsNullOrEmpty(content.TodoTaskStatusName))
            {
                TodoLogger.LogFailTaskStatusName();
                return true; 
            }
            if (isCreatedAtMissing)  
            {
                TodoLogger.LogFailCreatedAt();
                return true; 
            }
            if (isUpdatedAtMissing)
            {
                TodoLogger.LogFailUpdatedAt();
                return true; 
            }  

            return false; 
        }
        #endregion
    }
}

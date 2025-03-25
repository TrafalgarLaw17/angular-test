using MySql.Data.MySqlClient;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Tools.Utils
{
    public static class TodoIsColumnExistsUtil
    {
        #region (1) VALIDATION for TodoContext - Check if column exists
        public static bool CreateTable(string query, MySqlCommand sql)
        {
            var missingColumns = new List<string>();
            var missingParameters = new List<string>();

            var requiredColumns = new Dictionary<string, string>
            {
                { "TODO_TITLE", "@TodoTitle" },
                { "TODO_DESCRIPTION", "@TodoDescription" },
                { "TODO_TASK_STATUS_ID", "@TodoTaskStatusId" },
                { "TODO_IS_ARCHIVE", "@TodoIsArchive" },
                { "TODO_CREATED_AT", "@TodoCreatedAt" },
                { "TODO_UPDATED_AT", "@TodoUpdatedAt" }
            };

            foreach (var column in requiredColumns)
            {
                if (!query.Contains(column.Key))
                    missingColumns.Add(column.Key);
                if (!query.Contains(column.Value) || !sql.Parameters.Contains(column.Value))
                    missingParameters.Add(column.Value);
            }

            if (missingColumns.Any() || missingParameters.Any())
            {
                string missingInfo = "";
                if (missingColumns.Any())
                    missingInfo += $"Missing Columns: {string.Join(", ", missingColumns)}. ";
                if (missingParameters.Any())
                    missingInfo += $"Missing Parameters: {string.Join(", ", missingParameters)}.";

                TodoLogger.LogFailMissingColumn(missingInfo);
                return false;
            }

            return true;
        }

        public static bool UpdateTable(string query, MySqlCommand sql)
        {
            var missingColumns = new List<string>();
            var missingParameters = new List<string>();

            var requiredColumns = new Dictionary<string, string>
            {
                { "TODO_TITLE", "@Title" },
                { "TODO_DESCRIPTION", "@Description" },
                { "TODO_TASK_STATUS_ID", "@StatusId" },
                { "TODO_UPDATED_AT", "UTC_TIMESTAMP()" },
                { "TODO_ID", "@TodoId" }  
            };

            foreach (var column in requiredColumns)
            {
                if (!query.Contains(column.Key))
                    missingColumns.Add(column.Key);
                if (column.Value != "UTC_TIMESTAMP()" && (!query.Contains(column.Value) || !sql.Parameters.Contains(column.Value)))
                    missingParameters.Add(column.Value);
            }

            if (missingColumns.Any() || missingParameters.Any())
            {
                string missingInfo = "";
                if (missingColumns.Any())
                    missingInfo += $"Missing Columns: {string.Join(", ", missingColumns)}. ";
                if (missingParameters.Any())
                    missingInfo += $"Missing Parameters: {string.Join(", ", missingParameters)}.";

                TodoLogger.LogFailMissingColumn(missingInfo);
                return false;
            }

            return true;
        }
        #endregion
    }
}
﻿using Microsoft.Extensions.Logging;

namespace FRA_Todolist_prj.Tools.Utils
{
    public static class TodoLogger 
    {
        private static ILogger? _logger;

        public static void ConfigureLogger(ILogger logger)
        {
            _logger = logger;
        }

        #region Success Logs
        public static void LogSuccessCreate(int todoId)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 201 - Todo item created successfully with ID {todoId}.");
        }

        public static void LogSuccessRetrieveAll()
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - All Todo items have been retrieved.");
        }
         public static void LogSuccessRetrieveAll(string message)
        {
            _logger?.LogInformation($"INFO: {message}");
        }

        public static void LogSuccessRetrieveById(int todoId)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - Todo item with ID {todoId} has been retrieved.");
        }

        public static void LogSuccessUpdate(int todoId)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - Todo item with ID {todoId} has been updated.");
        }

        public static void LogSuccessDelete(int todoId)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - Todo item with ID {todoId} was successfully deleted.");
        }

        public static void LogSuccessResponse(int intMessage)
        {
            _logger?.LogInformation($"INFO: {intMessage}");
        }

        public static void LogFailTitle()
        {
            _logger?.LogWarning($"ERROR: [FAIL] statusCode: 400 - Title is missing");
        }
        
        public static void LogFailDescription()
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 400 - Description is missing");
        }

        public static void LogFailTaskStatusId()
        {
            _logger?.LogError("Validation failed: TaskStatusId is invalid.");
        }

        public static void LogFailTaskStatusName()
        {
            _logger?.LogError("Validation failed: TaskStatusName is missing or invalid.");
        }

        public static void LogFailCreatedAt()
        {
            _logger?.LogError("Validation failed: CreatedAt timestamp is incorrect.");
        }

        public static void LogFailUpdatedAt()
        {
            _logger?.LogError("Validation failed: UpdatedAt timestamp is incorrect.");
        }
        #endregion

        #region Failure/Error Logs
        public static void LogFailRetrieveAll()
        {
            _logger?.LogError("ERROR: [FAIL] statusCode: 500 - Error retrieving Todo items.");
        }
        public static void LogFailRetrieveAll(string error)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - Error retrieving Todo items: {error}.");
        }

        public static void LogFailRetrieveById(int todoId)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - Error retrieving Todo item with ID {todoId}.");
        }
        public static void LogFailRetrieveById(int todoId, string error)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - Error retrieving Todo item with ID {todoId}: {error}.");
        }
        
        public static void LogFailCreate(int todoId)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 400 - Error creating Todo item with ID {todoId}.");
        }
        public static void LogFailCreate(int todoId, string error)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 400 - Error creating Todo item with ID {todoId}: {error}.");
        }

        public static void LogFailDelete(int todoId, string error)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 400 - Error deleting Todo item with ID {todoId}: {error}.");
        }

        public static void LogFailUpdateNoChanges(int todoId)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - No records updated for ID {todoId}.");
        }

        public static void LogFailInvalidStatus(int todoId, string todoTaskStatusName)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - Invalid status '{todoTaskStatusName}' for Todo item with ID {todoId}.");
        }

        public static void LogFailInvalidStatusId(int todoTaskStatusId)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - Invalid TodoTaskStatus Name. Returned: {todoTaskStatusId}");
        }

         public static void LogFailInvalidStatusName(string? todoTaskStatusName)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - Invalid TodoTaskStatus Name. Returned: {todoTaskStatusName}");
        }

        public static void LogFailMissingStatusName(string? todoTaskStatusName)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - TodoTaskStatus Name cannot be null or empty. Returned: {todoTaskStatusName}");
        }

        public static void LogFailMissingColumn()
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - Missing column.");
        }

        public static void LogFailUnauthorizedAction(int todoId)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 403 - Unauthorized action for Todo item with ID {todoId}.");
        }
        #endregion

        #region Exception Logs
        public static void LogException(string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - An exception occurred: {message}");
        }

        public static void LogExceptionMissingFields(int todoId)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 400 - [EXCEPTION] Required fields are missing in update request (ID {todoId}).");
        }

        public static void LogExceptionIsException(int todoId)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - An exception occurred: {todoId}");
        }
        public static void LogExceptionIsException(string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - An exception occurred: {message}");
        }

        public static void LogExceptionIsOk(string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - An exception occurred: {message}");
        }

        public static void LogExceptionDatabase()
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - [EXCEPTION] Database error: null value");
        }
        public static void LogExceptionDatabase(string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - [EXCEPTION] Database error: {message}");
        }
        public static void LogWarning(string message)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - {message}");
        }
        #endregion
        public static void LogInfoReceivedTodoTaskStatusId(int todoTaskStatusId)
        {
            _logger?.LogInformation($"INFO: Received TodoTaskStatusId: {todoTaskStatusId}");
        }

        public static void LogInfo(string message)
        {
            _logger?.LogInformation($"INFO: {message}");
        }
        public static void LogFailMissingColumn(string details)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 400 - Missing column or parameter. Details: {details}");
        }
    }

    public static class GenericLogger
    {
        private static ILogger? _logger;

        public static void ConfigureLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static void LogInfo(string className, string message)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - Response received from {className}: {message}");
        }

        public static void LogError(string className, string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - API request failed in {className}. {message}");
        }

        public static void LogException(string className, string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - Exception during API call in {className}: {message}");
        }

                public static void LogAppUrl(string? appUrl)
        {
            if (string.IsNullOrEmpty(appUrl))
            {
                _logger?.LogWarning("WARN: ASPNETCORE_URLS is not set.");
            }
            else
            {
                _logger?.LogInformation($"INFO: ASPNETCORE_URLS is set to: {appUrl}");
            }
        }

        public static void LogMigrationStarted()
        {
            _logger?.LogInformation("INFO: Starting database migration process.");
        }

        public static void LogMigrationSuccess()
        {
            _logger?.LogInformation("INFO: Database migration completed successfully.");
        }

        public static void LogMigrationFailure(string error)
        {
            _logger?.LogError($"ERROR: Database migration failed. Reason: {error}");
        }

        public static void LogRevertingMigration()
        {
            _logger?.LogInformation("INFO: Reverting to the previous migration version.");
        }

        public static void LogRevertSuccess(long previousVersion)
        {
            _logger?.LogInformation($"INFO: Successfully reverted to migration version {previousVersion}.");
        }

        public static void LogRevertFailure(string error)
        {
            _logger?.LogError($"ERROR: Migration revert failed. Reason: {error}");
        }

        public static void LogRevertWarning()
        {
            _logger?.LogWarning($"WARN: No previous migration found to revert.");
        }

        public static void LogDeletingTables()
        {
            _logger?.LogInformation("INFO: Deleting all tables.");
        }

        public static void LogDeleteSuccess()
        {
            _logger?.LogInformation("INFO: All tables deleted successfully.");
        }

        public static void LogDeleteFailure(string error)
        {
            _logger?.LogError($"ERROR: Table deletion failed. Reason: {error}");
        }

        public static void LogAuthSuccess(string username)
        {
            _logger?.LogInformation($"INFO: [SUCCESS] statusCode: 200 - User '{username}' authenticated successfully.");
        }

        public static void LogAuthFailure()
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: 401 - Authentication failed: Malformed authentication header or Invalid credentials provided.");
        }

        public static void LogAuthException(string message)
        {
            _logger?.LogError($"ERROR: [FAIL] statusCode: 500 - Exception in BasicAuth: {message}");
        }

        public static void LogAuthUnauthorized(int statusCode)
        {
            _logger?.LogWarning($"WARN: [FAIL] statusCode: {statusCode} - Unauthorized access attempt.");
        }
    }
}

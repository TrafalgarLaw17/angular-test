using FluentMigrator;

namespace FRA_Todolist_prj.Migrations
{
    [Migration(101)]
    public class _0101ResponseHttp : Migration
    {
        public override void Up()
        {
            string seedDataQuery =
            " INSERT INTO RESPONSE_HTTP_TABLE (RESPONSE_HTTP_STATUS_CODE, RESPONSE_HTTP_TITLE, RESPONSE_HTTP_STATUS, RESPONSE_HTTP_MESSAGE) " +
            " VALUES " +
            " (200, 'OK', 'SUCCESS', 'The request was successful.'), " +
            " (201, 'Created', 'SUCCESS', 'The resource was successfully created.'), " +
            " (204, 'No Content', 'SUCCESS', 'The request was successful but there is no content to return.'), " +
            " (400, 'Bad Request', 'FAILED', 'The request could not be understood by the server due to malformed syntax.'), " +
            " (401, 'Unauthorized', 'FAILED', 'Authentication is required to access the resource.'), " +
            " (403, 'Forbidden', 'FAILED', 'The server understood the request but refuses to authorize it.'), " +
            " (404, 'Not Found', 'FAILED', 'The requested resource could not be found.'), " +
            " (405, 'Method Not Allowed', 'FAILED', 'The method specified in the request is not allowed for the resource.'), " +
            " (408, 'Request Timeout', 'FAILED', 'The server timed out waiting for the request.'), " +
            " (409, 'Conflict', 'FAILED', 'The request could not be completed due to a conflict with the current state of the resource.'), " +
            " (422, 'Unprocessable Entity', 'FAILED', 'The request was well-formed but was unable to be followed due to semantic errors.'), " +
            " (429, 'Too Many Requests', 'FAILED', 'The user has sent too many requests in a given amount of time.'), " +
            " (500, 'Internal Server Error', 'FAILED', 'An unexpected error occurred on the server.'); " ;
            Execute.Sql(seedDataQuery);
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM RESPONSE_HTTP_TABLE;");
        }
    }
}

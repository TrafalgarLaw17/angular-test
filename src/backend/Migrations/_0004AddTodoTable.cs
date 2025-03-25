using FluentMigrator;

namespace FRA_Todolist_prj.Migrations
{
    [Migration(4)] 
    public class _0004AddTodoTable : Migration
    {
        public override void Up()
        {
            string sqlCommand =
            " CREATE TABLE TODO_TABLE ( " +
            " TODO_ID bigint(20) NOT NULL AUTO_INCREMENT, " +
            " TODO_TITLE varchar(255) NOT NULL, " +
            " TODO_DESCRIPTION varchar(255), " +
            " TODO_TASK_STATUS_ID bigint(20) NOT NULL, " +
            " TODO_IS_ARCHIVE boolean NOT NULL DEFAULT FALSE, " + 
            " TODO_CREATED_AT timestamp NOT NULL DEFAULT current_timestamp, " +
            " TODO_UPDATED_AT timestamp NOT NULL DEFAULT current_timestamp ON UPDATE current_timestamp, " +
            " PRIMARY KEY (TODO_ID), " + 
            " FOREIGN KEY (TODO_TASK_STATUS_ID) REFERENCES TODO_TASK_STATUS_TABLE(TODO_TASK_STATUS_ID) ON DELETE CASCADE " + 
            " ) ENGINE=InnoDB; ";
            Execute.Sql(sqlCommand);

            string sqlIdxTCACommand =
            " CREATE INDEX IDX_TODO_CREATED_AT ON TODO_TABLE(TODO_CREATED_AT); " ;
            Execute.Sql(sqlIdxTCACommand);

            string sqlIdxTISCommand = 
            " CREATE INDEX IDX_TODO_IS_ARCHIVE ON TODO_TABLE(TODO_IS_ARCHIVE); " ;
            Execute.Sql(sqlIdxTISCommand);
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS TODO_TABLE;");
        }
    }
}

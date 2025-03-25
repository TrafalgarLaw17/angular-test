using FluentMigrator;

namespace FRA_Todolist_prj.Migrations
{
    [Migration(3)]
    public class _0003AddTodoTaskStatusTable : Migration
    {
        public override void Up()
        {
            string sqlCommand =
            " CREATE TABLE TODO_TASK_STATUS_TABLE ( " +
            " TODO_TASK_STATUS_ID bigint(20) NOT NULL AUTO_INCREMENT, " +
            " TODO_TASK_STATUS_CODE varchar(255) NOT NULL UNIQUE, " +
            " TODO_TASK_STATUS_NAME varchar(255) NOT NULL UNIQUE, " +
            " TODO_TASK_STATUS_CREATED_AT timestamp NOT NULL DEFAULT current_timestamp, " +
            " TODO_TASK_STATUS_UPDATED_AT timestamp NOT NULL DEFAULT current_timestamp ON UPDATE current_timestamp, " +
            " PRIMARY KEY (TODO_TASK_STATUS_ID) " +
            " ) ENGINE=InnoDB; ";
            Execute.Sql(sqlCommand);
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS TODO_TASK_STATUS_TABLE;");
        }
    }
}
using FluentMigrator;

namespace FRA_Todolist_prj.Migrations.Seeders
{
    [Migration(103)] 
    public class _0103TodoTaskStatusTable : Migration
    {
        public override void Up()
        {
            string seedData =
            " INSERT INTO TODO_TASK_STATUS_TABLE (TODO_TASK_STATUS_CODE, TODO_TASK_STATUS_NAME) " +
            " VALUES " +
            " ('_pending', 'Pending'), " +
            " ('_working', 'Working'), " +
            " ('_completed', 'Completed'); ";
            Execute.Sql(seedData);
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM TODO_TASK_STATUS_TABLE;");
        }
    }
}
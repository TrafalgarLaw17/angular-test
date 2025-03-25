using FluentMigrator;

namespace FRA_Todolist_prj.Migrations.Seeders
{
    [Migration(104)] 
    public class _0004TodoTableSeeder : Migration
    {
        public override void Up()
        {
            string seedData =
            " INSERT INTO TODO_TABLE (TODO_TITLE, TODO_DESCRIPTION, TODO_TASK_STATUS_ID, TODO_CREATED_AT, TODO_UPDATED_AT) " +
            " VALUES " +
            " ('Complete project', 'Finish ASP.NET CRUD API', 1, UTC_TIMESTAMP(), UTC_TIMESTAMP()), " +  
            " ('Study FluentMigrator', 'Learn how to manage database migrations', 2, UTC_TIMESTAMP(), UTC_TIMESTAMP()), " + 
            " ('Deploy application', 'Upload to production server', 1, UTC_TIMESTAMP(), UTC_TIMESTAMP()); "; 
            
            Execute.Sql(seedData);
        }

        public override void Down()
        {
            Execute.Sql("DELETE FROM TODO_TABLE;");
        }
    }
}

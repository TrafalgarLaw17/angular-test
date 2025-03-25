using FluentMigrator;

namespace FRA_Todolist_prj.Migrations
{
    [Migration(1)]
    public class _0001AddResponseHttpTable : Migration 
    {
        public override void Up()
        {
            string sqlCommand =
            "CREATE TABLE RESPONSE_HTTP_TABLE ( " +
            "RESPONSE_HTTP_ID BIGINT(20) NOT NULL AUTO_INCREMENT, " +
            "RESPONSE_HTTP_STATUS_CODE INT(11) NOT NULL, " +
            "RESPONSE_HTTP_TITLE VARCHAR(255) NOT NULL, " +
            "RESPONSE_HTTP_STATUS VARCHAR(255) NOT NULL, " +
            "RESPONSE_HTTP_MESSAGE VARCHAR(255) NOT NULL, " +
            "RESPONSE_HTTP_CREATED_AT DATETIME DEFAULT CURRENT_TIMESTAMP, " +
            "RESPONSE_HTTP_UPDATED_AT DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, " +
            "PRIMARY KEY (RESPONSE_HTTP_ID) " +
            ") ENGINE=InnoDB;";
            Execute.Sql(sqlCommand);

            string sqlIdxCommand = 
            "CREATE INDEX IDX_RESPONSE_HTTP_STATUS_CODE ON RESPONSE_HTTP_TABLE(RESPONSE_HTTP_STATUS_CODE); ";
            Execute.Sql(sqlIdxCommand);
        }
        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS RESPONSE_HTTP_TABLE;");
        }
    }    
}
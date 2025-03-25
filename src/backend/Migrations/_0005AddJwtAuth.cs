// using FluentMigrator;

// namespace FRA_Todolist_prj.Migrations
// {
//     [Migration(2)] 
//     public class _0002AddAccountTable : Migration
//     {
//         public override void Up()
//         {
//             string sqlCommand =
//             " CREATE TABLE ACCOUNT_TABLE ( " +
//             " ACCOUNT_ID bigint(20) NOT NULL AUTO_INCREMENT, " +
//             " ACCOUNT_USERNAME varchar(32) NOT NULL UNIQUE, " +
//             " ACCOUNT_EMAIL varchar(255) NOT NULL UNIQUE, " +
//             " ACCOUNT_PASSWORD varchar(255) NOT NULL, " +
//             " ACCOUNT_CREATED_AT datetime DEFAULT current_timestamp, " +
//             " ACCOUNT_UPDATED_AT datetime DEFAULT current_timestamp ON UPDATE current_timestamp, " + 
//             " PRIMARY KEY (ACCOUNT_ID) " + 
//             " ) ENGINE=InnoDB; ";
//             Execute.Sql(sqlCommand);
//         }

//         public override void Down()
//         {
//             Execute.Sql("DROP TABLE IF EXISTS ACCOUNT_TABLE;");
//         }
//     }
// }

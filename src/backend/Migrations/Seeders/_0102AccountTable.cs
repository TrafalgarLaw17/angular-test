using FluentMigrator;
using BCrypt.Net;

[Migration(102)]
public class _0102AccountTableSeeder : Migration
{
    public override void Up()
    {
        string password1 = BCrypt.Net.BCrypt.HashPassword("pass@123");
        string password2 = BCrypt.Net.BCrypt.HashPassword("pass@123");
        string adminPassword = BCrypt.Net.BCrypt.HashPassword("admin@123");

        string seedData = $@"
        INSERT INTO ACCOUNT_TABLE (ACCOUNT_USERNAME, ACCOUNT_EMAIL, ACCOUNT_PASSWORD, ACCOUNT_CREATED_AT, ACCOUNT_UPDATED_AT)
        VALUES 
        ('franz1', 'sample1user@gmail.com', '{password1}', UTC_TIMESTAMP(), UTC_TIMESTAMP()),
        ('franz2', 'sample2user@gmail.com', '{password2}', UTC_TIMESTAMP(), UTC_TIMESTAMP()),
        ('admin', 'admin@gmail.com', '{adminPassword}', UTC_TIMESTAMP(), UTC_TIMESTAMP());
        ";
        Execute.Sql(seedData);
    }

    public override void Down()
    {
        Execute.Sql("DELETE FROM ACCOUNT_TABLE;");
    }
}

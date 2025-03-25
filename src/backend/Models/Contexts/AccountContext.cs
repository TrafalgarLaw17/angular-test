using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Models.Contexts
{
    public interface IAccountContext
    {
        List<Account> RetrieveAllAccounts();
    }

    public class AccountContext : BaseContext
    {
        public AccountContext(string connectionString) : base(connectionString) {}

        #region (1) ACCOUNT - GET method - ALL
        public List<Account> RetrieveAllAccounts()
        {
            List<Account> list = new List<Account>();
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT ACCOUNT_ID, ACCOUNT_USERNAME, ACCOUNT_EMAIL, ACCOUNT_CREATED_AT, ACCOUNT_UPDATED_AT FROM ACCOUNT_TABLE";

                MySqlCommand sql = new MySqlCommand(query, connection);
                MySqlDataReader reader = sql.ExecuteReader();

                try
                {
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            Account account = new Account();
                            account.AccountId = Convert.ToInt32(row["ACCOUNT_ID"]);
                            account.AccountUsername = row["ACCOUNT_USERNAME"].ToString();
                            account.AccountEmail = row["ACCOUNT_EMAIL"].ToString();
                            account.AccountCreatedAt = Convert.ToDateTime(row["ACCOUNT_CREATED_AT"]);
                            account.AccountUpdatedAt = Convert.ToDateTime(row["ACCOUNT_UPDATED_AT"]);
                            list.Add(account);
                        }
                    }
                }
                catch (Exception ex)
                {
                    TodoLogger.LogFailRetrieveAll(ex.Message);
                }
                finally
                {
                    reader.Close();
                    connection.Close();
                }
            }
            return list;
        }
        #endregion
    }
}

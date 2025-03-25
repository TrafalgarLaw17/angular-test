using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Models.Contexts
{
    public interface IResponseHttpContext
    {
        List<ResponseHttp> RetrieveAllResponsesHttp();
        ResponseHttp GetResponseHttpByCode(int statusCode);
    }
    public class ResponseHttpContext : BaseContext
    {
        public ResponseHttpContext(string connectionString) : base(connectionString){}

        #region (1.1) HTTP RESPONSE - GET method - ALL
        public List<ResponseHttp> RetrieveAllResponsesHttp()
        {
            List<ResponseHttp> list = new List<ResponseHttp>();
            ResponseHttp content = new ResponseHttp();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT RESPONSE_HTTP_ID, RESPONSE_HTTP_STATUS_CODE, RESPONSE_HTTP_TITLE, RESPONSE_HTTP_STATUS, RESPONSE_HTTP_MESSAGE, RESPONSE_HTTP_CREATED_AT, RESPONSE_HTTP_UPDATED_AT FROM RESPONSE_HTTP_TABLE";

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
                            content = new ResponseHttp();
                            content.ResponseHttpId = Convert.ToInt64(row["RESPONSE_HTTP_ID"]);
                            content.ResponseHttpStatusCode = Convert.ToInt32(row["RESPONSE_HTTP_STATUS_CODE"]);
                            content.ResponseHttpTitle = row["RESPONSE_HTTP_TITLE"].ToString();
                            content.ResponseHttpStatus = row["RESPONSE_HTTP_STATUS"].ToString();
                            content.ResponseHttpMessage = row["RESPONSE_HTTP_MESSAGE"].ToString();
                            content.ResponseHttpCreatedAt = Convert.ToDateTime(row["RESPONSE_HTTP_CREATED_AT"]);
                            content.ResponseHttpUpdatedAt = Convert.ToDateTime(row["RESPONSE_HTTP_UPDATED_AT"]);
                            list.Add(content);
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

        #region (1.2) HTTP RESPONSE - GET method - By Status Code
        public ResponseHttp GetResponseHttpByCode(int statusCode)
        {
            ResponseHttp content = new ResponseHttp();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT RESPONSE_HTTP_STATUS_CODE, RESPONSE_HTTP_TITLE, RESPONSE_HTTP_STATUS, RESPONSE_HTTP_MESSAGE " +
                            "FROM RESPONSE_HTTP_TABLE " +
                            "WHERE RESPONSE_HTTP_STATUS_CODE = @StatusCode LIMIT 1";

                MySqlCommand sql = new MySqlCommand(query, connection);
                sql.Parameters.Add(new MySqlParameter("@StatusCode", statusCode));
                MySqlDataReader reader = sql.ExecuteReader();

                try
                {
                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            content = new ResponseHttp();
                            content.ResponseHttpStatusCode = Convert.ToInt32(row["RESPONSE_HTTP_STATUS_CODE"]);
                            content.ResponseHttpTitle = row["RESPONSE_HTTP_TITLE"].ToString();
                            content.ResponseHttpStatus = row["RESPONSE_HTTP_STATUS"].ToString();
                            content.ResponseHttpMessage = row["RESPONSE_HTTP_MESSAGE"].ToString();
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
            return content;
        }
        #endregion
    }
}

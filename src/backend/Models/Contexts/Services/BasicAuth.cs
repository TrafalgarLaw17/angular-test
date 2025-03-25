using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Text.Encodings.Web;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Tools.Utils;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FRA_Todolist_prj.Models.Contexts.Services
{
    public class BasicAuth : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly DBContext _databaseContext;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BasicAuth(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder, 
            DBContext databaseContext,
            IOptions<JsonSerializerOptions> jsonSerializerOptions
        ) : base(options, logger, encoder) 
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
            _jsonSerializerOptions = jsonSerializerOptions?.Value ?? throw new ArgumentNullException(nameof(jsonSerializerOptions));
        }

        #region HandleAuthenticateAsync
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                GenericLogger.LogAuthUnauthorized(StatusCodes.Status401Unauthorized);
                return await UnauthorizedResponse(StatusCodes.Status401Unauthorized);
            }

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                {
                    var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                    var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                    var credentials = decodedCredentials.Split(':');

                    if (credentials.Length != 2)
                    {
                        GenericLogger.LogAuthFailure();
                        return await UnauthorizedResponse(StatusCodes.Status401Unauthorized);
                    }

                    var accountUsername = credentials[0];
                    var accountEmail = credentials[1]; // Now checking email instead of password

                    if (!ValidateCredentials(accountUsername, accountEmail))
                    {
                        GenericLogger.LogAuthFailure();
                        return await UnauthorizedResponse(StatusCodes.Status401Unauthorized);
                    }

                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, accountUsername),
                        new Claim(ClaimTypes.Email, accountEmail)
                    };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    GenericLogger.LogAuthSuccess(accountUsername);
                    return AuthenticateResult.Success(ticket);
                }

                GenericLogger.LogAuthFailure();
                return await UnauthorizedResponse(StatusCodes.Status401Unauthorized);
            }
            catch (Exception e)
            {
                GenericLogger.LogAuthException(e.Message);
                return await UnauthorizedResponse(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        #region UnauthorizedResponse
        private async Task<AuthenticateResult> UnauthorizedResponse(int statusCode)
        {
            Response.StatusCode = statusCode;
            Response.ContentType = "application/json";

            var responseHttpContext = new ResponseHttpContext(_databaseContext.GetConnection().ConnectionString);
            var response = ResponseBuilderUtil.BuildResponse(responseHttpContext, statusCode);
            
            var jsonResult = response as JsonResult;
            var jsonString = JsonSerializer.Serialize(jsonResult?.Value, _jsonSerializerOptions);
            
            await Response.WriteAsync(jsonString);

            GenericLogger.LogAuthUnauthorized(statusCode);
            return AuthenticateResult.Fail(jsonString);
        }
        #endregion

        #region ValidateCredentials
        private bool ValidateCredentials(string accountUsername, string accountEmail)
        {
            try
            {
                using (var connection = _databaseContext.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM ACCOUNT_TABLE WHERE ACCOUNT_USERNAME = @AccountUsername AND ACCOUNT_EMAIL = @AccountEmail";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountUsername", accountUsername);
                        command.Parameters.AddWithValue("@AccountEmail", accountEmail);
                        var count = Convert.ToInt32(command.ExecuteScalar());

                        bool isValid = count > 0;
                        if (!isValid)
                        {
                            GenericLogger.LogAuthFailure();
                        }
                        return isValid;
                    }
                }
            }
            catch (Exception ex)
            {
                GenericLogger.LogAuthException(ex.Message);
                return false;
            }
        }
        #endregion
    }
}

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using FRA_Todolist_prj.Models.Contexts;
using FRA_Todolist_prj.Models.Request;
using FRA_Todolist_prj.Tools.Utils;

namespace FRA_Todolist_prj.Tools.Utils
{
    public static class ResponseBuilderUtil
    {
        public static ActionResult BuildResponse(TodoContext todoContext, int statusCode, object? data = null)
        {
            return BuildResponseInternal(todoContext, statusCode, data);
        }

        public static ActionResult BuildResponse(ResponseHttpContext responseHttpContext, int statusCode, object? data = null)
        {
            return BuildResponseInternal(responseHttpContext, statusCode, data);
        }

        public static ActionResult BuildResponse(DBContext dbContext, int statusCode, object? data = null)
        {
            return BuildResponseInternal(dbContext, statusCode, data);
        }

        public static ActionResult BuildResponse(AccountContext accountContext, int statusCode, object? data = null)
        {
            return BuildResponseInternal(accountContext, statusCode, data);
        }


        private static ActionResult BuildResponseInternal(dynamic context, int statusCode, object? data = null)
        {
            try
            {
                HttpStatusCode httpStatusCode = (HttpStatusCode)statusCode;
                ResponseHttp response = context.GetResponseHttpByCode((int)httpStatusCode);
                
                if (response == null || response.ResponseHttpStatusCode == 0)
                {
                    response = context.GetResponseHttpByCode(StatusCodes.Status500InternalServerError);
                }

                if (response == null || response.ResponseHttpStatusCode == 0)
                {
                    response = context.GetResponseHttpByCode(
                        statusCode == StatusCodes.Status404NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status500InternalServerError
                    );
                }

                var responseBody = new
                {
                    ResponseHttpStatusCode = response.ResponseHttpStatusCode,
                    ResponseHttpTitle = response.ResponseHttpTitle,
                    ResponseHttpStatus = response.ResponseHttpStatus,
                    ResponseHttpMessage = response.ResponseHttpMessage,
                    data = data
                };

                TodoLogger.LogSuccessResponse((int)httpStatusCode);

                return new JsonResult(responseBody)
                {
                    StatusCode = response.ResponseHttpStatusCode
                };
            }
            catch (Exception ex)
            {
                TodoLogger.LogFailRetrieveAll(ex.Message);

                ResponseHttp badResponse = context.GetResponseHttpByCode(StatusCodes.Status500InternalServerError);

                var fallbackResponse = new
                {
                    ResponseHttpStatusCode = badResponse.ResponseHttpStatusCode,
                    ResponseHttpTitle = badResponse.ResponseHttpTitle,
                    ResponseHttpStatus = badResponse.ResponseHttpStatus,
                    ResponseHttpMessage = badResponse.ResponseHttpMessage,
                    error = ex.Message
                };

                return new JsonResult(fallbackResponse)
                {
                    StatusCode = badResponse.ResponseHttpStatusCode
                };
            }
        }
    }
}

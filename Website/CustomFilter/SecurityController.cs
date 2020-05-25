using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace Website
{
    public class SecurityController : IActionFilter
    {
        private IUnitofWork _uow { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do Something After Method being executed
        }

        public SecurityController(IUnitofWork uow)
        {
            _uow = uow;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var response = new ApiResponseModel();
            var validations = new List<Validation>();
            var AuthToken = (context.HttpContext.Request.Headers.TryGetValue("AuthToken", out var authorizationToken)) ? authorizationToken.ToString() : string.Empty;
            var ip = context.HttpContext.Connection.RemoteIpAddress;

            try
            {
                if (string.IsNullOrEmpty(AuthToken))
                {
                    validations.Add(new Validation() { Key = "Token", Value = "Please send your Token to Access this resource" });
                }
                else
                {
                    var val = AuthToken.Split(".");

                    if (val.Length != int.Parse(_uow.GetAppSettings("TokenLength")))
                    {
                        validations.Add(new Validation() { Key = "Token", Value = "Incorrect Token Format" });
                    }
                    else
                    {
                        var UserToken = Helper.Decrypt(val[0], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize")));
                        var IpAddress = Helper.Decrypt(val[1], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize"))); ;
                        var ExecutedDate = Helper.Decrypt(val[2], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize"))); ;

                        //Full Token : MsJPOXWboRbZL0Z14auT8w==.X3M3Yl3MucZr+LXqJpS2aA==.7GgDQjrzTdLDVkw/2q6U3xYt3Kk7pKtWp1WHyMHhE3s=

                        //MsJPOXWboRbZL0Z14auT8w== --> Encrypt Tante Erni
                        if (UserToken != "tanteerni")
                        {
                            validations.Add(new Validation() { Key = "Token", Value = "Incorrect Token Value" });
                        }

                        //X3M3Yl3MucZr+LXqJpS2aA== --> ::1 (Localhost)
                        if (IpAddress != ip.ToString())
                        {
                            validations.Add(new Validation() { Key = "IpAddress", Value = "Different IP detected" });
                        }

                        //7GgDQjrzTdLDVkw/2q6U3xYt3Kk7pKtWp1WHyMHhE3s= --> 2020-05-22 13:35:53
                        var date = DateTime.Parse(ExecutedDate);
                        //if(date )
                    }
                }

                response.Validations = validations;

                if (response.Validations.Count > 0)
                {
                    context.HttpContext.Response.StatusCode = (int)ResponseMessageEnum.UnAuthorized;
                    context.Result = ApiHelper.Response(ResponseMessageEnum.UnAuthorized, response);
                }
            }
            catch
            {
                context.Result = ApiHelper.Response(ResponseMessageEnum.InternalServerError, response);
            }
        }
    }
}
using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Website
{
    public class SecuredController : IActionFilter
    {
        private IUnitofWork _uow { get; set; }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do Something After Method being executed
        }

        public SecuredController(IUnitofWork uow)
        {
            _uow = uow;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var response = new ApiResponseModel();
            var validations = new List<Validation>();
            var AuthToken = (context.HttpContext.Request.Headers.TryGetValue("AuthToken", out var authorizationToken)) ? authorizationToken.ToString() : string.Empty;
            var ip = context.HttpContext.Connection.RemoteIpAddress;
            var auditLoginRepo = new AuditLoginRepository(_uow);
            var currentData = new AuditLogin();

            try
            {
                if (string.IsNullOrEmpty(AuthToken))
                {
                    validations.Add(new Validation() { Key = "Token", Value = "Please send your Token to Access this resource" });
                }
                else
                {
                    var val = AuthToken.Split(".");

                    if (val.Length != int.Parse(_uow.GetAppSettings("SecuredTokenLength")))
                    {
                        validations.Add(new Validation() { Key = "Token", Value = "Incorrect Token Format" });
                    }
                    else
                    {
                        var UserClaim = Helper.Decrypt(val[0], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize")));
                        var UserToken = Helper.Decrypt(val[1], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize")));
                        var IpAddress = Helper.Decrypt(val[2], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize"))); ;
                        var ExecutedDate = Helper.Decrypt(val[3], _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize"))); ;

                        UserClaim UserClaimResult = Helper.JSONDeserialize<UserClaim>(UserClaim);

                        _uow.OpenConnection("dbConnection");

                        currentData = auditLoginRepo.ReadByLambda(x => x.UserID == UserClaimResult.UserID).FirstOrDefault();

                        if (currentData == null)
                        {
                            validations.Add(new Validation() { Key = "User", Value = "We couldnt find your user id" });
                        }
                        else
                        {
                            if (currentData.UserKey != UserToken)
                            {
                                validations.Add(new Validation() { Key = "User", Value = "Incorrect User Token" });
                            }
                            else
                            {
                                if (currentData.IPAddress != IpAddress)
                                {
                                    validations.Add(new Validation() { Key = "User", Value = "Oooppsss another user use your user id" });
                                }
                                else
                                {
                                    if (currentData.IPAddress != ip.ToString())
                                    {
                                        validations.Add(new Validation() { Key = "User", Value = "Oooppsss there is someone on middle" });
                                    }
                                    else
                                    {
                                        ////7GgDQjrzTdLDVkw/2q6U3xYt3Kk7pKtWp1WHyMHhE3s= --> 2020-05-22 13:35:53
                                        var date = DateTime.Parse(ExecutedDate);
                                    }
                                }
                            }
                        }
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

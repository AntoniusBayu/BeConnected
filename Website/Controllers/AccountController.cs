using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Website.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController
    {
        private IAccount _Account { get; set; }
        public AccountController(IServiceProvider serviceProvider)
        {
            this._Account = serviceProvider.GetRequiredService<IAccount>();
        }

        [HttpPost, Route("registerUser")]
        public IActionResult register(MasterUser data)
        {
            try
            {
                _Account.RegisterUser(data);

                return ApiResponse(ApiResullt.Ok, SaveSuccessful);
            }
            catch (Exception ex)
            {
                return ApiResponse(ApiResullt.Failed);
            }
        }

        [HttpPost, Route("login")]
        public IActionResult login(MasterUser data)
        {
            try
            {
                _Account.Login(data);

                return ApiResponse(ApiResullt.Ok, SaveSuccessful);
            }
            catch (Exception ex)
            {
                return ApiResponse(ApiResullt.Failed);
            }
        }
    }
}
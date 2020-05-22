using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Website
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
        [ServiceFilter(typeof(SecurityController))]
        public IActionResult register(MasterUser data)
        {
            try
            {
                var response = _Account.RegisterUser(data);

                return ApiResponse(ResponseMessageEnum.Success, response);
            }
            catch (Exception ex)
            {
                return ApiResponse(ResponseMessageEnum.InternalServerError, new ApiResponseModel() { Message = GlobalErrorMessage });
            }
        }

        [HttpPost, Route("login")]
        [ServiceFilter(typeof(SecurityController))]
        public IActionResult login(MasterUser data)
        {
            try
            {
                var response = _Account.Login(data);

                return ApiResponse(ResponseMessageEnum.Success, response);
            }
            catch (Exception ex)
            {
                return ApiResponse(ResponseMessageEnum.InternalServerError, new ApiResponseModel() { Message = GlobalErrorMessage });
            }
        }
    }
}
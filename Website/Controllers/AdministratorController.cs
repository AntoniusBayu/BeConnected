using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Website.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [ServiceFilter(typeof(SecuredController))]
    public class AdministratorController : BaseController
    {
        private IMasterCompany _Company { get; set; }
        public AdministratorController(IServiceProvider serviceProvider)
        {
            this._Company = serviceProvider.GetRequiredService<IMasterCompany>();
        }

        [HttpPost, Route("addCompany")]
        public IActionResult register(MasterCompany data)
        {
            try
            {
                var response = _Company.AddCompany(data);

                return ApiResponse(ResponseMessageEnum.Success, response);
            }
            catch (Exception ex)
            {
                return ApiResponse(ResponseMessageEnum.InternalServerError, new ApiResponseModel() { Message = GlobalErrorMessage });
            }
        }

        [HttpGet, Route("getAllCompany")]
        public IActionResult GetAll()
        {
            try
            {
                var response = _Company.GetAllCompany();

                return ApiResponse(ResponseMessageEnum.Success, response);
            }
            catch (Exception ex)
            {
                return ApiResponse(ResponseMessageEnum.InternalServerError, new ApiResponseModel() { Message = GlobalErrorMessage });
            }
        }
    }
}

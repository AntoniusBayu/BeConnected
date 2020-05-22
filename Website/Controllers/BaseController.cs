using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Website
{
    public abstract class BaseController : Controller
    {
        protected const string SaveSuccessful = "Data has been saved successfully";
        protected const string UpdateSuccessful = "Data has been updated successfully";
        protected const string DeleteSuccessful = "Data has been deleted successfully";
        protected const string GlobalErrorMessage = "Ooops Something went wrong!";

        protected enum ApiResullt
        {
            Ok = 1,
            Failed = 2
        }

        protected virtual IActionResult ApiResponse(ResponseMessageEnum code, ApiResponseModel model)
        {
            IActionResult response;

            Response.StatusCode = (int)code;
            response = ApiHelper.Response(code, result: model);

            return response;
        }
    }
}
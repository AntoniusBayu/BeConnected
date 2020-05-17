﻿using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
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

        protected virtual IActionResult ApiResponse(ApiResullt result, object data = null)
        {
            IActionResult response;

            if (result == ApiResullt.Failed)
            {
                response = BadRequest(GlobalErrorMessage);
            }
            else
            {
                response = Ok(new { dataObject = data });
            }
            return response;
        }
    }
}
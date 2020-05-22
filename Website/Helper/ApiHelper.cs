using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace Website
{
    public static class ApiHelper
    {
        private static string msg_200 = "OK";
        private static string msg_201 = "Created";
        private static string msg_204 = "No Content";
        private static string msg_400 = "Bad Request";
        private static string msg_404 = "UnAuthorized";
        private static string msg_500 = "Internal Server Error";

        public static JsonResult Response(ResponseMessageEnum code, ApiResponseModel result = null, string apiVersion = "1.0.0.0")
        {
            //Declare Variable
            result.Version = apiVersion;
            result.StatusCode = (int)code;

            //Get Message
            switch (code)
            {
                case ResponseMessageEnum.Success:
                    result.Message = msg_200;
                    break;
                case ResponseMessageEnum.Created:
                    result.Message = msg_201;
                    break;
                case ResponseMessageEnum.BadRequest:
                    result.Message = msg_400;
                    break;
                case ResponseMessageEnum.UnAuthorized:
                    result.Message = msg_404;
                    break;
                case ResponseMessageEnum.InternalServerError:
                    result.Message = msg_500;
                    break;
            }

            //Validations
            if (result.Validations != null)
            {
                if (result.Validations.Count > 0)
                {
                    result.StatusCode = (int)ResponseMessageEnum.NoContent;
                    result.Message = msg_204;
                }
            }

            return new JsonResult(result);
        }
    }
}

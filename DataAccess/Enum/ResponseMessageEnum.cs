using System.ComponentModel;

namespace DataAccess
{
    public enum ResponseMessageEnum
    {
        [Description("OK")]
        Success = 200,
        [Description("Created")]
        Created = 201,
        [Description("No Content")]
        NoContent = 204,
        [Description("Bad Request")]
        BadRequest = 400,
        [Description("UnAuthorized")]
        UnAuthorized = 401,
        [Description("Internal Server Error")]
        InternalServerError = 500
    }
}

using DataAccess;

namespace Business
{
    public interface IAccount
    {
        ApiResponseModel RegisterUser(MasterUser data);
        ApiResponseModel Login(MasterUser data, string ipAddress);
        MasterUser GetUserClaim(MasterUser data);
    }
}

using DataAccess;

namespace Business
{
    public interface IAccount
    {
        ApiResponseModel RegisterUser(MasterUser data);
        ApiResponseModel Login(MasterUser data);
        MasterUser GetUserClaim(MasterUser data);
    }
}

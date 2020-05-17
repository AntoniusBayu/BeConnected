using DataAccess;

namespace Business
{
    public interface IAccount
    {
        void RegisterUser(MasterUser data);
        bool Login(MasterUser data);
        MasterUser GetUserClaim(MasterUser data);
    }
}

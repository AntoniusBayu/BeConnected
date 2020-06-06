using DataAccess;

namespace Business
{
    public interface IMasterCompany
    {
        ApiResponseModel AddCompany(MasterCompany data);
        ApiResponseModel GetAllCompany();
    }
}

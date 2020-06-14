using DataAccess;

namespace Business
{
    public interface IMasterCompany
    {
        ApiResponseModel AddCompany(MasterCompany data);
        ApiResponseModel GetAllCompany();
        ApiResponseModel UpdateCompany(MasterCompany data);
        ApiResponseModel DeleteCompany(string CompanyID);
    }
}

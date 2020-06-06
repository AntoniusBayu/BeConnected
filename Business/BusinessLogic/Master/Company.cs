using DataAccess;
using System;
using System.Collections.Generic;

namespace Business
{
    public class Company : BaseBusinessLogic, IMasterCompany
    {
        private IUnitofWork _uow { get; set; }
        public Company(IUnitofWork uow)
        {
            _uow = uow;
        }
        public ApiResponseModel AddCompany(MasterCompany data)
        {
            var response = new ApiResponseModel();

            _uow.OpenConnection(base.SQLDBConn);

            var CompanyRepository = new MasterCompanyRepository(_uow);

            try
            {

                data.CompanyID = Helper.GeneratedID(Convert.ToInt32(_uow.GetAppSettings("LengthRandomString")), "COMP");
                data.IsActive = true;

                _uow.BeginTransaction();

                CompanyRepository.Insert(data);

                _uow.CommitTransaction();

                response.Message = "Company has been registered successfully";
                response.Result = data;

                return response;
            }
            catch
            {
                _uow.RollbackTransaction();
                throw;
            }
            finally
            {
                _uow.Dispose();
            }
        }

        public ApiResponseModel GetAllCompany()
        {
            var response = new ApiResponseModel();

            _uow.OpenConnection(base.SQLDBConn);

            var CompanyRepository = new MasterCompanyRepository(_uow);

            try
            {
                IList<MasterCompany> list = CompanyRepository.ReadByLambda(x => x.IsActive == true);
                response.Result = list;

                return response;
            }
            catch
            {
                throw;
            }
            finally
            {
                _uow.Dispose();
            }
        }
    }
}

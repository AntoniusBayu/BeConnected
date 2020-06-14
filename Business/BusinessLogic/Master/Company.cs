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

        public ApiResponseModel UpdateCompany(MasterCompany data)
        {
            var response = new ApiResponseModel();

            _uow.OpenConnection(base.SQLDBConn);

            var CompanyRepository = new MasterCompanyRepository(_uow);
            var currentData = new MasterCompany();

            try
            {
                currentData = CompanyRepository.GetSingleData(new MasterCompany { CompanyID = data.CompanyID });
                currentData.CompanyName = data.CompanyName;
                currentData.IsVerified = data.IsVerified;
                currentData.ModifiedDate = DateTime.Now;

                _uow.BeginTransaction();

                CompanyRepository.Update(currentData);

                _uow.CommitTransaction();

                response.Message = "Company has been updated successfully";
                response.Result = currentData;

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

        public ApiResponseModel DeleteCompany(string CompanyID)
        {
            var response = new ApiResponseModel();

            _uow.OpenConnection(base.SQLDBConn);

            var CompanyRepository = new MasterCompanyRepository(_uow);
            var currentData = new MasterCompany();

            try
            {
                currentData = CompanyRepository.GetSingleData(new MasterCompany { CompanyID = CompanyID });
                currentData.IsActive = false;

                _uow.BeginTransaction();

                CompanyRepository.Update(currentData);

                _uow.CommitTransaction();

                response.Message = "Company has been deleted successfully";
                response.Result = currentData;

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
    }
}

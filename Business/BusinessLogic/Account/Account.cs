using DataAccess;
using System;
using System.Collections.Generic;

namespace Business
{
    public class Account : BaseBusinessLogic, IAccount
    {
        private IUnitofWork _uow { get; set; }
        public Account(IUnitofWork uow)
        {
            _uow = uow;
        }
        public MasterUser GetUserClaim(MasterUser data)
        {
            _uow.OpenConnection(base.SQLDBConn);

            var currentData = new MasterUser();
            var UserRepository = new MasterUserRepository(_uow);

            try
            {
                currentData = UserRepository.GetSingleData(data);

                return currentData;
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

        public ApiResponseModel Login(MasterUser data)
        {
            var response = new ApiResponseModel();
            var validations = new List<Validation>();

            _uow.OpenConnection(base.SQLDBConn);

            var passPhrase = _uow.GetAppSettings("passPhrase");
            var hashAlgorithm = _uow.GetAppSettings("hashAlgorithm");
            var passwordIterations = _uow.GetAppSettings("passwordIterations");
            var initVector = _uow.GetAppSettings("initVector");
            var keySize = _uow.GetAppSettings("keySize");

            var currentData = new MasterUser();
            var UserRepository = new MasterUserRepository(_uow);

            try
            {
                currentData = UserRepository.GetSingleData(data);

                if (currentData == null)
                {
                    validations.Add(new Validation() { Key = "Username", Value = "We could not find your username, make sure you input the right username" });
                }
                else
                {
                    currentData.Password = Helper.Decrypt(currentData.Password, passPhrase, currentData.PasswordSalt, hashAlgorithm, Convert.ToInt32(passwordIterations), initVector, Convert.ToInt32(keySize));

                    if (data.Password != currentData.Password)
                    {
                        validations.Add(new Validation() { Key = "Password", Value = "Password is incorrect" });
                    }
                    else
                    {
                        response.Result = GetUserClaim(data);
                    }
                }

                response.Validations = validations;
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

        public ApiResponseModel RegisterUser(MasterUser data)
        {
            var response = new ApiResponseModel();
            var validations = new List<Validation>();

            _uow.OpenConnection(base.SQLDBConn);

            var saltText = Guid.NewGuid().ToString();
            var passPhrase = _uow.GetAppSettings("passPhrase");
            var hashAlgorithm = _uow.GetAppSettings("hashAlgorithm");
            var passwordIterations = _uow.GetAppSettings("passwordIterations");
            var initVector = _uow.GetAppSettings("initVector");
            var keySize = _uow.GetAppSettings("keySize");

            var currentData = new MasterUser();
            var UserRepository = new MasterUserRepository(_uow);

            try
            {
                currentData = UserRepository.GetSingleData(data);

                if (currentData != null)
                {
                    validations.Add(new Validation() { Key = "Username", Value = "Username already exists" });
                }
                else
                {
                    data.PasswordSalt = saltText;
                    data.Password = Helper.Encrypt(data.Password, passPhrase, saltText, hashAlgorithm, Convert.ToInt32(passwordIterations), initVector, Convert.ToInt32(keySize));
                    data.IsActive = true;
                    data.PasswordExpiredDate = DateTime.Now.AddMonths(6);

                    _uow.BeginTransaction();

                    UserRepository.Insert(data);

                    _uow.CommitTransaction();

                    response.Message = "User has been registered successfully";
                    response.Result = data;
                }

                response.Validations = validations;
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

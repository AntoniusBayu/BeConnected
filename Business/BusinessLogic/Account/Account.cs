using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public ApiResponseModel Login(MasterUser data, string ipAddress)
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
            var dataAudit = new AuditLogin();
            var AuthToken = string.Empty;
            var UserKey = string.Empty;
            var Ip = string.Empty;

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
                        dataAudit.UserID = currentData.UserID;
                        dataAudit.IPAddress = ipAddress;
                        AuditLogin(ref dataAudit);

                        UserKey = Helper.Encrypt(dataAudit.UserKey, _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize")));
                        Ip = Helper.Encrypt(ipAddress, _uow.GetAppSettings("passPhrase"), _uow.GetAppSettings("SaltFixed"), _uow.GetAppSettings("hashAlgorithm"), int.Parse(_uow.GetAppSettings("passwordIterations")), _uow.GetAppSettings("initVector"), int.Parse(_uow.GetAppSettings("keySize")));
                        AuthToken = UserKey + "." + Ip;

                        response.Result = new { AuthToken, Masteruser = GetUserClaim(data) };
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
                    data.UserID = Helper.GeneratedID(Convert.ToInt32(_uow.GetAppSettings("LengthRandomString")), data.UserName);
                    data.PasswordSalt = saltText;
                    data.Password = Helper.Encrypt(data.Password, passPhrase, saltText, hashAlgorithm, Convert.ToInt32(passwordIterations), initVector, Convert.ToInt32(keySize));
                    data.IsActive = true;
                    data.PasswordExpiredDate = DateTime.Now.AddMonths(6);
                    data.CreatedBy = data.UserID;

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

        private void AuditLogin(ref AuditLogin data)
        {
            var repo = new AuditLoginRepository(_uow);

            try
            {
                var UserID = data.UserID;
                var currentData = repo.ReadByLambda(x => x.UserID == UserID & x.IsActive).FirstOrDefault();

                if (currentData != null)
                {
                    data = currentData;
                    data.UserKey = Guid.NewGuid().ToString();
                    data.LoginDate = DateTime.Now;
                    data.IPAddress = data.IPAddress;
                    data.ModifiedBy = data.UserID;
                    data.ModifiedDate = DateTime.Now;

                    _uow.BeginTransaction();

                    repo.Update(data);

                    _uow.CommitTransaction();
                }
                else
                {
                    data.LogID = Helper.GeneratedID(Convert.ToInt32(_uow.GetAppSettings("LengthRandomString")), data.UserID);
                    data.UserKey = Guid.NewGuid().ToString();
                    data.IsActive = true;
                    data.CreatedBy = data.UserID;
                    data.CreatedDate = DateTime.Now;
                    data.LoginDate = DateTime.Now;

                    _uow.BeginTransaction();

                    repo.Insert(data);

                    _uow.CommitTransaction();
                }
            }
            catch (Exception ex)
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

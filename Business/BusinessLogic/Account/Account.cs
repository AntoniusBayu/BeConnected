using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

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

            var SaltFixed = _uow.GetAppSettings("SaltFixed");
            var passPhrase = _uow.GetAppSettings("passPhrase");
            var hashAlgorithm = _uow.GetAppSettings("hashAlgorithm");
            var passwordIterations = _uow.GetAppSettings("passwordIterations");
            var initVector = _uow.GetAppSettings("initVector");
            var keySize = _uow.GetAppSettings("keySize");

            MasterUser currentData = new MasterUser();
            var UserRepository = new MasterUserRepository(_uow);
            var dataAudit = new AuditLogin();
            var AuthToken = string.Empty;
            var UserKey = string.Empty;
            var Ip = string.Empty;
            var UserClaimResult = string.Empty;

            try
            {
                currentData = UserRepository.GetSingleData(data);

                if (currentData == null)
                {
                    validations.Add(new Validation() { Key = "Username", Value = "We could not find your username, make sure you input the right username" });
                }
                else
                {
                    data.Password = Helper.HashWithSalt(data.Password, currentData.PasswordSalt, SHA384.Create());

                    if (data.Password != currentData.Password)
                    {
                        validations.Add(new Validation() { Key = "Password", Value = "Password is incorrect" });
                    }
                    else
                    {
                        dataAudit.UserID = currentData.UserID;
                        dataAudit.IPAddress = ipAddress;
                        AuditLogin(ref dataAudit);

                        var userClaim = GetUserClaim(data);

                        var obj = new UserClaim
                        {
                            UserID = dataAudit.UserID,
                            UserEmail = userClaim.Email,
                            UserName = userClaim.UserName,
                            UserFullName = userClaim.FullName
                        };

                        UserClaimResult = Helper.Encrypt(Helper.JSONSerialize(obj), passPhrase, SaltFixed, hashAlgorithm, int.Parse(passwordIterations), initVector, int.Parse(keySize));
                        UserKey = Helper.Encrypt(dataAudit.UserKey, passPhrase, SaltFixed, hashAlgorithm, int.Parse(passwordIterations), initVector, int.Parse(keySize));
                        Ip = Helper.Encrypt(ipAddress, passPhrase, SaltFixed, hashAlgorithm, int.Parse(passwordIterations), initVector, int.Parse(keySize));

                        AuthToken = UserClaimResult + "." + UserKey + "." + Ip;

                        response.Result = new { AuthToken = AuthToken };
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

            var saltSize = _uow.GetAppSettings("SaltSize");

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
                    data.PasswordSalt = Helper.GenerateRandomCryptographicKey(Convert.ToInt32(saltSize));
                    data.Password = Helper.HashWithSalt(data.Password, data.PasswordSalt, SHA384.Create());
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

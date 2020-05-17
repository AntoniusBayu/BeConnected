using DataAccess;
using Microsoft.Extensions.Configuration;
using System;

namespace Business
{
    public class Account : BaseBusinessLogic, IAccount
    {
        private IUnitofWork _uow { get; set; }
        public Account(IUnitofWork uow, IConfiguration config) : base(config)
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

        public bool Login(MasterUser data)
        {
            _uow.OpenConnection(base.SQLDBConn);

            var passPhrase = GetAppSettings("passPhrase");
            var hashAlgorithm = GetAppSettings("hashAlgorithm");
            var passwordIterations = GetAppSettings("passwordIterations");
            var initVector = GetAppSettings("initVector");
            var keySize = GetAppSettings("keySize");

            var currentData = new MasterUser();
            var UserRepository = new MasterUserRepository(_uow);

            bool isValid = true;

            try
            {
                currentData = UserRepository.GetSingleData(data);
                currentData.Password = Helper.Decrypt(currentData.Password, passPhrase, currentData.PasswordSalt, hashAlgorithm, Convert.ToInt32(passwordIterations), initVector, Convert.ToInt32(keySize));

                if (data.Password != currentData.Password)
                {
                    isValid = false;
                }

                return isValid;
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

        public void RegisterUser(MasterUser data)
        {
            _uow.OpenConnection(base.SQLDBConn);

            var saltText = Guid.NewGuid().ToString();
            var passPhrase = GetAppSettings("passPhrase");
            var hashAlgorithm = GetAppSettings("hashAlgorithm");
            var passwordIterations = GetAppSettings("passwordIterations");
            var initVector = GetAppSettings("initVector");
            var keySize = GetAppSettings("keySize");

            var UserRepository = new MasterUserRepository(_uow);

            try
            {
                data.PasswordSalt = saltText;
                data.Password = Helper.Encrypt(data.Password, passPhrase, saltText, hashAlgorithm, Convert.ToInt32(passwordIterations), initVector, Convert.ToInt32(keySize));
                data.IsActive = true;
                data.PasswordExpiredDate = DateTime.Now.AddMonths(6);

                _uow.BeginTransaction();

                UserRepository.Insert(data);

                _uow.CommitTransaction();
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

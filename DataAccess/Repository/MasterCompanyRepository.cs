namespace DataAccess
{
    public class MasterCompanyRepository : RepoSQLDBRepository<MasterCompany>
    {
        public MasterCompanyRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

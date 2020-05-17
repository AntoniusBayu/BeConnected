namespace DataAccess
{
    public class MasterAdminCompanyRepository : RepoSQLDBRepository<MasterAdminCompany>
    {
        public MasterAdminCompanyRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

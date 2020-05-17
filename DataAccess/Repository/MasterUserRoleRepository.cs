namespace DataAccess
{
    public class MasterUserRoleRepository : RepoSQLDBRepository<MasterUserRole>
    {
        public MasterUserRoleRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

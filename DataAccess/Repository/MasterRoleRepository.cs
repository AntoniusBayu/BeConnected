namespace DataAccess
{
    public class MasterRoleRepository : RepoSQLDBRepository<MasterRole>
    {
        public MasterRoleRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

namespace DataAccess
{
    public class AuditLoginRepository : RepoSQLDBRepository<AuditLogin>
    {
        public AuditLoginRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

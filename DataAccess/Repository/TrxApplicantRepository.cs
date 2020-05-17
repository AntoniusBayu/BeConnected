namespace DataAccess
{
    public class TrxApplicantRepository : RepoSQLDBRepository<TrxApplicant>
    {
        public TrxApplicantRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

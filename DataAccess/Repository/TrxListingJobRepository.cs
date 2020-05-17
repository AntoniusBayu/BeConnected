namespace DataAccess
{
    public class TrxListingJobRepository : RepoSQLDBRepository<TrxListingJob>
    {
        public TrxListingJobRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

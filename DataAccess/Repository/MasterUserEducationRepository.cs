namespace DataAccess
{
    public class MasterUserEducationRepository : RepoSQLDBRepository<MasterUserEducation>
    {
        public MasterUserEducationRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

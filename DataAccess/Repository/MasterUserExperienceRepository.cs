namespace DataAccess
{
    public class MasterUserExperienceRepository : RepoSQLDBRepository<MasterUserExperience>
    {
        public MasterUserExperienceRepository(IUnitofWork uow) : base(uow)
        { }
    }
}

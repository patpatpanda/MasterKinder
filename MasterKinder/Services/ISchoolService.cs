using MasterKinder.Models;

public interface ISchoolService
{
    Task AddSchool(School school);
    Task<School> GetSchoolById(int id);
    Task<IEnumerable<School>> GetAllSchools();
    Task<SchoolDetailsDto> GetSchoolDetailsByGoogleName(string googlePlaceName);
}

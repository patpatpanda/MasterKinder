using MasterKinder.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterKinder.Services
{
    public interface ISchoolService
    {
        Task<IEnumerable<School>> GetAllSchools();
        Task<School> GetSchoolById(int id);
        Task AddSchool(School school);
        Task UpdateSchool(School school);
        Task DeleteSchool(int id);
        Task<SchoolDetailsDto> GetSchoolDetailsByGoogleName(string googlePlaceName); // Lägg till denna rad
    }
}

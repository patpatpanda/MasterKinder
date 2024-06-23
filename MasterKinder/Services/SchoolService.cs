using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SchoolService> _logger;

        public SchoolService(IServiceProvider serviceProvider, ILogger<SchoolService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<IEnumerable<School>> GetAllSchools()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();
                return await context.Schools.ToListAsync();
            }
        }

        public async Task<School> GetSchoolById(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();
                return await context.Schools.Include(s => s.Responses).FirstOrDefaultAsync(s => s.SchoolId == id);
            }
        }

        public async Task AddSchool(School school)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();
                context.Schools.Add(school);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateSchool(School school)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();
                context.Schools.Update(school);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSchool(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();
                var school = await context.Schools.FindAsync(id);
                if (school != null)
                {
                    context.Schools.Remove(school);
                    await context.SaveChangesAsync();
                }
            }
        }

        // Om du inte längre behöver GetSchoolDetailsByGoogleName, kan du ta bort denna metod.
        // public async Task<SchoolDetailsDto> GetSchoolDetailsByGoogleName(string googlePlaceName)
        // {
        //     // Implementation
        // }


        public static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
                return string.IsNullOrEmpty(b) ? 0 : b.Length;

            if (string.IsNullOrEmpty(b))
                return a.Length;

            var matrix = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                matrix[i, 0] = i;

            for (int j = 0; j <= b.Length; j++)
                matrix[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    var cost = (b[j - 1] == a[i - 1]) ? 0 : 1;
                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }

            Console.WriteLine($"Levenshtein distance between '{a}' and '{b}' is {matrix[a.Length, b.Length]}");

            return matrix[a.Length, b.Length];
        }


        public async Task<SchoolDetailsDto> GetSchoolDetailsByGoogleName(string googlePlaceAddress)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MrDb>();

                // Decode the incoming address
                var decodedAddress = Uri.UnescapeDataString(googlePlaceAddress).ToLower();

                // Sanitize the address by removing extra spaces
                string NormalizeAddress(string address) =>
                    string.Join(" ", address.Split(new char[] { ' ', ',', '.' }, StringSplitOptions.RemoveEmptyEntries));

                var normalizedGooglePlaceAddress = NormalizeAddress(decodedAddress);

                // Fetch all schools and normalize their addresses for comparison
                var schools = await context.Schools
                    .Include(s => s.Responses)
                    .ToListAsync();

                var school = schools
                    .FirstOrDefault(s => NormalizeAddress(s.Address.ToLower()) == normalizedGooglePlaceAddress);

                if (school == null)
                {
                    _logger.LogInformation($"No school found for address '{googlePlaceAddress}'");
                    return null;
                }

                var totalResponses = school.TotalResponses;
                var satisfactionPercentage = school.SatisfactionPercentage;
                var helhetsomdome = school.Responses
                    .Where(r => r.Question == "Helhetsomdöme")
                    .Select(r => r.Percentage)
                    .FirstOrDefault();
                var svarsfrekvens = satisfactionPercentage;
                var antalBarn = school.NumberOfChildren;

                return new SchoolDetailsDto
                {
                    SchoolName = school.SchoolName,
                    TotalResponses = totalResponses,
                    Helhetsomdome = helhetsomdome,
                    Svarsfrekvens = svarsfrekvens,
                    AntalBarn = antalBarn,
                    Address = school.Address,
                    Description = school.Description,
                    Principal = school.Principal,
                    Email = school.Email,
                    Phone = school.Phone,
                    Website = school.Website,
                    TypeOfService = school.TypeOfService,
                    OperatingArea = school.OperatingArea,
                    OrganizationForm = school.OrganizationForm,
                    ChildrenPerEmployee = school.ChildrenPerEmployee,
                    PercentageOfLicensedTeachers = school.PercentageOfLicensedTeachers,
                    Accessibility = school.Accessibility,
                    OrientationAndProfile = school.OrientationAndProfile,
                    IndoorDescription = school.IndoorDescription,
                    OutdoorDescription = school.OutdoorDescription,
                    FoodAndMealsDescription = school.FoodAndMealsDescription,
                    GoalsAndVisionDescription = school.GoalsAndVisionDescription,
                    // Lägg till andra detaljer om det behövs
                };
            }
        }



        private async Task<SchoolDetailsDto> GetSchoolDetailsByName(string schoolName)
        {
            var context = _serviceProvider.GetRequiredService<MrDb>();
            var school = await context.Schools
                .Include(s => s.Responses)
                .FirstOrDefaultAsync(s => s.SchoolName.ToLower() == schoolName.ToLower());

            if (school == null)
            {
                _logger.LogInformation($"No school found for name '{schoolName}'");
                return null;
            }

            var totalResponses = school.TotalResponses;
            var satisfactionPercentage = school.SatisfactionPercentage;
            var helhetsomdome = school.Responses
                .Where(r => r.Question == "Helhetsomdöme")
                .Select(r => r.Percentage)
                .FirstOrDefault();
            var svarsfrekvens = satisfactionPercentage;
            var antalBarn = school.NumberOfChildren;

            return new SchoolDetailsDto
            {
                SchoolName = school.SchoolName,
                TotalResponses = totalResponses,
                Helhetsomdome = helhetsomdome,
                Svarsfrekvens = svarsfrekvens,
                AntalBarn = antalBarn,
                // Add other details if needed
            };
        }









    }
}
   







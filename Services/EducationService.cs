using Dapper;
using GreenDonut.Data;
using PortfolioWebsite.Database;
using PortfolioWebsite.Objects;
using PortfolioWebsite.Objects.InputObjects;

namespace PortfolioWebsite.Services
{
    public class EducationService
    {
        private MawaddaDbContext _context;
        public EducationService(MawaddaDbContext context)
        {
            _context = context;
        }

        public async Task<List<Education>> GetAllEducationAsync()
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var sql = @"SELECT * FROM Education";
            var EducationQuery = await conn.QueryAsync<Education>(sql);
            List<Education> educationList = [.. EducationQuery];
            return educationList;
        }

        public async Task<Education> GetEducationByIdAsync(string Id)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            var parms = new DynamicParameters();
            parms.Add("@Id", Id);
            var sql = @"SELECT * FROM Education WHERE id = @Id";
            Education education = await conn.QueryFirstAsync<Education>(sql, parms);
            return education;
        }

        public async Task InsertEducationAsync(InputEducation educationInput)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();
            Education education = new Education(
                educationInput.Institution,
                educationInput.StudyProgram,
                educationInput.StudyProgramType,
                educationInput.StartDate,
                educationInput.OnGoing,
                educationInput.EndDate);
            var parms = new DynamicParameters();

            parms.Add("@Id", education.Id);
            parms.Add("@Institution", education.Institution);
            parms.Add("@StudyProgram", education.StudyProgram);
            parms.Add("@StudyProgramType", education.StudyProgramType);
            parms.Add("@StartDate", education.StartDate, System.Data.DbType.Date);
            parms.Add("@OnGoing", education.OnGoing, System.Data.DbType.Boolean);
            parms.Add("@EndDate", education.EndDate, System.Data.DbType.Date);

            var sql = @"INSERT INTO Education (id, institution, studyprogram, studyprogramtype, startdate, ongoing, enddate)
                        VALUES (@Id, @Institution, @StudyProgram, @StudyProgramType, @StartDate, @OnGoing, @EndDate)";

            await conn.ExecuteAsync(sql, parms);
        }

        public async Task UpdateEducationAsync(Education education)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();

            parms.Add("@Id", education.Id);
            parms.Add("@Institution", education.Institution);
            parms.Add("@StudyProgram", education.StudyProgram);
            parms.Add("@StudyProgramType", education.StudyProgramType);
            parms.Add("@StartDate", education.StartDate, System.Data.DbType.Date);
            parms.Add("@OnGoing", education.OnGoing, System.Data.DbType.Boolean);
            parms.Add("@EndDate", education.EndDate, System.Data.DbType.Date);

            var sql = @"UPDATE Education
                        SET institution = @Institution,
                            studyprogram = @StudyProgram,
                            studyprogramtype = @StudyProgramType,
                            startdate = @StartDate,
                            ongoing = @OnGoing,
                            enddate = @EndDate
                        WHERE id = @Id";

            await conn.ExecuteAsync(sql, parms);
        }

        public async Task DeleteEducationAsync(string Id)
        {
            var conn = await _context.ConnectionFactory.CreateConnection();

            var parms = new DynamicParameters();
            parms.Add("@Id", Id);

            var sql = @"DELETE FROM Education WHERE id = @Id";

            await conn.ExecuteAsync(sql, parms);
        }
    }
}

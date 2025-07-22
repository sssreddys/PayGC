using Compliance_Dtos.Dashboard;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance_Repository.Dashboard
{
    public class DashboardRepository:IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<DashboardDocumentDto>> GetDashboardDataAsync()
        {
            try
            {
                using var db = new SqlConnection(_connectionString);
                var result = await db.QueryAsync<DashboardDocumentDto>(
                    "GetDashboardDocumentCounts",
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch (SqlException ex)
            {
                // Log to file/db if needed
                throw new Exception("Database error occurred while fetching dashboard data.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred in the repository.", ex);
            }
        }

    }
}

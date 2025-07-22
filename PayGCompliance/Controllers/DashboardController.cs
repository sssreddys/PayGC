using Compliance_Services.AuditedAndTemplate;
using Compliance_Dtos.Dashboard;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using Compliance_Services.Dashboard;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;



[Authorize]
[Route("api/dashboard")]
[ApiController]
 public class DashboardController : ControllerBase
    {

        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpGet("document-counts")]
        public async Task<IActionResult> GetDashboardDocumentCounts()
        {
            try
            {
                var data = await _service.GetDashboardAsync();
                return Ok(new ApiResponse<IEnumerable<DashboardDocumentDto>>
                {
                    Success = true,
                    Message = "Dashboard data fetched successfully.",
                    Data = data
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "Database error. Please try again later.",
                    Data = ex.Message // or null if you want to hide details
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An unexpected error occurred.",
                    Data = ex.Message // or null
                });
            }
        }


    }



using Compliance_Dtos.Common;
using Compliance_Dtos.Policies;
using Compliance_Services.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Data.SqlClient;

namespace PayGCompliance.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _service;

        public PolicyController(IPolicyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] AddPolicyDto dto)
        {
            var createdBy = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrWhiteSpace(createdBy))
                return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized: CreatedBy not found in token."));

            dto.CreatedBy = createdBy;

            try
            {
                var newId = await _service.AddPolicyAsync(dto);

                if (newId == -5)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Duplicate BR Numbers not allowed."));

                return Ok(ApiResponse<int>.SuccessResponse(newId, "Policy added successfully."));
            }
            catch (SqlException ex) when (ex.Number == 51000)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user. The 'CreatedBy' user does not exist."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] UpdatePolicyDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<string>.ErrorResponse("Mismatched ID between route and body."));

            var performedBy = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrWhiteSpace(performedBy))
                return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized: PerformedBy not found in token."));

            dto.PerformedBy = performedBy;

            try
            {
                var result = await _service.UpdatePolicyAsync(dto);

                if (result == -5)
                    return BadRequest(ApiResponse<string>.ErrorResponse("Duplicate BR Numbers not allowed."));
                if (result == 0)
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found."));

                return Ok(ApiResponse<int>.SuccessResponse(result, "Policy updated successfully."));
            }
            catch (SqlException ex) when (ex.Number == 51001)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user. The 'PerformedBy' user does not exist."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            try
            {
                var result = await _service.GetPolicyByIdAsync(id);

                if (result == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found."));
                }

                return Ok(ApiResponse<GetPolicyDto>.SuccessResponse(result, "Policy fetched successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int size = 10,
            [FromQuery] string? search = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? status = null)
        {
            try
            {
                if (page <= 0 || size <= 0)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Page number and page size must be greater than 0."));
                }

                var result = await _service.GetAllPoliciesAsync(page, size, search, fromDate, toDate, status);

                return Ok(ApiResponse<PagedResult<ListPolicyDto>>.SuccessResponse(result, "Policies fetched successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var performedBy = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrWhiteSpace(performedBy))
                return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized: PerformedBy not found in token."));

            try
            {
                var result = await _service.DeletePolicyAsync(id, performedBy);

                if (result == 0)
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found or already deleted."));

                return Ok(ApiResponse<int>.SuccessResponse(result, "Policy deleted successfully."));
            }
            catch (SqlException ex) when (ex.Number == 51002)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Invalid user. The 'PerformedBy' user does not exist."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }


        }

}

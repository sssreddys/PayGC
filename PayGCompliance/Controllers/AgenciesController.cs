using Compliance_Dtos.Agencies;
using Compliance_Dtos.Common;
using Compliance_Services.Agencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayGCompliance.Common;
using System.Security.Claims; // Assuming ApiResponse is in this namespace

namespace PayGCompliance.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/agencies")]
    public class AgenciesController : ControllerBase // Changed to ControllerBase, common for APIs
    {
        private readonly AgenciesService _service;

        public AgenciesController(AgenciesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] string? status = null
            )
        {

            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Page number and page size must be greater than 0."));
            }

            var data = await _service.GetAllAsync(pageNumber, pageSize, searchTerm ,fromDate,toDate,status);

            return Ok(new ApiResponse<PagedResult<AgencyGetDto>> // Changed DTO type
            {
                Success = true,
                Message = "Agencies fetched successfully.",
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var agency = await _service.GetByIdAsync(id);
            if (agency == null)
            {
                return NotFound(new ApiResponse<AgencyGetDto> // Changed DTO type
                {
                    Success = false,
                    Message = "Agency not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<AgencyGetDto> // Changed DTO type
            {
                Success = true,
                Message = "Agency found.",
                Data = agency
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AgencyAddDto agency)
        {
            var createdBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(createdBy))
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid user token."));
            }

            agency.CreatedBy = createdBy;

            try
            {
                var newAgency = await _service.AddAsync(agency);

                return CreatedAtAction(nameof(GetById), new { id = newAgency?.Id }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Agency added successfully.",
                    Data = newAgency!
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AgencyUpdateDto agency)
        {
            if (id != agency.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch between route and body."));

            var performedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(performedBy))
                return Unauthorized(ApiResponse<object>.ErrorResponse("User identity not found in token."));

            agency.PerformedBy = performedBy;

            try
            {
                var updatedAgency = await _service.UpdateAsync(agency);
                return Ok(ApiResponse<object>.SuccessResponse(updatedAgency, "Agency updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var performedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(performedBy))
                {
                    return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized: PerformedBy not found in token."));
                }

                var result = await _service.DeleteAsync(id, performedBy);

                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Agency not found or could not be deleted.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Agency deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

    }
}
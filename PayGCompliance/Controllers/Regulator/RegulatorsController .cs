// PayGCompliance.Controllers.Regulator.RegulatorsController
using Compliance_Dtos.Common;
using Compliance_Dtos.Regulator;
using Compliance_Services.Regulator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Security.Claims; // Assuming ApiResponse is defined here

namespace PayGCompliance.Controllers.Regulator
{
    [Authorize]
    [ApiController]
    [Route("api/regulators")]
    public class RegulatorsController : ControllerBase
    {
        private readonly RegulatorService _service;

        public RegulatorsController(RegulatorService service)
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
            [FromQuery] string? status = null)
        {

            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest(ApiResponse<string>.ErrorResponse("Page number and page size must be greater than 0."));
            }

            var result = await _service.GetAllAsync(pageNumber, pageSize, searchTerm, fromDate, toDate, status);

            return Ok(new ApiResponse<PagedResult<RegulatorGetDto>>
            {
                Success = true,
                Message = "Regulators fetched successfully.",
                Data = result
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var regulator = await _service.GetByIdAsync(id);
            if (regulator == null)
            {
                return NotFound(new ApiResponse<RegulatorGetDto> // Changed to RegulatorGetDto
                {
                    Success = false,
                    Message = "Regulator not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<RegulatorGetDto> // Changed to RegulatorGetDto
            {
                Success = true,
                Message = "Regulator found.",
                Data = regulator
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(RegulatorAddDto regulatorAddDto)
        {
            var createdBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(createdBy))
                return Unauthorized(ApiResponse<object>.ErrorResponse("User identity not found in token."));

            regulatorAddDto.CreatedBy = createdBy;

            try
            {
                var newRegulator = await _service.AddAsync(regulatorAddDto);
                return Ok(ApiResponse<RegulatorGetDto>.SuccessResponse(newRegulator, "Regulator added successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RegulatorUpdateDto regulatorUpdateDto)
        {
            if (id != regulatorUpdateDto.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("ID mismatch between route and body."));

            var performedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(performedBy))
                return Unauthorized(ApiResponse<object>.ErrorResponse("User identity not found in token."));

            regulatorUpdateDto.PerformedBy = performedBy;

            try
            {
                var updatedRegulator = await _service.UpdateAsync(regulatorUpdateDto);
                return Ok(ApiResponse<RegulatorGetDto>.SuccessResponse(updatedRegulator, "Regulator updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var performedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(performedBy))
                return Unauthorized(ApiResponse<string>.ErrorResponse("Unauthorized: PerformedBy not found in token."));

            try
            {
                var result = await _service.DeleteAsync(id, performedBy);

                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Regulator not found or could not be deleted."));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Regulator deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

    }
}
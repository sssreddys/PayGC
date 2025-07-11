﻿// PayGCompliance.Controllers.Regulator.RegulatorsController
using Compliance_Dtos.Regulator;
using Compliance_Services.Regulator;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common; // Assuming ApiResponse is defined here

namespace PayGCompliance.Controllers.Regulator
{
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var data = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);

            return Ok(new ApiResponse<IEnumerable<RegulatorGetDto>> // Changed to RegulatorGetDto
            {
                Success = true,
                Message = "Regulators fetched successfully.",
                Data = data
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
        public async Task<IActionResult> Add(RegulatorAddDto regulatorAddDto) // Accepts RegulatorAddDto
        {
            // ID should not be set by the client for an Add operation.
            // regulatorAddDto.Id = 0; // This line is not strictly needed if Id is [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

            try
            {
                var newRegulator = await _service.AddAsync(regulatorAddDto); // Pass RegulatorAddDto

                return Ok(new ApiResponse<RegulatorGetDto> // Returns RegulatorGetDto
                {
                    Success = true,
                    Message = "Regulator added successfully.",
                    Data = newRegulator!
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
        public async Task<IActionResult> Update(int id, RegulatorUpdateDto regulatorUpdateDto) // Accepts RegulatorUpdateDto
        {
            if (id != regulatorUpdateDto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "ID mismatch between route and body.",
                    Data = null
                });
            }

            try
            {
                var updatedRegulator = await _service.UpdateAsync(regulatorUpdateDto); // Pass RegulatorUpdateDto

                if (updatedRegulator == null)
                {
                    return NotFound(new ApiResponse<RegulatorGetDto>
                    {
                        Success = false,
                        Message = "Regulator not found or update failed.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<RegulatorGetDto> // Returns RegulatorGetDto
                {
                    Success = true,
                    Message = "Regulator updated successfully.",
                    Data = updatedRegulator
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string performedBy)
        {
            try
            {
                var result = await _service.DeleteAsync(id, performedBy);

                if (!result)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Regulator not found or could not be deleted.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Regulator deleted successfully.",
                    Data = result
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
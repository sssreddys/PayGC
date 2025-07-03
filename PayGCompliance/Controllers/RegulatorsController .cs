using Compliance_Dtos;
using Compliance_Services.Regulator;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

namespace PayGCompliance.Controllers
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

            return Ok(new ApiResponse<IEnumerable<RegulatorDto>>
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
                return NotFound(new ApiResponse<RegulatorDto>
                {
                    Success = false,
                    Message = "Regulator not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<RegulatorDto>
            {
                Success = true,
                Message = "Regulator found.",
                Data = regulator
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(RegulatorDto regulator)
        {
            regulator.Id = 0;

            try
            {
                var newRegulator = await _service.AddAsync(regulator);

                return Ok(new ApiResponse<object>
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
        public async Task<IActionResult> Update(int id, RegulatorDto regulator)
        {
            if (id != regulator.Id)
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
                var updatedRegulator = await _service.UpdateAsync(regulator);

                return Ok(new ApiResponse<object>
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
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Regulator id not found.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Regulator deleted successfully.",
                Data = null
            });
        }
    }
}

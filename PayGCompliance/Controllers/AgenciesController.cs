using Compliance_Dtos.Agencies;
using Compliance_Services.Agencies;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common; // Assuming ApiResponse is in this namespace

namespace PayGCompliance.Controllers
{
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
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var data = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);

            return Ok(new ApiResponse<IEnumerable<AgencyGetDto>> // Changed DTO type
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
        public async Task<IActionResult> Add([FromBody] AgencyAddDto agency) // Changed input DTO, added [FromBody]
        {
            // Id is not expected from client on Add, will be set by DB
            // agency.Id = 0; // This line is not needed if you use a dedicated AgencyAddDto

            try
            {
                var newAgency = await _service.AddAsync(agency);

                // Use CreatedAtAction for proper REST response for resource creation
                return CreatedAtAction(nameof(GetById), new { id = newAgency?.Id }, new ApiResponse<object>
                {
                    Success = true,
                    Message = "Agency added successfully.",
                    Data = newAgency!
                });
            }
            catch (Exception ex)
            {
                // Consider more specific exception handling for different error codes
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AgencyUpdateDto agency) // Changed input DTO, added [FromBody]
        {
            if (id != agency.Id)
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
                var updatedAgency = await _service.UpdateAsync(agency);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Agency updated successfully.",
                    Data = updatedAgency
                });
            }
            catch (Exception ex)
            {
                // Consider more specific exception handling for different error codes
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
                    // More specific messages based on the exception thrown by the repository
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
                // Catching specific exceptions from repository is better
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
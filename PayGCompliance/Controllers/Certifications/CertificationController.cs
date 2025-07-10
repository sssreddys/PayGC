using Compliance_Dtos.BoardResolution;
using Compliance_Dtos.Certifications;
using Compliance_Dtos.Common;
using Compliance_Services.Certifications;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

namespace PayGCompliance.Controllers.Certifications
{
    [ApiController]
    [Route("api/certifications")]
    public class CertificationController : ControllerBase
    {
        private readonly ICertificationService _service;

        public CertificationController(ICertificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
           [FromQuery] string? search = null,
           [FromQuery] bool? status = null,
           [FromQuery] int page = 1,
           [FromQuery] int pageSize = 10,
           [FromQuery] DateTime? fromDate = null,
           [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var result = await _service.GetPagedAsync(search, status, page, pageSize, fromDate, toDate);
                return Ok(new ApiResponse<PagedResult<CertificationDto>>
                {
                    Success = true,
                    Message = "Certifications fetched successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error fetching data: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _service.GetByIdAsync(id);
                if (dto == null)
                {
                    return NotFound(new ApiResponse<CertificationDto>
                    {
                        Success = false,
                        Message = "Certification not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<CertificationDto>
                {
                    Success = true,
                    Message = "Certification found.",
                    Data = dto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error retrieving Certification: {ex.Message}",
                    Data = null
                });
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CertificationDto dto)
        {
            try
            {
                dto.CreatedAt = DateTime.UtcNow;

                var id = await _service.CreateAsync(dto, dto.AttachedCertificate);

                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Message = "Certification created successfully.",
                    Data = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error creating certification: {ex.Message}",
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CertificationDto dto)
        {
            if (id != dto.CertificationId)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "ID in URL and body do not match.",
                    Data = null
                });
            }

            try
            {
                dto.UpdatedAt = DateTime.UtcNow;
                var result = await _service.UpdateAsync(dto, dto.AttachedCertificate);

                if (result < 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Certification not found.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Certification updated successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error updating Certification: {ex.Message}",
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
                if (result < 0)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Certification not found or could not be deleted.",
                        Data = null
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Certification deleted successfully.",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Error deleting Certification: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}


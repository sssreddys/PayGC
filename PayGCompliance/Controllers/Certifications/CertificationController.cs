using Compliance_Dtos.Certifications;
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
    }
}


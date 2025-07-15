using Compliance_Dtos.NetWorthCertificates;
using Compliance_Services.AuditedAndTemplate;
using Compliance_Services.NetWorthCertificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace PayGCompliance.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class NetWorthCertificatesController : ControllerBase
    {
        private readonly INetWorthService _service;

        public NetWorthCertificatesController(INetWorthService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateNetWorth dto)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(new { message = errorMessages });
            }

            try
            {
                byte[]? documentBytes = null;
                if (dto.AttachedDocument != null && dto.AttachedDocument.Length > 0)
                {
                    if (dto.AttachedDocument.Length > 500 * 1024) // Max 1MB
                        return BadRequest(new
                        {
                            success = false,
                            message = "Attached Document size should not exceed 500KB."

                        });

                    using var ms = new MemoryStream();
                    await dto.AttachedDocument.CopyToAsync(ms);
                    documentBytes = ms.ToArray();
                }
                var created_by = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

                var id = await _service.CreateAsync(dto, documentBytes,created_by!);
                return Ok(new
                {
                    success = true,
                    message = "NetWorth Certificate created successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


    }
}

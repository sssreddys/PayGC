using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Dtos.RbiNotifications;
using Compliance_Dtos.Regulator;
using Compliance_Services.RbiNotifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;
using System.Security.Claims;

namespace PayGCompliance.Controllers.RbiNotifications
{

    [ApiController]
    [Route("api/[controller]")]
    public class RbiNotificationsController : ControllerBase
    {
        private readonly IRbiNotificationService _service;
        public RbiNotificationsController(IRbiNotificationService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateRbiNotificationDto dto)
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
                            message = "File size should not exceed 500KB."
                        });

                    using var ms = new MemoryStream();
                    await dto.AttachedDocument.CopyToAsync(ms);
                    documentBytes = ms.ToArray();
                }
                var created_by = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var id = await _service.CreateAsync(dto, documentBytes, created_by);
                return Ok(new
                {
                    success = true,
                    message = "RBI notification created successfully."
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


        [HttpGet("rbi-notificationsPaged")]
        public async Task<IActionResult> GetRbiNotificationsAsync(
           [FromQuery(Name = "id")] int? rbiNotificationId,
           [FromQuery(Name = "search")] string? searchKeyword,
           [FromQuery(Name = "status")] string? statusFilter,
           [FromQuery(Name = "page")] int pageNumber = 1,
           [FromQuery(Name = "pageSize")] int recordsPerPage = 10,
           [FromQuery(Name = "fromDate")] DateTime? fromDate = null,
           [FromQuery(Name = "toDate")] DateTime? toDate = null
   )
        {
            // If ID is provided, return a single record
            if (rbiNotificationId.HasValue)
            {
                var financialRecord = await _service.GetByIdAsync(rbiNotificationId.Value);
                if (financialRecord == null)
                    return NotFound(new { message = "Record not found." });

                return Ok(new
                {
                    success = true,
                    data = financialRecord
                });
            }

            // Else, return paginated and filtered results
            var paginatedResult = await _service.GetPagedAsync(
                searchKeyword,
                statusFilter,
                pageNumber,
                recordsPerPage,
                fromDate,
                toDate
            );

            return Ok(new
            {
                success = true,
                data = paginatedResult
            });
        }

        [HttpPost("delete")]

        public async Task<IActionResult> Delete([FromBody] DeleteRequestDto dto)
        {
            try
            {
                var updatedBy = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or User Id missing." });
                var deleted = await _service.DeleteAsync(dto, updatedBy);

                if (deleted < 0)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Record not found"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Deleted successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the exception here if you have a logging framework
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateRbiNotificationDto dto)
        {
            try
            {
                byte[]? documentBytes = null;
                var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or user ID missing" });

                if (Request.Form.Files.Count > 0)
                {
                    var hpf = Request.Form.Files[0]; // safely get the uploaded file

                    using var memory = new MemoryStream();
                    hpf.CopyTo(memory);
                    documentBytes = memory.ToArray();
                }


                var updated = await _service.UpdateAsync(dto, documentBytes, updatedBy);
                if (updated == -1) return NotFound();
                return Ok(new
                {
                    success = true,
                    message = "RBI notification Updated successfully."
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

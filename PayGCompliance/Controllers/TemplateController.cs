﻿using Compliance_Dtos.AuditedAndTemplate;
using Compliance_Dtos.Common;
using Compliance_Services.AuditedAndTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PayGCompliance.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/template")]

    public class TemplateController :ControllerBase
    {
        private readonly IAuditedFinancialService _service;
        private string controller_name = "template";

        public TemplateController(IAuditedFinancialService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateAuditedTemplateDto dto)
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
                        {   success=false,
                            message = "Attached Document size should not exceed 500KB."

                        });

                    using var ms = new MemoryStream();
                    await dto.AttachedDocument.CopyToAsync(ms);
                    documentBytes = ms.ToArray();
                }

                var created_by = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var id = await _service.CreateAsync(dto, documentBytes, this.controller_name, created_by!);
                if (id < 0)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Error while creating Template."

                    });
                return Ok(new
                {
                    success = true,
                    message = "Template record created successfully."
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


        [HttpGet("templates_paged")]
        public async Task<IActionResult> GetAuditedFinancialsAsync(
         [FromQuery(Name = "id")] int? auditedFinancialId,
         [FromQuery(Name = "search")] string? searchKeyword,
         [FromQuery(Name = "status")] string? statusFilter,
         [FromQuery(Name = "page")] int pageNumber = 1,
         [FromQuery(Name = "pageSize")] int recordsPerPage = 10,
         [FromQuery(Name = "fromDate")] DateTime? fromDate = null,
         [FromQuery(Name = "toDate")] DateTime? toDate = null
     )
        {
            // If ID is provided, return a single record
            if (auditedFinancialId.HasValue)
            {
                var financialRecord = await _service.GetByIdAsync(auditedFinancialId.Value, this.controller_name);
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
                toDate,
                this.controller_name
            );

            return Ok(paginatedResult);
        }


        [HttpPost("update")]

        public async Task<IActionResult> Update([FromForm] UpdateAuditedTemplateDto dto)
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
                var updated = await _service.UpdateAsync(documentBytes, dto, updatedBy, this.controller_name);
                if (updated == -1) return NotFound();
                return Ok(new
                {
                    success = true,
                    message = "Template record Updated successfully."
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

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRequestDto dto)
        {
            try
            {
                var updatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(updatedBy))
                    return Unauthorized(new { message = "Invalid token or user ID missing" });

                var result = await _service.DeleteAsync(dto, updatedBy, this.controller_name);


                if (result == -1)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Record not found."
                    });
                }
                else if (result == -2)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Record already deleted."
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Deleted successfully."
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
    }
}

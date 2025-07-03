using Compliance_Dtos.AuditedFinancial;
using Compliance_Services.AuditedFincancial;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuditedFinancialController : ControllerBase
{
    private readonly IAuditedFinancialService _service;

    public AuditedFinancialController(IAuditedFinancialService service)
    {
        _service = service;
    }

    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateAuditedFinancialDto dto)
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
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                if (file.Length > 1024 * 1024) // Max 1MB
                    return BadRequest(new { message = "File size should not exceed 1MB." });

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                //dto.AttachedDocument = Convert.ToBase64String(ms.ToArray());
            }
            else
            {
                dto.AttachedDocument = null;
            }

            var id = await _service.CreateAsync(dto);
            return Ok(new { id, message = "Audited financial record created successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred: " + ex.Message });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await _service.GetByIdAsync(id);
        if (data == null) return NotFound();
        return Ok(data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAuditedFinancialDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        if (!updated) return NotFound();
        return Ok(new { Message = "Updated successfully" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return Ok(new { Message = "Deleted successfully" });
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] string search, [FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var paged = await _service.GetPagedAsync(search, status, page, pageSize);
        return Ok(paged);
    }

    [HttpGet("statuses")]
    public async Task<IActionResult> GetStatuses()
    {
        var statuses = await _service.GetStatusesAsync();
        return Ok(statuses);
    }
}

using Compliance_Dtos.Policies;
using Compliance_Services.Policies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PayGCompliance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _service;

        public PolicyController(IPolicyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddPolicy([FromBody] AddPolicyDto dto)
        {
            var newId = await _service.AddPolicyAsync(dto);
            if (newId == -5)
                return BadRequest(new { message = "Duplicate BR Numbers not allowed." });

            return Ok(new { NewId = newId });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] UpdatePolicyDto dto)
        {
            if (id != dto.Id) return BadRequest("Mismatched ID");
            var result = await _service.UpdatePolicyAsync(dto);
            return Ok(new { ResultCode = result });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            var result = await _service.GetPolicyByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? search = null)
        {
            var list = await _service.GetAllPoliciesAsync(page, size, search);
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string performedBy)
        {
            var result = await _service.DeletePolicyAsync(id, performedBy);
            return Ok(new { ResultCode = result });
        }
    }

}

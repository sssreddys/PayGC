using Compliance_Dtos.Policies;
using Compliance_Services.Policies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayGCompliance.Common;

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
            try
            {
                var newId = await _service.AddPolicyAsync(dto);

                if (newId == -5)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Duplicate BR Numbers not allowed."));
                }

                return Ok(ApiResponse<int>.SuccessResponse(newId, "Policy added successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] UpdatePolicyDto dto)
        {
            try
            {
                if (id != dto.Id)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Mismatched ID between route and body."));
                }

                var result = await _service.UpdatePolicyAsync(dto);

                if (result == -5)
                {
                    return BadRequest(ApiResponse<string>.ErrorResponse("Duplicate BR Numbers not allowed."));
                }

                if (result == 0)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found."));
                }

                return Ok(ApiResponse<int>.SuccessResponse(result, "Policy updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            try
            {
                var result = await _service.GetPolicyByIdAsync(id);

                if (result == null)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found."));
                }

                return Ok(ApiResponse<GetPolicyDto>.SuccessResponse(result, "Policy fetched successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }


        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? search = null)
        {
            try
            {
                var policies = await _service.GetAllPoliciesAsync(page, size, search);

                if (policies == null || !policies.Any())
                {
                    return Ok(ApiResponse<List<ListPolicyDto>>.SuccessResponse(new List<ListPolicyDto>(), "No policies found."));
                }

                return Ok(ApiResponse<List<ListPolicyDto>>.SuccessResponse((List<ListPolicyDto>)policies, "Policies fetched successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromQuery] string performedBy)
        {
            try
            {
                var result = await _service.DeletePolicyAsync(id, performedBy);

                if (result == 0)
                {
                    return NotFound(ApiResponse<string>.ErrorResponse("Policy not found or already deleted."));
                }

                return Ok(ApiResponse<int>.SuccessResponse(result, "Policy deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResponse("Server error: " + ex.Message));
            }
        }

    }

}

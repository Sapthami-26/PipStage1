using Microsoft.AspNetCore.Mvc;
using PipStage1.Data;
using PipStage1.Models;
using System.Threading.Tasks;

namespace PipStage1.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class PipStage1Controller : ControllerBase
    {
        private readonly IPipStage1Repository _repo;

        public PipStage1Controller(IPipStage1Repository repo)
        {
            _repo = repo;
        }

        // 1. GET: /api/PipStage1/{id}
        // Returns 200 OK with data or 404 Not Found.
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetailsByMasterId(int id)
        {
            // Call the repository to fetch the data
            var detail = await _repo.GetDetailsByMasterIdAsync(id);
            if (detail != null)
            {
                return Ok(detail); 
            }
            return NotFound($"PIP Stage 1 ID {id} was not found.");
        }

        // 2. PUT: /api/PipStage1/{id}
        // Returns 200 OK with a success message (Changed from 204 NoContent)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStage1Details(int id, [FromBody] PipStage1UpdateDto details)
        {
            await _repo.UpdateStage1DetailsAsync(id, details);
            
            return Ok(new 
            {
                Status = "Success",
                Message = $"PIP Stage 1 details for ID {id} updated successfully (Work Done)."
            }); 
        }

        // 3. POST: /api/PipStage1/{id}/submit/{mEmpId}
        // Returns 200 OK with a success message (Changed from 204 NoContent)
        [HttpPost("{id:int}/submit/{mEmpId:int}")]
        public async Task<IActionResult> EmployeeSubmit(int id, int mEmpId)
        {
            await _repo.UpdateEmployeeSubmitAsync(id, mEmpId);
            
            return Ok(new 
            {
                Status = "Success",
                Message = $"PIP Stage 1 ID {id} successfully submitted by Employee ID {mEmpId} (Work Done)."
            });
        }

        // 4. POST: /api/PipStage1/actionplan
        // Returns 200 OK with the full object and a success message.
        [HttpPost("actionplan")] 
        public async Task<IActionResult> InsertUpdateActionPlan([FromBody] ActionPlanItem actionPlan)
        {
            // Simple validation check
            if (actionPlan == null || actionPlan.PIPStage1ID == 0)
            {
                return BadRequest("Invalid Action Plan data.");
            }
            
            try
            {
                // This is the line that was failing due to missing parameter
                var rowsAffected = await _repo.InsertUpdateActionPlanAsync(actionPlan);

                if (rowsAffected > 0)
                {
                    // Assuming success, return a 200 OK or 201 Created
                    return Ok(new { Message = "Action Plan updated successfully.", Rows = rowsAffected });
                }
                return NotFound("Action Plan update failed. Check if PIPStage1ID exists.");
            }
            catch (Exception ex)
            {
                // Log the exception (recommended)
                return StatusCode(500, $"An error occurred during update: {ex.Message}");
            }
        }

        // 5. DELETE: /api/PipStage1/actionplan/{pipaid}/{pipStage1Id}
        
        [HttpDelete("actionplan/{pipaid:int}/{pipStage1Id:int}")]
        public async Task<IActionResult> DeleteActionPlan(int pipaid, int pipStage1Id)
        {
            await _repo.DeleteActionPlanAsync(pipaid, pipStage1Id);

            return Ok(new
            {
                Status = "Success",
                Message = $"Action Plan item PIPAID {pipaid} deleted successfully (Work Done)."
            });
        }
    }
}
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
            var detail = await _repo.GetDetailsByMasterIdAsync(id);
            if (detail == null)
            {
                return NotFound(); 
            }
            return Ok(detail); 
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
            await _repo.InsertUpdateActionPlanAsync(actionPlan);
            
            return Ok(new 
            {
                Status = "Success",
                Message = "Action Plan item saved/updated successfully (Work Done).",
                Data = actionPlan
            });
        }

        // 5. DELETE: /api/PipStage1/actionplan/{pipaid}/{pipStage1Id}
        // Assuming you may also want a success message for delete (Default is 204 NoContent)
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
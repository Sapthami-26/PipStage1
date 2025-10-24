using Microsoft.AspNetCore.Mvc;
using PipStage1.Data;
using PipStage1.Models;

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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDetailsByMasterId(int id)
        {
            var detail = await _repo.GetDetailsByMasterIdAsync(id);

            if (detail == null)
            {
                // The 404 response is generated HERE because the repository returned null
                return NotFound();
            }

            return Ok(detail);
        }

        // PUT: /api/PipStage1/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStage1Details(int id, [FromBody] PipStage1UpdateDto details)
        {
            await _repo.UpdateStage1DetailsAsync(id, details);
            return NoContent();
        }

        // POST: /api/PipStage1/{id}/submit/{mEmpId}
        [HttpPost("{id:int}/submit/{mEmpId:int}")]
        public async Task<IActionResult> EmployeeSubmit(int id, int mEmpId)
        {
            // Calls UpdateEmployeeSubmitAsync, which uses @PIPStage1ID (id) and @SubmittedByMEmpID (mEmpId)
            await _repo.UpdateEmployeeSubmitAsync(id, mEmpId);
            return NoContent();
        }

        // POST: /api/PipStage1/actionplan (Insert/Update)
        [HttpPost("actionplan")]
        public async Task<IActionResult> InsertUpdateActionPlan([FromBody] ActionPlanItem actionPlan)
        {
            await _repo.InsertUpdateActionPlanAsync(actionPlan);
            return CreatedAtAction(nameof(GetDetailsByMasterId), new { id = actionPlan.PIPStage1ID }, actionPlan);
        }

        // DELETE: /api/PipStage1/actionplan/{pipaid}/{pipStage1Id}
        [HttpDelete("actionplan/{pipaid:int}/{pipStage1Id:int}")]
        public async Task<IActionResult> DeleteActionPlan(int pipaid, int pipStage1Id)
        {
            await _repo.DeleteActionPlanAsync(pipaid, pipStage1Id);
            return NoContent();
        }
    }
}
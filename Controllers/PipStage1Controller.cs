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

        // GET: api/PipStage1/123
        [HttpGet("{masterId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PipStage1Detail))] // Updated Model Name
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PipStage1Detail>> GetPipStage1Details(int masterId) // Updated Model Name
        {
            if (masterId <= 0)
            {
                return BadRequest("Master ID must be a positive integer.");
            }

            try
            {
                var details = await _repo.GetPipStage1DetailsByMasterIDAsync(masterId);

                if (details == null)
                {
                    return NotFound($"PIP Stage 1 details not found for ID: {masterId}");
                }

                return Ok(details);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An internal server error occurred while retrieving data.");
            }
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
            return CreatedAtAction(nameof(GetPipStage1Details), new { masterId = actionPlan.PIPStage1ID }, actionPlan);
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
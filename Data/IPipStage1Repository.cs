using PipStage1.Models;

namespace PipStage1.Data
{
    public interface IPipStage1Repository
    {
        
        Task<PipStage1Detail> GetPipStage1DetailsByMasterIDAsync(int pipStage1Id);   
        Task UpdateStage1DetailsAsync(int pipStage1Id, PipStage1UpdateDto details);
        Task UpdateEmployeeSubmitAsync(int pipStage1Id, int submittedByMEmpId);
        Task InsertUpdateActionPlanAsync(ActionPlanItem actionPlan);
        Task DeleteActionPlanAsync(int pipaid, int pipStage1Id);
    }
}
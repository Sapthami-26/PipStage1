using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PipStage1.Models;
using System.Data;

namespace PipStage1.Data
{
    public class PipStage1Repository : IPipStage1Repository
    {
        private readonly string _connectionString;

        public PipStage1Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WFAppConnection")
                ?? throw new InvalidOperationException("WFAppConnection connection string not found.");
        }

        public async Task<PipStage1Detail?> GetDetailsByMasterIdAsync(int pipStage1Id)
        {
            const string spName = "PIP_Stage1_GetDetailsByMasterID";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@PIPStage1ID", pipStage1Id);

            using var multi = await connection.QueryMultipleAsync(spName, parameters, commandType: CommandType.StoredProcedure);

            // 1. Read the incomplete data into the DTO (SUCCESSFUL MAPPING)
            // We use PipStage1HeaderDto because it matches the incomplete SQL SELECT list.
            var headerDto = await multi.ReadSingleOrDefaultAsync<PipStage1HeaderDto>();

            if (headerDto == null)
            {
                return null; // Triggers the 404 in the controller
            }

            // 2. Manually construct the final detail object, setting the missing ID and copying properties
            var detail = new PipStage1Detail
            {
                PIPStage1ID = pipStage1Id, // 🔑 FIX: Inject the missing ID from the parameter

                // Transfer properties from the DTO to the final object (using inheritance is cleaner)
                MEmpID = headerDto.MEmpID,
                EmpName = headerDto.EmpName,
                GenID = headerDto.GenID,
                LevelName = headerDto.LevelName,
                TeamGroup = headerDto.TeamGroup,
                RMName = headerDto.RMName,
                HRBPName = headerDto.HRBPName,
                InitiatedOn = headerDto.InitiatedOn,
                PIPStartDate = headerDto.PIPStartDate,
                PIPEndDate = headerDto.PIPEndDate,
                PIPMidReviewDate = headerDto.PIPMidReviewDate,
                PerformanceHistory = headerDto.PerformanceHistory,
                ImprovementAreas = headerDto.ImprovementAreas,
                Comments = headerDto.Comments,
                IsAgreedByEmp = headerDto.IsAgreedByEmp,
                EmpAgreedOn = headerDto.EmpAgreedOn
            };

            // 3. Read the second result set (Action Plan)
            detail.ActionPlan = multi.Read<ActionPlanItem>().ToList();

            return detail; // This returns the full object (200 OK)
        }

        public async Task UpdateStage1DetailsAsync(int pipStage1Id, PipStage1UpdateDto details)
        {
            const string spName = "PIP_Stage1_Update";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@PIPStage1ID", pipStage1Id);
            parameters.Add("@PerformanceHistory", details.PerformanceHistory);
            parameters.Add("@ImprovementAreas", details.ImprovementAreas);
            parameters.Add("@Comments", details.Comments);

            parameters.Add("@HRBPRemarks", details.HRBPRemarks);
            parameters.Add("@PIPDuration", details.PIPDuration);

            parameters.Add("@PIPStartDate", details.PIPStartDate, dbType: DbType.DateTime2);
            parameters.Add("@PIPEndDate", details.PIPEndDate, dbType: DbType.DateTime2);
            parameters.Add("@PIPMidReviewDate", details.PIPMidReviewDate, dbType: DbType.DateTime2);

            parameters.Add("@IsSaveAsDraft", details.IsSaveAsDraft);

            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateEmployeeSubmitAsync(int pipStage1Id, int submittedByMEmpId)
        {
            const string spName = "PIP_Stage1_UpdateEmpSubmit";

            using var connection = new SqlConnection(_connectionString);
            // Parameters match the SQL procedure: @PIPStage1ID and @SubmittedByMEmpID
            await connection.ExecuteAsync(
                spName,
                new { @PIPStage1ID = pipStage1Id, @SubmittedByMEmpID = submittedByMEmpId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertUpdateActionPlanAsync(ActionPlanItem actionPlan)
        {
            const string spName = "PIP_Stage1_InsertUpdateActionPlan";

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@PIPAID", actionPlan.PIPAID);
            parameters.Add("@PIPStage1ID", actionPlan.PIPStage1ID);
            parameters.Add("@Task", actionPlan.Task);
            parameters.Add("@Weightage", actionPlan.Weightage);
            parameters.Add("@TargetDate", actionPlan.TargetDate, dbType: DbType.DateTime2);
            parameters.Add("@ReviewDate", actionPlan.ReviewDate, dbType: DbType.DateTime2);
            parameters.Add("@IsSaveAsDraft", actionPlan.IsSaveAsDraft);

            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteActionPlanAsync(int pipaid, int pipStage1Id)
        {
            const string spName = "PIP_Stage1_DeleteActionPlan";

            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                spName,
                new { @PIPAID = pipaid, @PIPStage1ID = pipStage1Id },
                commandType: CommandType.StoredProcedure);
        }
    }
}
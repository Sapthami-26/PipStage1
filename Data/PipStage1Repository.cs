using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PipStage1.Models;
using System.Data;
using System.Collections.Generic;

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

       public async Task<PipStage1Detail> GetPipStage1DetailsByMasterIDAsync(int pipStage1Id)
        {
            DataSet dsStage1 = new DataSet();
            string connectionString = _connectionString;
            const string spName = "[dbo].[PIP_Stage1_GetDetailsByMasterID]"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(spName, connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PIPStage1ID", pipStage1Id); 

                try
                {
                    await connection.OpenAsync(); 
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dsStage1);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception($"Database error executing {spName}.", ex); 
                }
            }

            // --- Map DataSet Result Sets to Model ---
            if (dsStage1.Tables.Count == 0 || dsStage1.Tables[0].Rows.Count == 0)
            {
                return null; 
            }

            // Map Table[0] (Main Details)
            DataRow row = dsStage1.Tables[0].Rows[0];
            var detail = new PipStage1Detail // Updated Model Name
            {
                Id = pipStage1Id,
                MEmpID = Convert.ToString(row["MEmpID"]),
                EmpName = Convert.ToString(row["EmpName"]),
                GenID = Convert.ToString(row["GenID"]),
                Band = Convert.ToString(row["LevelName"]),
                TeamGroup = Convert.ToString(row["TeamGroup"]),
                RMName = Convert.ToString(row["RMName"]),
                HRBPName = Convert.ToString(row["HRBPName"]),

                InitiatedOn = Convert.ToDateTime(row["InitiatedOn"]),
                
                PIPStartDate = row["PIPStartDate"] is DBNull ? null : (DateTime?)Convert.ToDateTime(row["PIPStartDate"]),
                PIPEndDate = row["PIPEndDate"] is DBNull ? null : (DateTime?)Convert.ToDateTime(row["PIPEndDate"]),
                PIPMidReviewDate = row["PIPMidReviewDate"] is DBNull ? null : (DateTime?)Convert.ToDateTime(row["PIPMidReviewDate"]),
                EmpAgreedOn = row["EmpAgreedOn"] is DBNull ? null : (DateTime?)Convert.ToDateTime(row["EmpAgreedOn"]),

                IsAgreedByEmp = Convert.ToBoolean(row["IsAgreedByEmp"]),
                PerformanceHistory = Convert.ToString(row["PerformanceHistory"]),
                ImprovementAreas = Convert.ToString(row["ImprovementAreas"]),
                Comments = Convert.ToString(row["Comments"])
            };

            // Ensure ActionPlan collection initialized if null
            if (detail.ActionPlan == null)
                detail.ActionPlan = new List<ActionPlanItem>();

            // Map Table[1] (Action Plan)
            if (dsStage1.Tables.Count > 1)
            {
                foreach (DataRow actionRow in dsStage1.Tables[1].Rows)
                {
                    detail.ActionPlan.Add(new ActionPlanItem
                    {
                        PIPAID = Convert.ToInt32(actionRow["PIPAID"]), 
                        Task = Convert.ToString(actionRow["Task"]),
                        Weightage = Convert.ToDecimal(actionRow["Weightage"]), 
                        
                        TargetDate = actionRow["TargetDate"] is DBNull ? null : (DateTime?)Convert.ToDateTime(actionRow["TargetDate"]),
                        ReviewDate = actionRow["ReviewDate"] is DBNull ? null : (DateTime?)Convert.ToDateTime(actionRow["ReviewDate"])
                    });
                }
            }

            return detail;
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

        public async Task UpdateStage1DetailsAsync(int pipStage1Id, PipStage1UpdateDto updateDto)
        {
            const string spName = "PIP_Stage1_UpdateDetails";

            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@PIPStage1ID", pipStage1Id);
            parameters.Add("@PerformanceHistory", updateDto.PerformanceHistory);
            parameters.Add("@ImprovementAreas", updateDto.ImprovementAreas);
            parameters.Add("@Comments", updateDto.Comments);
            
            await connection.ExecuteAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }
    }
}
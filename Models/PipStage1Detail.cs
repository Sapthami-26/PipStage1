namespace PipStage1.Models
{
   public class PipStage1Detail
    {
        // 🔑 CRITICAL: Must be returned by SQL Result Set 1
        public int PIPStage1ID { get; set; } 
        
        // Made nullable to be safe against DBNull in your tables
        public int? MEmpID { get; set; } 
        
        public string EmpName { get; set; } = string.Empty; 
        public string GenID { get; set; } = string.Empty; 
        public string LevelName { get; set; } = string.Empty; 
        public string TeamGroup { get; set; } = string.Empty; 
        public string RMName { get; set; } = string.Empty; 
        public string HRBPName { get; set; } = string.Empty; // Assuming this is returned by SQL
        
        // Made nullable, as non-nullable DateTimes frequently cause Dapper mapping failures
        public DateTime? InitiatedOn { get; set; } 
        
        public DateTime? PIPStartDate { get; set; } 
        public DateTime? PIPEndDate { get; set; } 
        public DateTime? PIPMidReviewDate { get; set; } 
        
        public string PerformanceHistory { get; set; } = string.Empty; 
        public string ImprovementAreas { get; set; } = string.Empty; 
        public string Comments { get; set; } = string.Empty; 
        public bool IsAgreedByEmp { get; set; } // Should map to isnull(IsAgreedByEmp, 0)
        public DateTime? EmpAgreedOn { get; set; }

        public List<ActionPlanItem> ActionPlan { get; set; } = new List<ActionPlanItem>();
    }
    
    public class PipStage1UpdateDto
    {
        public string PerformanceHistory { get; set; } = string.Empty;
        public string ImprovementAreas { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        
        // Fields matching PIP_Stage1_Update SP
        public string HRBPRemarks { get; set; } = string.Empty;
        public string PIPDuration { get; set; } = string.Empty;

        public DateTime? PIPStartDate { get; set; }
        public DateTime? PIPEndDate { get; set; }
        public DateTime? PIPMidReviewDate { get; set; }
        public bool IsSaveAsDraft { get; set; }
    }
}
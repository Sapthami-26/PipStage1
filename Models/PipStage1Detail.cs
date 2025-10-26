namespace PipStage1.Models
{
  public class PipStage1Detail // Changed class name to PipStage1Detail
    {
        // Employee/Identification Details
        public int Id { get; set; } 
        public string MEmpID { get; set; }
        public string EmpName { get; set; } 
        public string GenID { get; set; } 
        public string Band { get; set; } // LevelName
        public string TeamGroup { get; set; }
        public string RMName { get; set; } 
        public string HRBPName { get; set; } 

        // Date/Status Details
        public DateTime InitiatedOn { get; set; }
        public DateTime? PIPStartDate { get; set; } 
        public DateTime? PIPEndDate { get; set; }
        public DateTime? PIPMidReviewDate { get; set; }
        public bool IsAgreedByEmp { get; set; }
        public DateTime? EmpAgreedOn { get; set; } 

        // Performance Details
        public string PerformanceHistory { get; set; }
        public string ImprovementAreas { get; set; }
        public string Comments { get; set; } 

        // Navigation property for the second result set
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
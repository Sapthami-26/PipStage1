using System;

namespace PipStage1.Models
{
    public class PipStage1HeaderDto
    {
        public int? MEmpID { get; set; } 
        public string EmpName { get; set; } = string.Empty; 
        public string GenID { get; set; } = string.Empty; 
        public string LevelName { get; set; } = string.Empty; 
        public string TeamGroup { get; set; } = string.Empty; 
        public string RMName { get; set; } = string.Empty; 
        public string HRBPName { get; set; } = string.Empty; 
        public DateTime? InitiatedOn { get; set; } 
        public DateTime? PIPStartDate { get; set; } 
        public DateTime? PIPEndDate { get; set; } 
        public DateTime? PIPMidReviewDate { get; set; } 
        public string PerformanceHistory { get; set; } = string.Empty; 
        public string ImprovementAreas { get; set; } = string.Empty; 
        public string Comments { get; set; } = string.Empty; 
        public bool IsAgreedByEmp { get; set; } 
        public DateTime? EmpAgreedOn { get; set; }
    }
}
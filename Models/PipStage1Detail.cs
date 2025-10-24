namespace PipStage1.Models
{
   public class PipStage1Detail : PipStage1HeaderDto
    {
        // This is the property that was missing and will be set manually
        public int PIPStage1ID { get; set; } 
        
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
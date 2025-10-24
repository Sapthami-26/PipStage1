using System;

namespace PipStage1.Models
{
    public class ActionPlanItem
    {
        public int PIPAID { get; set; }
        public int PIPStage1ID { get; set; }
        public string Task { get; set; } = string.Empty;
        public int Weightage { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public bool IsSaveAsDraft { get; set; } 
    }
}
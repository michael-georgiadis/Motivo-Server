namespace Motivo.ApiModels.Goals.Outbound
{
    public class GeneralGoal
    {
        public int GoalId { get; set; }
        public string Title { get; set; }
        public int NumericCurrent { get; set; }
        public int NumericGoal { get; set; }
        public int AddBy { get; set; }
    }
}

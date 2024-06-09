namespace AADizErp.Models.Dtos.HrDtos
{
    public class ManpowerSummaryDto
    {
        public int TotalEmployee { get; set; }
        public int TotalNewJoining { get; set; }
       
        public int TotalCurrentLefty { get; set; }
        public int TotalPreviousLefty { get; set; }
        public int TotalLefty { get; set; }
        public int TotalDayPresent { get; set; }
        public int TotalDayAbsent { get; set; }
        public int ActiveOperator { get; set; }
        public int ActiveHelper { get; set; }
        public int PresentOperator { get; set; }
        public int PresentHelper { get; set; }
        public int OtherManpower { get; set; }
        public string MMR { get; set; }
    }
}

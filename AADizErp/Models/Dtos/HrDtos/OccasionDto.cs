namespace AADizErp.Models.Dtos.HrDtos
{
    public class OccasionDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalDays { get; set; }
        public int Year { get; set; }
        public string Tag { get; set; } // Event, Holiday etc.
        public string CreatedBy { get; set; }
    }
}

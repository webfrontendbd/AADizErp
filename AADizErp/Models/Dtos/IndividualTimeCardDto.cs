namespace AADizErp.Models.Dtos
{
    public class IndividualTimeCardDto
    {
        public string AttnDate { get; set; }
        public string Empno { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public int Late { get; set; }
        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string Status { get; set; }
        public decimal Othour { get; set; }
        public decimal Othour2 { get; set; }
        public decimal Extraot { get; set; }
        public decimal WorkingHours { get; set; }
    }
}

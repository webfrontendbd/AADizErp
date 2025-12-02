namespace AADizErp.Models.Dtos.HrDtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string ConfirmationStatus { get; set; }
        public string CompanyName { get; set; }
        public string EntryBy { get; set; }
        public string EmployeeType { get; set; }
        public string CreatedAt { get; set; }
        public string LastUpdateAt { get; set; }
    }
}

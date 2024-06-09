namespace AADizErp.Models.Dtos.HrDtos
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        public string EmployeeName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public bool ConfirmationStatus { get; set; }
    }
}

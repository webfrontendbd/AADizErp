namespace AADizErp.Models.Dtos
{
    public class PaySlipInfoDto
    {
        public DateTime PaymentDate { get; set; }
        public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public double OtHour { get; set; }
        public double OtAmount { get; set;}
        public double SalaryPayable { get; set; }
        public double AttendanceBonus { get; set; }

    }
}

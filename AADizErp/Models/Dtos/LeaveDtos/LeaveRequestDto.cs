namespace AADizErp.Models.Dtos.LeaveDtos
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string LeaveType { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string Reason { get; set; }
        public string RequestedBy { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public bool RequestMoved { get; set; } = false;
        public bool RequestSeen { get; set; }
        public int TotalDays
        {
            get
            {
                TimeSpan leaveDuration = this.LeaveEndDate - this.LeaveStartDate;
                return leaveDuration.Days + 1;
            }
        }
    }


    public class LeaveRequestForm
    {
        public string LeaveType { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public string Reason { get; set; }
    }
}

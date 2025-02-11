namespace AADizErp.Models.Dtos
{
    public class RemoteAttendanceDto
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string AttendanceArea { get; set; }
        public string Reason { get; set; }
        public string RequestedBy { get; set; }
        public string Status { get; set; }
        public string ApprovedBy { get; set; }
        public string CompanyName { get; set; }
        public bool AttendanceMoved { get; set; }
        public bool RequestSeen { get; set; }
        public string Created { get; set; }
    }
}

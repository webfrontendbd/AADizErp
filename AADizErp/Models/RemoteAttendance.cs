namespace AADizErp.Models
{
    public class RemoteAttendance
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
        public bool AttendanceMoved { get; set; }
        public string RequestedTime { get; set; }
    }
}

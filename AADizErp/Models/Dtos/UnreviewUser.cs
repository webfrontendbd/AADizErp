namespace AADizErp.Models.Dtos
{
    public class UnreviewUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string EmployeeNumber { get; set; }
        public string OrganizationName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsUserConfirmed { get; set; }
        public string Status { get; set; }
    }
}

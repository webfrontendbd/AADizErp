namespace AADizErp.Models
{
    public class UserInfo
    {
        public string Username { get; set; }
        public TokenUserMetaInfo TokenUserMetaInfo { get; set; }
        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }
        public string[] Buyers { get; set; }
        public string[] Factories { get; set; }
        public string[] Applications { get; set; }
    }
}

namespace AADizErp.Models.Dtos
{
    public class DeviceTokenDto
    {
        public DeviceTokenDto(string username, string deviceToken)
        {
            Username=username;
            DeviceToken=deviceToken;
        }

        public string Username { get; set; }
        public string DeviceToken { get; set; }
    }
}

namespace AADizErp.Models
{
    public class UserTokenDto
    {
        public string Token { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}

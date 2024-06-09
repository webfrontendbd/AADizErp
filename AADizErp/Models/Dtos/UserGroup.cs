namespace AADizErp.Models.Dtos
{
    public class UserGroup
    {
        public string Name { get; set; }
        public ICollection<UserConnection> UserConnections { get; set; }
    }

    public class UserConnection
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
}

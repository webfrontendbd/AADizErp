using AADizErp.Models;
using AADizErp.Services;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AADizErp
{
    public partial class App : Application
    {
        //public static string BaseAddress =  DeviceInfo.Platform == DevicePlatform.Android? "http://10.0.2.2:7005" : "http://localhost:7005";
       public static string BaseAddress = "http://gateway.asianapparels.com";
        public static BadgeManagerService BadgeManager { get; private set; }
        public App()
        {
            InitializeComponent();
            var notificationCounter = new NotificationCounter(new NotificationCounterImplementation());
            BadgeManager = new BadgeManagerService(notificationCounter);
            MainPage = new AppShell();
        }
        public static async Task<string> GetAuthToken()
        {
            return await SecureStorage.GetAsync("Token");
        }
        public static async Task<UserInfo> GetUserInfo()
        {
            var token = await SecureStorage.GetAsync("Token");
            if (token == null)
            {
                return null;
            }
            else
            {
                var jsonToken = new JwtSecurityTokenHandler().ReadToken(token) as
                    JwtSecurityToken;

                var tokenMeta = JsonConvert.DeserializeObject<TokenUserMetaInfo>(jsonToken.Claims.Where(c => c.Type == "tokenUser").FirstOrDefault().Value);

                return new UserInfo
                {
                    Username = jsonToken.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).ToString(),
                    TokenUserMetaInfo = tokenMeta,
                    Roles = jsonToken.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToArray(),
                    Permissions = jsonToken.Claims.Where(c => c.Type == "permissions").Select(c => c.Value).ToArray(),
                    Factories = jsonToken.Claims.Where(c => c.Type == "factories").Select(c => c.Value).ToArray(),
                    Buyers = jsonToken.Claims.Where(c => c.Type == "buyer").Select(c => c.Value).ToArray(),
                    Applications = jsonToken.Claims.Where(c => c.Type == "apps").Select(c => c.Value).ToArray()
                };
            }
        }
    }
}
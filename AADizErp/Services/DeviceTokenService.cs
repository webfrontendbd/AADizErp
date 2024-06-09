using AADizErp.Models.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AADizErp.Services
{
    public class DeviceTokenService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        public string StatusMessage;
        public DeviceTokenService()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async void AddDeviceToken(DeviceTokenDto tokenDto)
        {
            try
            {
                await SetAuthToken();
                HttpResponseMessage response = null;
                response = await _client.PutAsJsonAsync("/user/UserManagement/add-user-device-token", tokenDto);

                if (response.IsSuccessStatusCode)
                {
                    StatusMessage="Device Token added";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }

        public async Task<string> GetUserDeviceToken(string username)
        {
            string userDevice_token = String.Empty;
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync("/user/UserManagement/get-user-device-token?username=" + username);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return userDevice_token;
        }

        public async Task<string> GetManagerDeviceToken(string managerUsername)
        {
            string managerDevice_token = String.Empty;
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync("/user/UserManagement/get-manager-device-token?managerUsername=" + managerUsername);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return managerDevice_token;
        }
        public async Task SetAuthToken()
        {
            var token = await SecureStorage.GetAsync("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}

using AADizErp.Models;
using AADizErp.Models.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AADizErp.Services
{
    public class AuthenticationService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        public string StatusMessage;
        public AuthenticationService()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<UserTokenDto> Login(LoginDto loginDto)
        {
            UserTokenDto userTokenDto = new UserTokenDto();
            try
            {
                HttpResponseMessage response = null;
                response = await _client.PostAsJsonAsync("/auth/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    string returnContent = await response.Content.ReadAsStringAsync();
                    userTokenDto = System.Text.Json.JsonSerializer.Deserialize<UserTokenDto>(returnContent, _serializerOptions);
                    return userTokenDto;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return userTokenDto;
        }

        public async Task<RegisterDto> UnreviewedRegistration(RegisterDto registerDto)
        {
            RegisterDto responseUser = new RegisterDto();
            try
            {
                HttpResponseMessage response = null;
                response = await _client.PostAsJsonAsync("/auth/unreviewed-register", registerDto);

                if (response.IsSuccessStatusCode)
                {
                    string returnContent = await response.Content.ReadAsStringAsync();
                    responseUser = System.Text.Json.JsonSerializer.Deserialize<RegisterDto>(returnContent, _serializerOptions);
                    return responseUser;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return responseUser;
        }

        public async Task<UnreviewUser> UnreviewedRegistrationApproval(UnreviewUser registerDto)
        {
            try
            {
                await SetAuthToken();
                HttpResponseMessage response = null;
                response = await _client.PutAsJsonAsync("/user/UnreviewedUser/unreviewed-user-approval", registerDto);

                if (response.IsSuccessStatusCode)
                {
                    string returnContent = await response.Content.ReadAsStringAsync();
                    var responseUser = System.Text.Json.JsonSerializer.Deserialize<UnreviewUser>(returnContent, _serializerOptions);
                    return responseUser;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return default;
        }

        public void Logout()
        {
            Preferences.Default.Set("IsAuthenticated", false);
            //SecureStorage.Remove("Token");
        }

        public async Task<PasswordChangedResponse> ChangedUserPassword(LoginDto loginDto)
        {
            try
            {
                await SetAuthToken();
                HttpResponseMessage response = null;
                response = await _client.PostAsJsonAsync("/user/UserManagement/update-password", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    string returnContent = await response.Content.ReadAsStringAsync();
                    var responseUser = System.Text.Json.JsonSerializer.Deserialize<PasswordChangedResponse>(returnContent, _serializerOptions);
                    return responseUser;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return default;
        }

        public async Task<bool> DeleteUserAccount(string username)
        {
            try
            {
                await SetAuthToken();
                HttpResponseMessage response = null;
                response = await _client.PutAsJsonAsync("/user/UserManagement/delete-user-account", username);

                if (response.IsSuccessStatusCode)
                {
                    string returnContent = await response.Content.ReadAsStringAsync();
                    var responseUser = System.Text.Json.JsonSerializer.Deserialize<bool>(returnContent, _serializerOptions);
                    return responseUser;
                }
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return default;
        }

        public async Task SetAuthToken()
        {
            var token = await SecureStorage.GetAsync("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}

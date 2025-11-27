using AADizErp.Models;
using AADizErp.Models.Dtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AADizErp.Services.MisServices
{
    public class Mis_eAttendanceServices
    {

        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public Mis_eAttendanceServices()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public async Task<List<RemoteAttendanceDto>> GeteAttendanceListForSuperAdmin(AppQueryModel qm)
        {
           await SetAuthToken();
            try
            {
                var response = await _client.GetAsync($"/hr/attendance/get-eattendance-list-for-superadmin?CompanyName={qm.CompanyName}&CardId={qm.CardId}&DateFrom={qm.DateFrom}&DateTo={qm.DateTo}&Status={qm.Status}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RemoteAttendanceDto>>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

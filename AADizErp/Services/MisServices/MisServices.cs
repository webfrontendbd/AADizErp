using AADizErp.Models.Dtos.MisDtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AADizErp.Services.MisServices
{
    public class MisServices
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public MisServices()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public async Task<List<OvertimeDto>> GetGroupOvertimeSummary(string datefrom, string dateto, string companyName)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/mis/get-group-overtime-summary?DateFrom={datefrom}&DateTo={dateto}&CompanyName={companyName}");
                return System.Text.Json.JsonSerializer.Deserialize<List<OvertimeDto>>(response, _serializerOptions);
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

using AADizErp.Models.Dtos.McDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AADizErp.Services.McServices
{
    public class MachineService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public MachineService()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }


        public async Task<MachineInfoDto> GetMachinePresentStatusByMcid(string mcid)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-present-status-byid?machineId={mcid}");
                return System.Text.Json.JsonSerializer.Deserialize<MachineInfoDto>(response, _serializerOptions);
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

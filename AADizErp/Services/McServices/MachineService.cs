using AADizErp.Models;
using AADizErp.Models.Dtos.McDtos;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<List<MachineFloorDto>> GetMachineFloorByOrgid()
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-floor-byorg?orgname={userInfo.TokenUserMetaInfo.OrganizationName}&type=floor");
                return System.Text.Json.JsonSerializer.Deserialize<List<MachineFloorDto>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<MachineLineDto>> GetMachineLineByOrgid()
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-line-byorg?orgname={userInfo.TokenUserMetaInfo.OrganizationName}&type=line");
                return System.Text.Json.JsonSerializer.Deserialize<List<MachineLineDto>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
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

        public async Task<List<GroupMachineStatusDto>> GetGroupMachineStatusByDate(string reportDate)
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-group-mc-status-all?reportDate={reportDate}");
                return System.Text.Json.JsonSerializer.Deserialize<List<GroupMachineStatusDto>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<MovementHistoryModel>> GetMachineMovementHistoryByMcid(string mcid)
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-movement-history-bymcid?mcid={mcid}");
                return System.Text.Json.JsonSerializer.Deserialize<List<MovementHistoryModel>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<ServiceHistoryModel>> GetMachineServicingHistoryByMcid(string mcid)
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-servicing-history-bymcid?mcid={mcid}");
                return System.Text.Json.JsonSerializer.Deserialize<List<ServiceHistoryModel>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<SparePartsModel>> GetMachineSparePartsHistoryByMcid(string mcid)
        {
            UserInfo userInfo = await App.GetUserInfo();
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/machine/maintenance/get-mc-spareparts-history-bymcid?mcid={mcid}");
                return System.Text.Json.JsonSerializer.Deserialize<List<SparePartsModel>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<int> UpdateMachineStatusByMcid(MachineStatusUpdateDto machineInfoDto)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.PutAsJsonAsync($"/machine/maintenance/update-mc-status-byorg", machineInfoDto);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(jsonResponse);
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

using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.HrDtos;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AADizErp.Services.HrServices
{
    public class OccasionService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        public string StatusMessage;
        public OccasionService()
        {
            _client = new() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<PaginatedResult<OccasionDto>> GetCompanyOccasionDays(int pageNumber, int pageSize, string tag)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/occasion/get-list-of-events-holidays?pageIndex={pageNumber}&pageSize={pageSize}&search={tag}");
                return JsonSerializer.Deserialize<PaginatedResult<OccasionDto>>(response, _serializerOptions);
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

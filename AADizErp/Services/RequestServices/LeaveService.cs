using AADizErp.Models;
using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.LeaveDtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AADizErp.Services.RequestServices
{
    public class LeaveService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        public LeaveService()
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<PaginatedResult<LeaveRequestDto>> GetListOfIndividualLeaveRequest(int pageNumber, int pageSize, string username)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/leave/get-list-of-leave-request?pageIndex={pageNumber}&pageSize={pageSize}&search={username}");

                return JsonConvert.DeserializeObject<PaginatedResult<LeaveRequestDto>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<PaginatedResult<LeaveRequestDto>> GetListLeaveRequestForManager(int pageNumber, int pageSize, string username)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/leave/get-list-of-leave-request-for-manager?pageIndex={pageNumber}&pageSize={pageSize}&search={username}");

                return JsonConvert.DeserializeObject<PaginatedResult<LeaveRequestDto>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<IndividualLeaveSummary> GetIndividualLeaveSummary(UserInfo userInfo)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(userInfo.TokenUserMetaInfo.OrganizationName);
                await SetAuthToken();

                int currentYear = DateTime.Now.Year;
                DateTime firstDateOfYear = new DateTime(currentYear, 1, 1);
                DateTime lastDateOfYear = new DateTime(currentYear, 12, 31);

                var response = await _client.GetAsync($"/hr/leave/get-individual-leave-summary?CardId={userInfo.TokenUserMetaInfo.EmployeeNumber}&CompanyName={encodedCompany}&DateFrom={firstDateOfYear.ToString("dd-MMM-yyyy")}&DateTo={lastDateOfYear.ToString("dd-MMM-yyyy")}");

                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IndividualLeaveSummary>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<LeaveRequestDto> SubmitLeaveRequest(LeaveRequestDto remoteAttendance)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.PostAsJsonAsync("/hr/leave/add-leave-request", remoteAttendance);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LeaveRequestDto>(content);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public async Task<LeaveRequestDto> LeaveApprovalStatusChangedByManager(LeaveRequestDto remoteAttendance)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.PutAsJsonAsync($"/hr/leave/update-leave-request", remoteAttendance);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LeaveRequestDto>(content);

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

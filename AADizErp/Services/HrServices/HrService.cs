using AADizErp.Models;
using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.HrDtos;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AADizErp.Services.HrServices
{
    public class HrService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;
        public string StatusMessage;
        public HrService()
        {
            _client = new() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }
        public async Task<PaginatedResult<EmployeeDto>> GetUcEmployeeDataAsync(string companyName, int pageNumber, int pageSize)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/employee/get-uc-employees?CompanyName={companyName}&pageIndex={pageNumber}&pageSize={pageSize}");
                return JsonSerializer.Deserialize<PaginatedResult<EmployeeDto>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }
        public async Task<EmployeeDto> SaveUnconfirmedEmployee(Employee employee)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.PostAsJsonAsync("/hr/employee/add-unconfirm-employee", employee);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<EmployeeDto>(content, _serializerOptions);
                }

            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
            return default;
        }

        public async Task<ManpowerSummaryDto> GetManpowerStatusDataAsync(AppQueryModel queryModel)
        {
            try
            {
                string encodedCompany = WebUtility.UrlEncode(queryModel.CompanyName);
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/Attendance/GetDashboardSummary?companyName={encodedCompany}&dateFrom={queryModel.DateFrom}&dateTo={queryModel.DateTo}");
                ManpowerSummaryDto dResponse = JsonSerializer.Deserialize<ManpowerSummaryDto>(response, _serializerOptions);
                return dResponse;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }

        public async Task<IReadOnlyList<EmployeeComboList>> GetEmployeeByTermsAsync(string company, string terms)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                var response = await _client.GetStringAsync($"/noauth/Openforall/get-employees-by-terms?company={encodedCompany}&terms={terms}");
                IReadOnlyList<EmployeeComboList> dResponse = JsonSerializer.Deserialize<IReadOnlyList<EmployeeComboList>>(response, _serializerOptions);
                return dResponse;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }
        public async Task<IReadOnlyList<EmployeeComboList>> GetEmployeeByCompanyAsync(string company)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                var response = await _client.GetStringAsync($"/noauth/Openforall/get-employees-by-company?company={encodedCompany}");
                IReadOnlyList<EmployeeComboList> dResponse = JsonSerializer.Deserialize<IReadOnlyList<EmployeeComboList>>(response, _serializerOptions);
                return dResponse;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }

        public async Task<EmployeeInfoDto> GetEmployeeDetailsInfoDataAsync(string company, string cardid)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/Attendance/get-profile-info?CardId={cardid}&CompanyName={encodedCompany}");
                EmployeeInfoDto dResponse = JsonSerializer.Deserialize<EmployeeInfoDto>(response, _serializerOptions);
                return dResponse;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }

        public async Task<IReadOnlyList<EmployeeInfoDto>> GetDailyAbsentDetailsAsync(string company, string date)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/Attendance/get-daily-absent?CompanyName={encodedCompany}&DateFrom={date}");
                IReadOnlyList<EmployeeInfoDto> dResponse = JsonSerializer.Deserialize<IReadOnlyList<EmployeeInfoDto>>(response, _serializerOptions);
                return dResponse;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }

            return default;
        }

        public async Task<IReadOnlyList<UnreviewUser>> GetUnreviewedUserListAsync(string company)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/user/UnreviewedUser/get-unreviewed-user-by-company?company={encodedCompany}");
                IReadOnlyList<UnreviewUser> dResponse = JsonSerializer.Deserialize<IReadOnlyList<UnreviewUser>>(response, _serializerOptions);
                return dResponse;
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

using AADizErp.Models;
using AADizErp.Models.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AADizErp.Services.RequestServices
{
    public class AttendanceService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        private readonly NotificationService _notify;
        private readonly IGeolocation _geolocation;
        public AttendanceService(NotificationService notify, IGeolocation geolocation)
        {
            _client = new HttpClient() { BaseAddress = new Uri(App.BaseAddress) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            _notify = notify;
            _geolocation = geolocation;
        }

        public async Task<Location> GetCurrentLocation()
        {
            var location = await _geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }
            return location;
        }

        public async Task<RemoteAttendanceDto> CheckedIndvidualAttTimeByDateType(string date, string type, string username)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/attendance/checked-ind-atttime-bydate-type?date={date}&username={username}&type={type}");
                return System.Text.Json.JsonSerializer.Deserialize<RemoteAttendanceDto>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        public async Task<RemoteAttendanceDto> SubmitAttendanceRequest(RemoteAttendance remoteAttendance)
        {

            try
            {
                await SetAuthToken();
                var response = await _client.PostAsJsonAsync("/hr/attendance/add-attendance-request", remoteAttendance);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RemoteAttendanceDto>(content);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        public async Task<PaginatedResult<RemoteAttendanceDto>> GetAttendanceDetailsByEmpid(string empId,int index, int size)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/attendance/get-paginated-attendance-list?search={empId}&pageIndex={index}&pageSize={size}");
                return System.Text.Json.JsonSerializer.Deserialize<PaginatedResult<RemoteAttendanceDto>>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<RemoteAttendanceDto>> GetRemoteAttendanceListByManagerUsername(string username)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetAsync("/hr/attendance/remote-attendance-list-for-manager?username="+username);
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

        public async Task<IndividualAttendanceSummaryDto> GetIndividualAttendanceSummary(UserInfo userInfo)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(userInfo.TokenUserMetaInfo.OrganizationName);
                await SetAuthToken();

                DateTime currentDate = DateTime.Now;

                DateTime firstDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime lastDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));


                var response = await _client.GetAsync($"/hr/attendance/get-individual-attendance-summary?CardId={userInfo.TokenUserMetaInfo.EmployeeNumber}&CompanyName={encodedCompany}&DateFrom={firstDateOfMonth.ToString("dd-MMM-yyyy")}&DateTo={lastDateOfMonth.ToString("dd-MMM-yyyy")}");

                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IndividualAttendanceSummaryDto>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<List<PaySlipInfoDto>> GetIndividualPaySlipInfo(UserInfo userInfo)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(userInfo.TokenUserMetaInfo.OrganizationName);
                await SetAuthToken();                

                var response = await _client.GetAsync($"/hr/attendance/get-individual-payslip?CardId={userInfo.TokenUserMetaInfo.EmployeeNumber}&CompanyName={encodedCompany}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PaySlipInfoDto>>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<IReadOnlyList<IndividualTimeCardDto>> GetIndividualTimeCardDetails(UserInfo userInfo, DateTime startDate, DateTime endDate)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(userInfo.TokenUserMetaInfo.OrganizationName);
                await SetAuthToken();               

                var response = await _client.GetAsync($"/hr/attendance/get-individual-timecard?CardId={userInfo.TokenUserMetaInfo.EmployeeNumber}&CompanyName={encodedCompany}&DateFrom={startDate.ToString("dd-MMM-yyyy")}&DateTo={endDate.ToString("dd-MMM-yyyy")}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IReadOnlyList<IndividualTimeCardDto>>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<IReadOnlyList<IndividualTimeCardDto>> GetEmployeeTimeCardDetails(string cardnumber, string company, DateTime startDate, DateTime endDate)
        {
            try
            {
                var encodedCompany = WebUtility.UrlEncode(company);
                await SetAuthToken();

                var response = await _client.GetAsync($"/hr/attendance/get-individual-timecard?CardId={cardnumber}&CompanyName={encodedCompany}&DateFrom={startDate.ToString("dd-MMM-yyyy")}&DateTo={endDate.ToString("dd-MMM-yyyy")}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IReadOnlyList<IndividualTimeCardDto>>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<RemoteAttendanceDto> AttendanceRequestApproval(RemoteAttendanceDto remoteAttendanceDto)
        {
            try
            {
                await SetAuthToken();
                var response = await _client.PutAsJsonAsync($"/hr/attendance/update-attendance-request", remoteAttendanceDto);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RemoteAttendanceDto>(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return default;
        }

        public async Task<int> GetAttendanceRequestSeenDataAsync()
        {
            try
            {
                await SetAuthToken();
                var response = await _client.GetStringAsync($"/hr/Attendance/get-attendance-request-seen");
                int dResponse = System.Text.Json.JsonSerializer.Deserialize<int>(response, _serializerOptions);
                return dResponse;
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

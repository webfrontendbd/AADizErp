using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class UnconfirmEmployeeListPageViewModel : BaseViewModel
    {
        private readonly HrService _employeeService;

        private int pageNumber = 1;
        private const int PageSize = 10;

        // Backup list for search filtering
        private List<EmployeeDto> _originalList = new();

        // MAIN LIST BOUND TO UI
        [ObservableProperty]
        ObservableRangeCollection<EmployeeDto> unconfirmList = new();

        // SEARCH TEXT
        [ObservableProperty]
        private string searchText;

        partial void OnSearchTextChanged(string value)
        {
            ApplySearchFilter();
        }

        public UnconfirmEmployeeListPageViewModel(HrService employeeService)
        {
            _employeeService = employeeService;
            LoadInitialData();
        }

        // ─────────────────────────────────────────────
        // INITIAL LOAD
        // ─────────────────────────────────────────────
        private async void LoadInitialData()
        {
            await LoadUnconfirmEmployees(reset: true);
        }

        // ─────────────────────────────────────────────
        // LOAD EMPLOYEES (Main Logic)
        // ─────────────────────────────────────────────
        private async Task LoadUnconfirmEmployees(bool reset)
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                if (reset)
                {
                    pageNumber = 1;
                    UnconfirmList.Clear();
                    _originalList.Clear();
                }

                var userInfo = await App.GetUserInfo();

                var result =
                    await _employeeService.GetUcEmployeeDataAsync(
                        userInfo.TokenUserMetaInfo.OrganizationName,
                        pageNumber,
                        PageSize
                    );

                if (result?.Data?.Any() == true)
                {
                    if (reset)
                        _originalList = result.Data.ToList();
                    else
                        _originalList.AddRange(result.Data);

                    UnconfirmList.AddRange(result.Data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Load Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ─────────────────────────────────────────────
        // SEARCH FILTER
        // ─────────────────────────────────────────────
        private void ApplySearchFilter()
        {
            if (_originalList == null || _originalList.Count == 0)
                return;

            // If search text empty → restore full list
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                UnconfirmList.ReplaceRange(_originalList);
                return;
            }

            var text = SearchText.Trim();

            var filtered = _originalList.Where(x =>
                (x.EmployeeName ?? "").Contains(text, StringComparison.OrdinalIgnoreCase) ||
                (x.Phone ?? "").Contains(text) ||
                (x.ReferenceNumber ?? "").Contains(text, StringComparison.OrdinalIgnoreCase)
            );

            UnconfirmList.ReplaceRange(filtered);
        }

        // ─────────────────────────────────────────────
        // PULL TO REFRESH
        // ─────────────────────────────────────────────
        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadUnconfirmEmployees(reset: true);
        }

        // ─────────────────────────────────────────────
        // LOAD MORE PAGINATION
        // ─────────────────────────────────────────────
        [RelayCommand]
        private async Task LoadMoreAsync()
        {
            if (IsLoading) return;

            pageNumber++;
            await LoadUnconfirmEmployees(reset: false);
        }
    }
}

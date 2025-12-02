using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class UnconfirmEmployeeListPageViewModel : BaseViewModel
    {
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalCount = 0;

        private readonly HrService _employeeService;

        [ObservableProperty]
        ObservableRangeCollection<EmployeeDto> unconfirmList = new();

        public UnconfirmEmployeeListPageViewModel(HrService employeeService)
        {
            _employeeService = employeeService;
            GetUnconfirmEmployeeList(pageNumber, pageSize);
        }

        private async void GetUnconfirmEmployeeList(int index, int size)
        {
            if (totalCount != 0) totalCount = 0;

            IsLoading = true;
            UnconfirmList.Clear();

            var userInfo = await App.GetUserInfo();

            var result =
                await _employeeService.GetUcEmployeeDataAsync(userInfo.TokenUserMetaInfo.OrganizationName, index, size);

            if (result?.Count > 0)
            {
                totalCount = result.Count;
                UnconfirmList.ReplaceRange(result.Data);
            }

            IsLoading = false;
        }


        [RelayCommand]
        async Task PullToRefreshUnconfirmList()
        {
            IsLoading = true;
            pageNumber = 1;
            pageSize = 10;
            if (totalCount != 0) totalCount = 0;
            UnconfirmList.Clear();
            var userInfo = await App.GetUserInfo();
            var result = await _employeeService.GetUcEmployeeDataAsync(userInfo.TokenUserMetaInfo.OrganizationName, pageNumber, pageSize);
            if (result.Count > 0)
            {
                App.Current.Dispatcher.Dispatch(() =>
                {
                    totalCount = result.Count;
                    UnconfirmList.ReplaceRange(result.Data);
                });
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        async Task LoadMoreUnconfirmData()
        {
            var userInfo = await App.GetUserInfo();
            if (totalCount == UnconfirmList.Count())
            {
                IsLoading = false;
                return;
            }
            IsLoading = true;
            pageNumber++;
            var result = await _employeeService.GetUcEmployeeDataAsync(userInfo.TokenUserMetaInfo.OrganizationName, pageNumber, pageSize);
            if (result.Count > 0)
            {
                UnconfirmList.AddRange(result.Data);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }
    }
}

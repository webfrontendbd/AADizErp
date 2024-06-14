using AADizErp.Models.Dtos;
using AADizErp.Pages.ManagerPages;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.ManagerPagesVM
{
    public partial class ManagerAttendanceListViewModel : BaseViewModel
    {
        private readonly AttendanceService _attnService;
        private List<RemoteAttendanceDto> _remoteAttendances;
        private int _pageSize = 10;
        [ObservableProperty]
        private ObservableRangeCollection<RemoteAttendanceDto> attendances = new();


        public ManagerAttendanceListViewModel(AttendanceService attnService)
        {
            _attnService=attnService;
            GetRemoteAttendanceListForStatusUpdate();
        }


        private void GetRemoteAttendanceListForStatusUpdate()
        {
            IsLoading = true;
            Attendances.Clear();
            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();
                _remoteAttendances = await _attnService.GetRemoteAttendanceListByManagerUsername(userInfo.TokenUserMetaInfo.UserName);
                if (_remoteAttendances != null)
                {
                    App.Current.Dispatcher.Dispatch(() =>
                    {
                        Attendances.ReplaceRange(_remoteAttendances.Take(_pageSize).ToList());
                        IsLoading = false;
                    }); 
                }
                else
                {
                    IsLoading = false;
                }
            });

        }

        [RelayCommand]
        void LoadMoreRemoteAttendanceData()
        {
            if (_remoteAttendances?.Count > 0)
            {
                Attendances.AddRange(_remoteAttendances.Skip(Attendances.Count).Take(_pageSize).ToList());
            }
        }

        [RelayCommand]
        async Task ViewAttendanceDetails(RemoteAttendanceDto remoteAttendanceDto)
        {
            if (remoteAttendanceDto == null) return;
            await Shell.Current.GoToAsync($"{nameof(MgrAttendanceDetailsPage)}", true, new Dictionary<string, object>
            {
                {nameof(RemoteAttendanceDto), remoteAttendanceDto}
            });
        }
    }
}

using AADizErp.Models;
using AADizErp.Models.Dtos;
using AADizErp.Pages.ManagerPages;
using AADizErp.Services.MisServices;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.MisPageVM
{
    public partial class Mis_eAttendancePageViewModel : BaseViewModel
    {
        private readonly Mis_eAttendanceServices _attnService;
        private List<RemoteAttendanceDto> _remoteAttendances;
        private int _pageSize = 10;
        [ObservableProperty]
        private ObservableRangeCollection<RemoteAttendanceDto> attendances = new();


        public Mis_eAttendancePageViewModel(Mis_eAttendanceServices attnService)
        {
            _attnService = attnService;
            GetRemoteAttendanceListForStatusUpdate();
        }


        private void GetRemoteAttendanceListForStatusUpdate()
        {
            IsLoading = true;
            Attendances.Clear();
            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();
                var queryModel = new AppQueryModel
                {
                    CompanyName = userInfo.TokenUserMetaInfo.OrganizationName,
                    DateFrom = DateTime.Now.Date.ToString("dd-MMM-yyyy"),
                    DateTo = DateTime.Now.Date.ToString("dd-MMM-yyyy"),
                };
                _remoteAttendances = await _attnService.GeteAttendanceListForSuperAdmin(queryModel);
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

        //[RelayCommand]
        //async Task ViewAttendanceDetails(RemoteAttendanceDto remoteAttendanceDto)
        //{
        //    if (remoteAttendanceDto == null) return;
        //    if (!remoteAttendanceDto.RequestSeen)
        //    {
        //        remoteAttendanceDto.RequestSeen = true;
        //        await _attnService.AttendanceRequestApproval(remoteAttendanceDto);
        //    }
        //    await Shell.Current.GoToAsync($"{nameof(MgrAttendanceDetailsPage)}", true, new Dictionary<string, object>
        //    {
        //        {nameof(RemoteAttendanceDto), remoteAttendanceDto}
        //    });
        //}
    }
}

using MvvmHelpers;
using AADizErp.Models;
using CommunityToolkit.Mvvm.Input;
using AADizErp.Models.Dtos.HrDtos;
using CommunityToolkit.Mvvm.ComponentModel;
using AADizErp.Services.MisServices;
using AADizErp.Models.Dtos.MisDtos;

namespace AADizErp.ViewModels.MisPageVM
{
    public partial class OverTimePageViewModel : BaseViewModel
    {
        private readonly MisServices _misService;
        [ObservableProperty]
        ObservableRangeCollection<OvertimeDto> overtimes = new();
        [ObservableProperty]
        DateTime currentDate = DateTime.Today.AddDays(-1);

        public OverTimePageViewModel(MisServices misService)
        {
            _misService = misService;
            OvertimeSummaryOnPageLoad();
        }

        public async void OvertimeSummaryOnPageLoad()
        {
            var userInfo = await App.GetUserInfo();
            var queryModel = new AppQueryModel
            {
                CompanyName = userInfo.TokenUserMetaInfo.OrganizationName,
                DateFrom = CurrentDate.ToString("dd-MMM-yyyy"),
                DateTo = CurrentDate.ToString("dd-MMM-yyyy"),
            };
            var returnOvertimes = await _misService.GetGroupOvertimeSummary(queryModel.DateFrom, queryModel.DateTo, queryModel.CompanyName);
            if (returnOvertimes != null)
            {
                Overtimes.AddRange(returnOvertimes);
            }


        }
        [RelayCommand]
        async Task GetDailyOvertimeSummary()
        {
            var userInfo = await App.GetUserInfo();
            var queryModel = new AppQueryModel
            {
                CompanyName = userInfo.TokenUserMetaInfo.OrganizationName,
                DateFrom = CurrentDate.ToString("dd-MMM-yyyy"),
                DateTo = CurrentDate.ToString("dd-MMM-yyyy"),
            };
            try
            {
                var returnObject = await _misService.GetGroupOvertimeSummary(queryModel.DateFrom, queryModel.DateTo, queryModel.CompanyName);
                if (returnObject != null)
                {
                    Overtimes.ReplaceRange(returnObject);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }

    }
}

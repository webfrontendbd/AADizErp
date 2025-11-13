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
        ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        int selectedFactoryIndex;
        [ObservableProperty]
        DateTime currentDate = DateTime.Now;

        public OverTimePageViewModel(MisServices misService)
        {
            _misService = misService;
            Organizations.Add(new Organization { OrganizationName = "Select an unit", Abbr = "NA" });
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

            if (userInfo.Factories.Length > 0)
            {
                for (int i = 0; i < userInfo.Factories.Length; i++)
                {
                    var org = new Organization
                    {
                        OrganizationName = userInfo.Factories[i]
                    };
                    Organizations.Add(org);
                }
            }

        }
        [RelayCommand]
        async Task GetDailyOvertimeSummary()
        {
            try
            {
                var company = Organizations.ElementAt(SelectedFactoryIndex).OrganizationName;
                string qDate = CurrentDate.ToString("dd-MMM-yyyy");
                if (company != "Select an unit")
                {
                    var returnObject = await _misService.GetGroupOvertimeSummary(qDate, qDate, company);
                    if (returnObject != null)
                    {
                        Overtimes.ReplaceRange(returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }

    }
}

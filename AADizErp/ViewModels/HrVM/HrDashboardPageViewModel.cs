using AADizErp.Models;
using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class HrDashboardPageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        [ObservableProperty]
        private ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        private Organization organization = new();
        [ObservableProperty]
        private ManpowerSummaryDto dashboardDto = new();
        [ObservableProperty]
        private string selectedFactory;
        [ObservableProperty]
        private DateTime currentDate = DateTime.Now;

        public HrDashboardPageViewModel(HrService hrService)
        {
            Title="Dashboard";
            _hrService = hrService;
            OrganizationList();
        }
        private void OrganizationList()
        {            

            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();
                var queryModel = new AppQueryModel
                {
                    CompanyName = userInfo.TokenUserMetaInfo.OrganizationName,
                    DateFrom = DateTime.Now.ToString("dd-MMM-yyyy"),
                    DateTo = DateTime.Now.ToString("dd-MMM-yyyy")
                };
                DashboardDto = await _hrService.GetManpowerStatusDataAsync(queryModel);

                if(userInfo.Factories.Length > 0)
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
                
            });

        }

        [RelayCommand]
        async Task ReloadDashboardWhenCompanyChanged()
        {

            var queryModel = new AppQueryModel
            {
                CompanyName = SelectedFactory,
                DateFrom = CurrentDate.ToString("dd-MMM-yyyy"),
                DateTo = CurrentDate.ToString("dd-MMM-yyyy")
            };

            try
            {
                DashboardDto = await _hrService.GetManpowerStatusDataAsync(queryModel);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

    }
}

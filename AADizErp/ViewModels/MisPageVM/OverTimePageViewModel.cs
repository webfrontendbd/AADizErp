using MvvmHelpers;
using AADizErp.Models;
using CommunityToolkit.Mvvm.Input;
using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AADizErp.ViewModels.MisPageVM
{
    public partial class OverTimePageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        [ObservableProperty]
        ObservableRangeCollection<EmployeeInfoDto> employees = new();
        [ObservableProperty]
        ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        int selectedFactoryIndex;
        [ObservableProperty]
        DateTime currentDate = DateTime.Now;

        public OverTimePageViewModel(HrService hrService)
        {
            _hrService = hrService;
            Organizations.Add(new Organization { OrganizationName = "Select an unit", Abbr = "NA" });
            OrganizationList();
        }

        public async void OrganizationList()
        {
            var userInfo = await App.GetUserInfo();
            var queryModel = new AppQueryModel
            {
                CompanyName = userInfo.TokenUserMetaInfo.OrganizationName,
                DateFrom = CurrentDate.ToString("dd-MMM-yyyy"),
                DateTo = CurrentDate.ToString("dd-MMM-yyyy"),
            };
            var returnEmployees = await _hrService.GetDailyAbsentDetailsAsync(queryModel.CompanyName, queryModel.DateFrom);
            if (returnEmployees != null)
            {
                Employees.AddRange(returnEmployees);
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
        async Task GetDailyAbsentDetails()
        {
            try
            {
                var company = Organizations.ElementAt(SelectedFactoryIndex).OrganizationName;
                string qDate = CurrentDate.ToString("dd-MMM-yyyy");
                if (company != "Select an unit")
                {
                    var returnEmployees = await _hrService.GetDailyAbsentDetailsAsync(company, qDate);
                    if (returnEmployees != null)
                    {
                        Employees.ReplaceRange(returnEmployees);
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

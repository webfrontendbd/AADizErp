using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class EmployeeFileQueryPageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        [ObservableProperty]
        ObservableRangeCollection<EmployeeComboList> employees = new();
        [ObservableProperty]
        EmployeeInfoDto employeeInfoDto = new();
        [ObservableProperty]
        ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        string formattedDisplayText; 
        
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string cardNumber;
        [ObservableProperty]
        private string selectedFactory;

        public EmployeeFileQueryPageViewModel(HrService hrService)
        {
            _hrService=hrService;
            OrganizationList();
        }
        private async void OrganizationList()
        {
            Organizations.Add(new Organization { OrganizationName="Select an unit", Abbr="NA" });
            var userInfo = await App.GetUserInfo();

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
        async Task LoadingEmployeesWhenOrganizationSelectionChanged()
        {
            try
            {
                Employees.ReplaceRange(await _hrService.GetEmployeeByCompanyAsync(SelectedFactory));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

      

        [RelayCommand]
        async Task SelectionChangedForUser()
        {
            try
            {
                FormattedDisplayText= CardNumber+" - "+ Employees.FirstOrDefault(e => e.CardNumber==CardNumber).Name;
                EmployeeInfoDto = await _hrService.GetEmployeeDetailsInfoDataAsync(SelectedFactory, CardNumber);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

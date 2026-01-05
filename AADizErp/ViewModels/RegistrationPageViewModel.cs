using MvvmHelpers;
using AADizErp.Models;
using AADizErp.Services;
using CommunityToolkit.Mvvm.Input;
using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AADizErp.ViewModels
{
    public partial class RegistrationPageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        private readonly AuthenticationService _authService;
        [ObservableProperty]
        private ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        private ObservableRangeCollection<EmployeeComboList> employees = new();
        #region properties
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string fullName;
        [ObservableProperty]
        string username;
        [ObservableProperty]
        string password;
        [ObservableProperty]
        string confirmPassword;
        [ObservableProperty]
        string cardNumber;
        [ObservableProperty]
        string emailAddress;
        [ObservableProperty]
        string phone;
        [ObservableProperty]
        private string selectedFactory;
        [ObservableProperty]
        string formattedDisplayText;
        #endregion
        public RegistrationPageViewModel(HrService hrService, AuthenticationService authService)
        {
            _hrService = hrService;
            _authService = authService;
            OrganizationList();
        }
        private void OrganizationList()
        {
            Organizations.ReplaceRange(GetCompanies());
        }

        private List<Organization> GetCompanies()
        {
            List<Organization> list = new List<Organization>();
            list.Add(new Organization() { OrganizationName = "SELECT AN ORGANIZATION", Abbr="NA" });
            list.Add(new Organization() { OrganizationName = "ASIAN GROUP", Abbr="HO" });
            list.Add(new Organization() { OrganizationName = "ASIAN P&D DEPARTMENT", Abbr="PND" });
            list.Add(new Organization() { OrganizationName = "CHITTAGONG ASIAN APPARELS LTD.", Abbr="CTA" });
            list.Add(new Organization() { OrganizationName = "FASHION WATCH LIMITED (BUILDING-02)", Abbr="FWB" });
            list.Add(new Organization() { OrganizationName = "FASHION WATCH LIMITED", Abbr="FWL" });
            list.Add(new Organization() { OrganizationName = "PRIYAM GARMENTS LTD.", Abbr="PRI" });
            list.Add(new Organization() { OrganizationName = "FORTUNE APPARELS LTD.", Abbr="FOR" });
            list.Add(new Organization() { OrganizationName = "ZAKMARS FASHION LTD.", Abbr="ZAK" });
            list.Add(new Organization() { OrganizationName = "MEHER GARMENTS LTD.", Abbr="MEH" });
            list.Add(new Organization() { OrganizationName = "Mark Fashion Wear (Pvt.) Ltd. Unit-2", Abbr="MRK" });
            list.Add(new Organization() { OrganizationName = "MOHARA ASIAN APPARELS LTD.", Abbr="MOH" });
            list.Add(new Organization() { OrganizationName = "PANMARK APPARELS (PVT) LTD.", Abbr="PAN" });
            list.Add(new Organization() { OrganizationName = "SEA TEX LIMITED", Abbr="SEA" });
            list.Add(new Organization() { OrganizationName = "SEA BLUE TEXTILE LTD.", Abbr="SBT" });
            list.Add(new Organization() { OrganizationName = "SUBARNA GARMENTS LTD.", Abbr="SUB" });
            list.Add(new Organization() { OrganizationName = "LEGACY FASHION LTD.", Abbr="LEG" });
            list.Add(new Organization() { OrganizationName = "ANOWARA FASHIONS LTD.", Abbr="ANO" });
            list.Add(new Organization() { OrganizationName = "COTTONEX FASHIONS LTD.", Abbr="COT" });
            list.Add(new Organization() { OrganizationName = "DAF KNITWEARS LTD. ( OLD PORTION )", Abbr="DAO" });
            list.Add(new Organization() { OrganizationName = "DAF KNITWEARS LTD. ( EXTENDED PORTION )", Abbr="DAE" });
            list.Add(new Organization() { OrganizationName = "WASH LINE LIMITED.", Abbr="WL" });
            list.Add(new Organization() { OrganizationName = "M/S. FASHION WATCH WASHING.", Abbr="FWW" });

            return list;
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
                var employee = Employees.FirstOrDefault(e => e.CardNumber ==CardNumber);
                FormattedDisplayText= CardNumber+" - "+ employee.Name;
                if (employee != null)
                {
                    Username =  Organizations.FirstOrDefault(o => o.OrganizationName == SelectedFactory).Abbr+employee.CardNumber;
                    FullName = employee.Name;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SaveUser()
        {

            if (Password != null || FullName != null || CardNumber !=null || Username != null || SelectedFactory != "NA")
            {
                try
                {
                    var user = new RegisterDto
                    {
                        Name = FullName,
                        EmployeeNumber = CardNumber,
                        UserName = Username,
                        Password = Password,
                        PhoneNumber = Phone,
                        Email = EmailAddress.ToLower(),
                        OrganizationName = SelectedFactory,
                    };
                    //Minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character
                    var expectedPasswordPattern = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$");
                    var isValidPassword = expectedPasswordPattern.IsMatch(Password);
                    if (isValidPassword )
                    {
                        var response = await _authService.UnreviewedRegistration(user);
                        if (response.UserName != null)
                        {
                            await Shell.Current.GoToAsync($"{nameof(ThankYouPage)}");
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", "Duplicate username found", "OK");
                        }
                    }
                    else
                    {
                       
                        await Shell.Current.DisplayAlert("Password Error", "Minimum 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character", "OK");
                    }

                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Oops!", "Fill all the field and try again", "OK");
            }
        }
    }
}

using AADizErp.Models.Dtos;
using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class EmployeeTimeCardPageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        private readonly AttendanceService _attnService;

        #region Observable Property
        [ObservableProperty]
        ObservableRangeCollection<Organization> organizations = new();
        [ObservableProperty]
        Organization organization = new();
        [ObservableProperty]
        ObservableRangeCollection<EmployeeComboList> employees = new();
        [ObservableProperty]
        private ObservableRangeCollection<IndividualTimeCardDto> attendances = new();
        [ObservableProperty]
        private ObservableRangeCollection<IndividualTimeCardDto> filterAttendances = new();
        [ObservableProperty]
        string selectedFactory;
        [ObservableProperty]
        string formattedDisplayText;
        [ObservableProperty]
        DateTime currentDate = DateTime.Now;
        [ObservableProperty]
        string cardNumber;

        [ObservableProperty]
        DateTime startDate;

        [ObservableProperty]
        DateTime endDate;
        [ObservableProperty]
        int workingDays;
        [ObservableProperty]
        int presentDays;
        [ObservableProperty]
        int absentDays;
        [ObservableProperty]
        int leaveDays;
        [ObservableProperty]
        int weekdays;
        [ObservableProperty]
        int holidays;
        [ObservableProperty]
        int lateDays;
        [ObservableProperty]
        int monthDays;
        [ObservableProperty]
        string name;
        [ObservableProperty]
        decimal otHour;
        #endregion

        public EmployeeTimeCardPageViewModel(HrService hrService, AttendanceService attnService)
        {
            Title="Dashboard";
            _hrService = hrService;
            _attnService=attnService;
            OrganizationList();
            DateTime currentDate = DateTime.Now;
            StartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            //GetIndividualTimeCardDetails(StartDate, EndDate);
        }
        private void OrganizationList()
        {
            Organizations.Add(new Organization { OrganizationName="Select an unit" });
            Task.Run(async () =>
            {
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

            });

        }

        [RelayCommand]
        async Task GetEmployeeTimeCardDetails()
        {
            IsLoading = true;
            Attendances.Clear();

            FormattedDisplayText= CardNumber+" - "+ Employees.FirstOrDefault(e => e.CardNumber==CardNumber).Name;
            var returnAttendances = await _attnService.GetEmployeeTimeCardDetails(CardNumber, SelectedFactory, StartDate, EndDate);

            if (returnAttendances.Count > 0 || returnAttendances==null)
            {
                Attendances.ReplaceRange(returnAttendances);
                FilterAttendances.ReplaceRange(returnAttendances);
                CalculateAttendanceSummary(returnAttendances);
                IsLoading = false;
            }
            else
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        void FilterTimecardByStatus(string status)
        {
            if (status == "All")
            {
                FilterAttendances.ReplaceRange(Attendances);
            }
            else
            {
                FilterAttendances.ReplaceRange(Attendances.Where(a => a.Status ==status).ToList());
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
                var employee = Employees.FirstOrDefault(e => e.CardNumber ==CardNumber);
                if (employee != null)
                {
                    Name = employee.Name;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void CalculateAttendanceSummary(IReadOnlyList<IndividualTimeCardDto> attendances)
        {
            WorkingDays = attendances.Where(d => d.Status =="P" || d.Status == "A"||d.Status=="L").Count();
            Weekdays = attendances.Where(d => d.Status =="W").Count();
            Holidays = attendances.Where(d => d.Status =="H").Count();
            LeaveDays = attendances.Where(d => d.Status =="L").Count();
            PresentDays = attendances.Where(d => d.Status =="P").Count();
            AbsentDays = attendances.Where(d => d.Status =="A").Count();
            MonthDays = attendances.Count();
            LateDays = attendances.Where(d => d.Late > 0).Count();
            OtHour = attendances.Sum(d => d.Othour);
        }

    }
}

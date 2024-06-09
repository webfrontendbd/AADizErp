using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using AADizErp.Models.Dtos;
using MvvmHelpers;
using CommunityToolkit.Mvvm.Input;

namespace AADizErp.ViewModels.RequestVM
{
    public partial class IndividualTimeCardViewModel : BaseViewModel
    {
        #region obserable properties
        [ObservableProperty]
        private ObservableRangeCollection<IndividualTimeCardDto> attendances = new();
        [ObservableProperty]
        private ObservableRangeCollection<IndividualTimeCardDto> filterAttendances = new();
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
        decimal otHour;
        #endregion

        private readonly AttendanceService _attnService;

        public IndividualTimeCardViewModel(AttendanceService attnService)
        {
            _attnService = attnService;

            DateTime currentDate = DateTime.Now;
            StartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            EndDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
            GetIndividualTimeCardDetails(StartDate, EndDate);
        }

        private void GetIndividualTimeCardDetails(DateTime start, DateTime end)
        {
            IsLoading = true;
            Attendances.Clear();

            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();


                var returnAttendances = await _attnService.GetIndividualTimeCardDetails(userInfo, start, end);

                if (returnAttendances.Count > 0)
                {
                    App.Current.Dispatcher.Dispatch(() =>
                    {
                        Attendances.ReplaceRange(returnAttendances);
                        FilterAttendances.ReplaceRange(returnAttendances);
                        CalculateAttendanceSummary(returnAttendances);
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
        void DateChanged()
        {
            if (StartDate > EndDate)
            {
                Shell.Current.DisplayAlert("Error", "Date selection issue", "OK");
            }
            else
            {
                GetIndividualTimeCardDetails(StartDate, EndDate);
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
                FilterAttendances.ReplaceRange(Attendances.Where(a=>a.Status ==status).ToList());
            }
        }

        private void CalculateAttendanceSummary(IReadOnlyList<IndividualTimeCardDto> attendances)
        {
            WorkingDays = attendances.Where(d => d.Status =="P" || d.Status == "A" || d.Status=="L").Count();
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

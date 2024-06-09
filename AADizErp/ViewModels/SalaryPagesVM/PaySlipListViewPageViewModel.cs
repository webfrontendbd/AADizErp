using AADizErp.Models.Dtos;
using AADizErp.Services.RequestServices;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace AADizErp.ViewModels.SalaryPagesVM
{
    public partial class PaySlipListViewPageViewModel : BaseViewModel
    {
        private readonly AttendanceService _attnService;
        [ObservableProperty]
        private ObservableRangeCollection<PaySlipInfoDto> paySlips = new();
        public PaySlipListViewPageViewModel(AttendanceService attnService)
        {
            _attnService=attnService;
            GetIndividualPaySlipInformation();
        }

        private async void GetIndividualPaySlipInformation()
        {
            IsLoading = true;
            PaySlips.Clear();
            var userInfo = await App.GetUserInfo();
            var payslips = await _attnService.GetIndividualPaySlipInfo(userInfo);
            if (payslips != null)
            {
                App.Current.Dispatcher.Dispatch(() =>
                {
                    PaySlips.ReplaceRange(payslips);
                    IsLoading = false;
                });
            }
            else
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error", "Not Found", "OK");
                IsLoading = false;
            }
        }
    }
}

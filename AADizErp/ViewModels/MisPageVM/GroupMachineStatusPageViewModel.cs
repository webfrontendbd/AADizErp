using AADizErp.Models.Dtos.McDtos;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.MisPageVM
{
    public partial class GroupMachineStatusPageViewModel: BaseViewModel
    {
        private readonly MachineService _mcService;
        [ObservableProperty]
        ObservableRangeCollection<GroupMachineStatusDto> machineStatusDto = new();
        [ObservableProperty]
        DateTime currentDate = DateTime.Today.AddDays(-1);

        public GroupMachineStatusPageViewModel(MachineService mcService)
        {
            _mcService = mcService;
            MachineStatusSummaryOnPageLoad();
        }

        public async void MachineStatusSummaryOnPageLoad()
        {
            string reportdate = CurrentDate.ToString("dd-MMM-yyyy");
            var result = await _mcService.GetGroupMachineStatusByDate(reportdate);
            if (result != null)
            {
                MachineStatusDto.AddRange(result);
            }


        }
        [RelayCommand]
        async Task GetDailyMachineStatuSummary()
        {
            string reportdate = CurrentDate.ToString("dd-MMM-yyyy");
            try
            {
                var result = await _mcService.GetGroupMachineStatusByDate(reportdate);
                if (result != null)
                {
                    MachineStatusDto.ReplaceRange(result);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}

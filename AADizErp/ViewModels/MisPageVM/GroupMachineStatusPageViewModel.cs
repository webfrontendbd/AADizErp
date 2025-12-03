using AADizErp.Models.Dtos.McDtos;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.MisPageVM
{
    public partial class GroupMachineStatusPageViewModel : BaseViewModel
    {
        private readonly MachineService _mcService;
        [ObservableProperty]
        ObservableRangeCollection<MachineStatusSummaryDto> machineStatusDto = new();
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
                var groupedResult = result
                    .GroupBy(x => x.Orgname)
                    .Select(g => new MachineStatusSummaryDto
                    {
                        Orgname = g.Key,
                        Running = g.Where(s => s.Status == "Running").Sum(s => s.Quantity),
                        RunningIdle = g.Where(s => s.Status == "Running Idle").Sum(s => s.Quantity),
                        Idle = g.Where(s => s.Status == "Idle").Sum(s => s.Quantity),
                        Rent = g.Where(s => s.Status == "Rented").Sum(s => s.Quantity),
                        Total = g.Sum(s => s.Quantity)
                    })
                    .ToList();

                MachineStatusDto.ReplaceRange(groupedResult);
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
                    var groupedResult = result
                    .GroupBy(x => x.Orgname)
                    .Select(g => new MachineStatusSummaryDto
                    {
                        Orgname = g.Key,
                        Running = g.Where(s => s.Status == "Running").Sum(s => s.Quantity),
                        RunningIdle = g.Where(s => s.Status == "Running Idle").Sum(s => s.Quantity),
                        Idle = g.Where(s => s.Status == "Idle").Sum(s => s.Quantity),
                        Rent = g.Where(s => s.Status == "Rented").Sum(s => s.Quantity),
                        Total = g.Sum(s => s.Quantity)
                    })
                    .ToList();

                    MachineStatusDto.ReplaceRange(groupedResult);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

        }
    }
}

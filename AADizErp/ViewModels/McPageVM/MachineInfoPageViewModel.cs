using AADizErp.Models.Dtos.McDtos;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmHelpers;

namespace AADizErp.ViewModels.McPageVM
{
    [QueryProperty(nameof(Mcid), "Mcid")]
    public partial class MachineInfoPageViewModel : BaseViewModel
    {
        private readonly MachineService _mcService;

        [ObservableProperty]
        private MachineInfoDto machineInfoDto;

        [ObservableProperty]
        private ObservableRangeCollection<ServiceHistoryModel> serviceHistoryList = new();

        [ObservableProperty]
        private ObservableRangeCollection<SparePartsModel> sparepartsHistoryList = new();

        [ObservableProperty]
        private ObservableRangeCollection<MovementHistoryModel> movementHistoryList = new();

        [ObservableProperty]
        private string mcid;

        public MachineInfoPageViewModel(MachineService machineService)
        {
            _mcService = machineService;
        }

        partial void OnMcidChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _ = InitializePageAsync(value);
        }

        private async Task InitializePageAsync(string mcid)
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                // Load main machine info first
                MachineInfoDto = await _mcService.GetMachinePresentStatusByMcid(mcid);

                if (MachineInfoDto == null)
                    return;

                // Load all three histories in parallel
                var servicingTask = _mcService.GetMachineServicingHistoryByMcid(mcid);
                var sparePartsTask = _mcService.GetMachineSparePartsHistoryByMcid(mcid);
                var movementTask = _mcService.GetMachineMovementHistoryByMcid(mcid);

                await Task.WhenAll(servicingTask, sparePartsTask, movementTask);

                // Update lists (UI thread safe)
                ServiceHistoryList.ReplaceRange(servicingTask.Result);
                SparepartsHistoryList.ReplaceRange(sparePartsTask.Result);
                MovementHistoryList.ReplaceRange(movementTask.Result);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}

using AADizErp.Models.Dtos.McDtos;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AADizErp.ViewModels.McPageVM
{
    [QueryProperty(nameof(Mcid), "Mcid")]
    public partial class MachineInfoPageViewModel : BaseViewModel
    {
        private readonly MachineService _mcService;

        [ObservableProperty]
        private MachineInfoDto machineInfoDto;

        [ObservableProperty]
        private string mcid;

        public MachineInfoPageViewModel(MachineService machineService)
        {
            _mcService = machineService;
            IsLoading = false;
        }
        partial void OnMcidChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _ = InitializePageAsync(value);
        }

        private async Task InitializePageAsync(string mcid)
        {
            IsLoading = true;

            MachineInfoDto = await _mcService.GetMachinePresentStatusByMcid(mcid);
            
            IsLoading = false;
        }

        public List<ServiceHistoryModel> ServiceHistoryList { get; set; } =
        [
            new() { ServiceDate = DateTime.Now.AddMonths(-2), ServiceType = "Full Service", Remarks="All OK" },
            new() { ServiceDate = DateTime.Now.AddMonths(-6), ServiceType = "Cleaning", Remarks="Filter changed" }
        ];

        public List<SparePartsModel> SparePartsHistory { get; set; } =
        [
            new() { PartName="Filter", Quantity=1, ChangedDate=DateTime.Now.AddMonths(-3), Remarks="Routine maintenance" }
        ];

        public List<MovementHistoryModel> MovementHistory { get; set; } =
        [
            new() { FromLocation="Factory A", ToLocation="Factory B", MovedDate=DateTime.Now.AddMonths(-1), Note="Shifted for production" }
        ];
    }
}

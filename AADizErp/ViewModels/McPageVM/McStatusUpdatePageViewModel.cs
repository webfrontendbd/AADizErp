using AADizErp.Models.Dtos.McDtos;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AADizErp.ViewModels.McPageVM
{
    [QueryProperty("Mcid", "Mcid")]
    public partial class McStatusUpdatePageViewModel : BaseViewModel
    {
        private readonly MachineService _mcService;
        [ObservableProperty]
        MachineInfoDto machineInfoDto;
        [ObservableProperty]
        string mcid;
        public McStatusUpdatePageViewModel(MachineService machineService)
        {
            _mcService = machineService;
            GetMachinePresentStatus(mcid);
        }
        

        private void GetMachinePresentStatus(string mcid)
        {
            IsLoading = true;
            Task.Run(async () =>
            {
                var machineInfo = await _mcService.GetMachinePresentStatusByMcid(mcid);
                MachineInfoDto = machineInfo;
            });

        }
    }
}

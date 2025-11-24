using AADizErp.Models;
using AADizErp.Models.Dtos.McDtos;
using AADizErp.Pages.NptPages;
using AADizErp.Services.McServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace AADizErp.ViewModels.McPageVM
{
    [QueryProperty(nameof(Mcid), "Mcid")]
    public partial class McStatusUpdatePageViewModel : BaseViewModel
    {
        private readonly MachineService _mcService;

        [ObservableProperty]
        private MachineInfoDto machineInfoDto;

        [ObservableProperty]
        private string mcid;

        [ObservableProperty]
        private ObservableRangeCollection<MachineFloorDto> floors = new();

        [ObservableProperty]
        private ObservableRangeCollection<MachineLineDto> lines = new();

        [ObservableProperty]
        private int selectedFloorId;

        [ObservableProperty]
        private string selectedLineId;

        [ObservableProperty]
        private ObservableCollection<string> statusList = new()
        {
            "Running", "Running Idle", "Idle", "Rent", "Inactive"
        };

        [ObservableProperty]
        private string selectedStatus;

        public McStatusUpdatePageViewModel(MachineService machineService)
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
            await LoadFloorsAsync();
            await Task.Delay(50);

            MachineInfoDto = await _mcService.GetMachinePresentStatusByMcid(mcid);
            if (MachineInfoDto == null)
                return;

            SelectedStatus = MachineInfoDto.Status;

           

            var floorMatched = Floors.FirstOrDefault(f => f.Floorname == MachineInfoDto.Floorname);
            if (floorMatched != null)
                SelectedFloorId = floorMatched.Floorid;

            await LoadLinesAsync();
            await Task.Delay(50);

            var matched = Lines.FirstOrDefault(l => l.Linename == MachineInfoDto.Line);
            if (matched != null)
                SelectedLineId = matched.Lineid;

            IsLoading = false;
        }


        private async Task GetMachineInfoAsync(string mcid)
        {
            MachineInfoDto = await _mcService.GetMachinePresentStatusByMcid(mcid);
        }

        [RelayCommand]
        private async Task LoadFloorsAsync()
        {
            var result = await _mcService.GetMachineFloorByOrgid();
            Floors.ReplaceRange(result);
        }

        [RelayCommand]
        private async Task LoadLinesAsync()
        {
            var result = await _mcService.GetMachineLineByOrgid();
            Lines.ReplaceRange(result);
        }

        [RelayCommand]
        private async Task UpdateMachineStatusAsync()
        {
            IsLoading = true;
            UserInfo userInfo = await App.GetUserInfo();
            if (MachineInfoDto == null)
            {
                await Shell.Current.DisplayAlert("Error", "No machine data loaded", "OK");
                return;
            }
            
            try
            {
                MachineStatusUpdateDto updateDto = new MachineStatusUpdateDto
                {
                    Mcid = MachineInfoDto.Mcid,
                    Floorid = SelectedFloorId,
                    Floorname = Floors.FirstOrDefault(f => f.Floorid ==  SelectedFloorId)?.Floorname,
                    Brand = MachineInfoDto.Brand,
                    Model = MachineInfoDto.Model,
                    Serial = MachineInfoDto.Serial,
                    Line = Lines.FirstOrDefault(l => l.Lineid == SelectedLineId)?.Linename,
                    Status = SelectedStatus,
                    UpdatedBy = userInfo.TokenUserMetaInfo.UserName
                };

                var result = await _mcService.UpdateMachineStatusByMcid(updateDto);

                if (result > 0)
                {
                    await Shell.Current.DisplayAlert("Success", "Machine status updated.", "OK");
                    await Shell.Current.GoToAsync($"{nameof(McBarcodeScanPage)}");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Failed", "Nothing updated.", "OK");
                }
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

using AADizErp.Models.Dtos.HrDtos;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AADizErp.ViewModels.HrVM
{
    public partial class ProfilePageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        [ObservableProperty]
        EmployeeInfoDto employeeInfoDto = new();

        public ProfilePageViewModel(HrService hrService)
        {
            _hrService=hrService;
            GetEmployeeDetails();
        }

        public void GetEmployeeDetails()
        {
            Task.Run(async () =>
            {
                var userInfo = await App.GetUserInfo();
                EmployeeInfoDto = await _hrService.GetEmployeeDetailsInfoDataAsync(userInfo.TokenUserMetaInfo.OrganizationName, userInfo.TokenUserMetaInfo.EmployeeNumber);       

            });
        }
    }
}

using AADizErp.Models.Dtos;
using AADizErp.Services;
using AADizErp.Services.HrServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MvvmHelpers;

namespace AADizErp.ViewModels.HrVM
{
    public partial class UnreviewedUserPageViewModel : BaseViewModel
    {
        private readonly HrService _hrService;
        private readonly AuthenticationService _auth;
        [ObservableProperty]
        ObservableRangeCollection<UnreviewUser> unreviewedUserList = new();
        [ObservableProperty]
        UnreviewUser unreviewUser=new();
        [ObservableProperty]
        bool isPopupOpen=false;

        public UnreviewedUserPageViewModel(HrService hrService, AuthenticationService auth)
        {
            _hrService=hrService;
            _auth=auth;
            GetUnreviewedUser();
        }

        private async void GetUnreviewedUser()
        {
            var userInfo = await App.GetUserInfo();
            var listOfUsers = await _hrService.GetUnreviewedUserListAsync(userInfo.TokenUserMetaInfo.OrganizationName);
            if(listOfUsers == null)
            {
                await Shell.Current.DisplayAlert("Oops!", "No Result Found", "OK");
            }
            else
            {
                UnreviewedUserList.ReplaceRange(listOfUsers); 
            }
        }
        [RelayCommand]
        void UnreviewedApprovalPopupAction(UnreviewUser unreview)
        {
            if (unreview == null) return;
            UnreviewUser = unreview;
            IsPopupOpen = true;
        }

        [RelayCommand]
        async Task UnreviewedApprovalStatusSubmit(string status)
        {
            if (status == "Approve")
            {
                UnreviewUser.IsUserConfirmed = true;
            }
            else if(status =="Decline")
            {
                UnreviewUser.IsUserConfirmed = false;
            }

            var returnObject = await _auth.UnreviewedRegistrationApproval(UnreviewUser);

            if (returnObject != null)
            {
                var changedRequest = UnreviewedUserList.FirstOrDefault(l => l.Id == UnreviewUser.Id);
                if (changedRequest is not null)
                {
                    if(changedRequest.IsUserConfirmed)
                    {
                        UnreviewedUserList.Remove(UnreviewedUserList.FirstOrDefault(l => l.Id == changedRequest.Id));
                    }
                }

                IsPopupOpen = false;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Something went wrong", "OK");
            }

        }
    }
}

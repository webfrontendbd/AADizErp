using AADizErp.ViewModels.RequestVM;

namespace AADizErp.Pages.RequestPages;

public partial class LeaveRequestPage : ContentPage
{

    public LeaveRequestPage(LeaveRequestPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void CreateDXButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(LeaveRequestFormPopup)}");
    }
}
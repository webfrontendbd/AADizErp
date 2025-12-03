using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class UnconfirmEmployeeListPage : ContentPage
{
    public UnconfirmEmployeeListPage(UnconfirmEmployeeListPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void DXButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddUnconfirmEmployeePage)}");
    }
}
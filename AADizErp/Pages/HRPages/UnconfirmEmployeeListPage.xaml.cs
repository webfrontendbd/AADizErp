using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class UnconfirmEmployeeListPage : ContentPage
{
	public UnconfirmEmployeeListPage(UnconfirmEmployeeListPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    private async Task AddUnconfirmEmployeeDXButton_Tapped(object sender, DevExpress.Maui.Core.DXTapEventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddUnconfirmEmployeePage)}");
    }
}
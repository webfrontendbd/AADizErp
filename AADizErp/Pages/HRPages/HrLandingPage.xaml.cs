namespace AADizErp.Pages.HRPages;

public partial class HrLandingPage : ContentPage
{
	public HrLandingPage()
	{
		InitializeComponent();
	}

    private async void HrDashboardMenu_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(HrDashboardPage)}");
    } 
    
    private async void EmployeeTimeCardMenu_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(EmployeeTimeCardPage)}");
    }

    private async void EmployeeFileQueryMenu_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(EmployeeFileQueryPage)}");
    }
    private async void DailyAbsentDetailsQueryMenu_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(DailyAbsentDetailPage)}");
    }

    private async void UnreviewedUserListButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(UnreviewedUserListPage)}");
    }
    private async void UnconfirmEmployeeListButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AddUnconfirmEmployeePage)}");
    }

}
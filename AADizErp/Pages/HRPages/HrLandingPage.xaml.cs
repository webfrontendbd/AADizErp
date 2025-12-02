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
    private async void BasicRecruitmentProcessButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(AddUnconfirmEmployeePage)}");
        }
        catch (Exception ex)
        {

            await DisplayAlert("Navigation Error", $"An error occurred while navigating to the Unconfirm Employee Page: {ex.Message}", "OK");   
        }
        
    }
    private async void UnconfirmEmployeeListButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync($"{nameof(UnconfirmEmployeeListPage)}");
        }
        catch (Exception ex)
        {

            await DisplayAlert("Navigation Error", $"An error occurred while navigating to the Unconfirm Employee Page: {ex.Message}", "OK");   
        }
        
    }

}
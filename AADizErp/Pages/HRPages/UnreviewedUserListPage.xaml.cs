using AADizErp.ViewModels.HrVM;

namespace AADizErp.Pages.HRPages;

public partial class UnreviewedUserListPage : ContentPage
{
	public UnreviewedUserListPage(UnreviewedUserPageViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext=viewmodel;
	}
}
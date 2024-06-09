using AADizErp.ViewModels.RequestVM;

namespace AADizErp.Pages.RequestPages;

public class AttendanceQuery
{
	public DateTime DateFrom { get; set; }
	public DateTime DateTo { get; set; }
}
public partial class IndividualTimeCardViewPage : ContentPage
{
	public IndividualTimeCardViewPage(IndividualTimeCardViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
	}

    private void DataFormDateItem_BindingContextChanged(object sender, EventArgs e)
    {

    }
}
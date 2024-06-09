using AADizErp.ViewModels.HrVM;
using DevExpress.Maui.Core;

namespace AADizErp.Pages.HRPages;

public partial class HrDashboardPage : ContentPage
{
    public HrDashboardPage(HrDashboardPageViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
    }

}
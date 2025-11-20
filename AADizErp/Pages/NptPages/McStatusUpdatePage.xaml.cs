using AADizErp.ViewModels.McPageVM;

namespace AADizErp.Pages.NptPages;

public partial class McStatusUpdatePage : ContentPage
{
    public McStatusUpdatePage(McStatusUpdatePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
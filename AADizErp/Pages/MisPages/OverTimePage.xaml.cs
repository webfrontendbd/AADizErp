using AADizErp.ViewModels.MisPageVM;

namespace AADizErp.Pages.MisPages;

public partial class OverTimePage : ContentPage
{
    public OverTimePage(OverTimePageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
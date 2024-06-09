using DevExpress.Maui.Core;

namespace AADizErp.Pages.RequestPages;

public partial class SaveAttendancePage : ContentPage
{
    DetailEditFormViewModel ViewModel => (DetailEditFormViewModel)BindingContext;
    public SaveAttendancePage()
	{
		InitializeComponent();
	}
    void SaveItemClick(object sender, EventArgs e)
    {
        if (!dataForm.Validate())
            return;
        dataForm.Commit();
        ViewModel.Save();
    }
}
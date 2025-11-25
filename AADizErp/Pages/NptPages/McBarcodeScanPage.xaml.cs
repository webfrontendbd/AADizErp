using ZXing.Net.Maui;

namespace AADizErp.Pages.NptPages;

public partial class McBarcodeScanPage : ContentPage
{
    private bool _isNavigating = false;

    public McBarcodeScanPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
        }

        if (status == PermissionStatus.Granted)
        {
            BarcodeScannerView.IsDetecting = true;
        }
        else
        {
            await DisplayAlert("Permission Denied", "Camera permission is required to scan QR codes.", "OK");
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        BarcodeScannerView.IsDetecting = false;
    }


    protected void BarcodeScannerView_BarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (_isNavigating)
            return;

        _isNavigating = true;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                BarcodeScannerView.IsDetecting = false;

                var text = e.Results.First().Value;

                await Shell.Current.GoToAsync(
                    $"{nameof(McStatusUpdatePage)}?Mcid={Uri.EscapeDataString(text)}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ZXing Crash: {ex}");
                await DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                _isNavigating = false;
            }
        });
    }

    private void Torch_Clicked(object sender, EventArgs e)
    {
        try
        {
            BarcodeScannerView.IsTorchOn = !BarcodeScannerView.IsTorchOn;
        }
        catch
        {
            DisplayAlert("Torch Error", "Unable to toggle torch.", "OK");
        }
    }

    private void ScanAgain_Clicked(object sender, EventArgs e)
    {
        _isNavigating = false;  // reset
        BarcodeScannerView.IsDetecting = true;
        ResultLabel.Text = "Align QR Code in the box";
    }

    private async void FlipCamera_Clicked(object sender, EventArgs e)
    {
        try
        {
            BarcodeScannerView.CameraLocation =
                BarcodeScannerView.CameraLocation == CameraLocation.Rear
                ? CameraLocation.Front
                : CameraLocation.Rear;
        }
        catch
        {
            // safe async in event
            await DisplayAlert("Camera Error", "Unable to switch camera.", "OK");
        }
    }
}

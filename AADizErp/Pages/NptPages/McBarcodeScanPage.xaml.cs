using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace AADizErp.Pages.NptPages;

public partial class McBarcodeScanPage : ContentPage
{
    private bool _isNavigating = false;

    public McBarcodeScanPage()
    {
        InitializeComponent();
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
            finally
            {
                _isNavigating = false;
            }
        });
    }

    private void Torch_Clicked(object sender, EventArgs e)
    {
        BarcodeScannerView.IsTorchOn = !BarcodeScannerView.IsTorchOn;
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

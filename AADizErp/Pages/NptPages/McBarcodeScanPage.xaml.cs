using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace AADizErp.Pages.NptPages;

public partial class McBarcodeScanPage : ContentPage
{
	public McBarcodeScanPage()
	{
        InitializeComponent();
	}
    protected void BarcodeScannerView_BarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (e.Results?.Any() ?? false)
            {
                var result = e.Results.First();
                var text = result.Value;

                ResultLabel.Text = text;

                BarcodeScannerView.IsDetecting = false;

                // Save / process scanned data
                //await SaveScanAsync(text, result.Format.ToString(), DateTime.UtcNow);

                await DisplayAlert("Scanned", text, "OK");

                // resume if required
                // BarcodeScannerView.IsDetecting = true;
            }
        });
    }

    private void Torch_Clicked(object sender, EventArgs e)
    {
        BarcodeScannerView.IsTorchOn = !BarcodeScannerView.IsTorchOn;
    }
    private void ScanAgain_Clicked(object sender, EventArgs e)
    {
        ResultLabel.Text = "Align QR Code in the box";
        BarcodeScannerView.IsDetecting = true;
    }
    private void FlipCamera_Clicked(object sender, EventArgs e)
    {
        BarcodeScannerView.CameraLocation =
            BarcodeScannerView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }


}
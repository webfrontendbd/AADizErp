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

        // This event runs on a thread-pool thread; marshal to UI thread.
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (e.Results?.Any() ?? false)
            {
                // take first result
                var result = e.Results.First();
                var text = result.Value;

                // show result
                ResultLabel.Text = text;

                // optional: stop detecting if you want a single-shot scan
                BarcodeScannerView.IsDetecting = false;

                // Save / process scanned data
                //await SaveScanAsync(text, result.Format.ToString(), DateTime.UtcNow);

                // navigate or show popup
                await DisplayAlert("Scanned", text, "OK");

                // resume if required
                // BarcodeScannerView.IsDetecting = true;
            }
        });
    }

    private void Torch_Clicked(object sender, EventArgs e)
    {
        //BarcodeScannerView.ScannerOptions.TorchEnabled = !BarcodeScannerView.ScannerOptions.TorchEnabled;
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
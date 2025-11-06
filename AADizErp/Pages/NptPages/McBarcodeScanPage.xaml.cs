using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace AADizErp.Pages.NptPages;

public partial class McBarcodeScanPage : ContentPage
{
	public McBarcodeScanPage()
	{
        InitializeComponent();
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = true
        };
	}
    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        foreach (var barcode in e.Results)
            Console.WriteLine($"Barcodes: {barcode.Format} -> {barcode.Value}");
    }
}
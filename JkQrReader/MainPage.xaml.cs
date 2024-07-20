using BarcodeScanning;
using System.Diagnostics;

namespace JkQrReader
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            InitializeAsync();


            // Asenna marginaali dynaamisesti SizeChanged-tapahtuman kautta
            btTurnOnOff.SizeChanged += OnButtonSizeChanged;
        }
        private void OnButtonSizeChanged(object sender, EventArgs e)
        {
            if (btTurnOnOff.Height > 0)
            {
                var buttonHeight = btTurnOnOff.Height;
                cameraView.Margin = new Thickness(10, 10, 10, buttonHeight + 10);

                // Poista tapahtumakäsittelijä, jotta sitä ei kutsuta uudelleen
                btTurnOnOff.SizeChanged -= OnButtonSizeChanged;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            cameraView.CameraEnabled = true;
        }
        private async Task InitializeAsync()
        {
            await Methods.AskForRequiredPermissionAsync();
        }

        private void cameraView_OnDetectionFinished(object sender, OnDetectionFinishedEventArg e)
        {
            var result = e.BarcodeResults.FirstOrDefault();
            if (result != null)
            {
                cameraView.PauseScanning = true;
                Dispatcher.DispatchAsync(async () => await ShowBarCode(result));
            }
        }

        private void btTurnOnOff_Clicked(object sender, EventArgs e)
        {
            cameraView.CameraEnabled = !cameraView.CameraEnabled;
            btTurnOnOff.Source = cameraView.CameraEnabled ? "video_on.png" : "video_off.png";
        }
        private async Task ShowBarCode(BarcodeResult e)
        {
            await DisplayAlert("Koodi luettu", e.DisplayValue + " " + Environment.NewLine + "Tyyppi: " + e.BarcodeFormat, "OK");
            cameraView.PauseScanning = false;
        }

        private void btTorchOnOff_Clicked(object sender, EventArgs e)
        {
            cameraView.TorchOn = !cameraView.TorchOn;
            btTorchOnOff.Source = cameraView.TorchOn ? "torch_on.png" : "torch_off.png";
        }
    }

}

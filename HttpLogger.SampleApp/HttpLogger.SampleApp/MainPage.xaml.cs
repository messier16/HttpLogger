using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HttpLogger.SampleApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            var loggingHandler = new HttpLoggingHandler(new HttpClientHandler(), RequestAction, ResponseAction);
            var c = new HttpClient(loggingHandler);

            var result = await c.GetStringAsync("http://pokeapi.co/api/v2/pokemon/1/");
        }

        private Task ResponseAction(HttpResponseMessage httpResponseMessage)
        {
            Device.BeginInvokeOnMainThread(() => { LabelResponse.Text = httpResponseMessage.ToString(); });

            return Task.FromResult(0);
        }

        private Task RequestAction(HttpRequestMessage httpRequestMessage)
        {
            Device.BeginInvokeOnMainThread(() => { LabelRequest.Text = httpRequestMessage.ToString(); });

            return Task.FromResult(0);
        }
    }
}
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
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
            var sndr = sender as Button;
            HttpClient client;
#if DEBUG
            var loggingHandler = new HttpLoggingHandler(new NativeMessageHandler(), RequestAction, ResponseAction);
            client = new HttpClient(loggingHandler);
#else
			client = new HttpClient(new NativeMessageHandler());
#endif
            sndr.Text = "Querying";
            sndr.IsEnabled = false;
            await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon/1/");
            sndr.Text = "Query";
            sndr.IsEnabled = true;
        }

        private async Task ResponseAction(HttpResponseMessage httpResponseMessage)
        {
            string content = null;
            if (httpResponseMessage.Content != null)
            {
                content = await httpResponseMessage.Content.ReadAsStringAsync();
                content = content.Substring(0, Math.Min(100, content.Length)) + "...";
            }
            Device.BeginInvokeOnMainThread(() =>
            {

                var fs = new FormattedString();

                fs.Spans.Add(new Span { Text = "Status: " });
                fs.Spans.Add(new Span { Text = httpResponseMessage.StatusCode.ToString(), FontAttributes = FontAttributes.Bold });

                fs.Spans.Add(NewLine());

                if (httpResponseMessage.Headers.Any())
                {
                    fs.Spans.Add(new Span { Text = "Headers:" });
                    fs.Spans.Add(NewLine());
                    foreach (var header in httpResponseMessage.Headers)
                    {
                        fs.Spans.Add(new Span { Text = "\t•" + header.Key + ": " });
                        fs.Spans.Add(new Span { Text = String.Join(",", header.Value), FontAttributes = FontAttributes.Bold });
                        fs.Spans.Add(NewLine());
                    }
                }

                if (content != null)
                {
                    fs.Spans.Add(new Span { Text = "Content: " });
                    fs.Spans.Add(new Span { Text = content, FontAttributes = FontAttributes.Bold });
                }

                LabelResponse.FormattedText = fs;
            });
        }

        private async Task RequestAction(HttpRequestMessage httpRequestMessage)
        {
            string content = null;
            if (httpRequestMessage.Content != null)
            {
                content = await httpRequestMessage.Content.ReadAsStringAsync();
            }

            Device.BeginInvokeOnMainThread(() =>
            {

                var fs = new FormattedString();

                fs.Spans.Add(new Span { Text = "URL: " });
                fs.Spans.Add(new Span { Text = httpRequestMessage.RequestUri.ToString(), FontAttributes = FontAttributes.Bold });

                fs.Spans.Add(NewLine());

                fs.Spans.Add(new Span { Text = "Method: " });
                fs.Spans.Add(new Span { Text = httpRequestMessage.Method.ToString(), FontAttributes = FontAttributes.Bold });

                fs.Spans.Add(NewLine());

                if (httpRequestMessage.Headers.Any())
                {
                    fs.Spans.Add(new Span { Text = "Headers:" });
                    fs.Spans.Add(NewLine());
                    foreach (var header in httpRequestMessage.Headers)
                    {
                        fs.Spans.Add(new Span { Text = "\t•" + header.Key + ": " });
                        fs.Spans.Add(new Span { Text = String.Join(",", header.Value), FontAttributes = FontAttributes.Bold });
                    }
                }

                if (content != null)
                {
                    fs.Spans.Add(new Span { Text = "Content: " });
                    fs.Spans.Add(new Span { Text = content, FontAttributes = FontAttributes.Bold });
                }

                LabelRequest.FormattedText = fs;
            });
        }

        Span NewLine()
        {
            return new Span { Text = "\n" };
        }
    }
}
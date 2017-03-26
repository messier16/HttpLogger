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
			HttpClient client;

#if DEBUG
			var loggingHandler = new HttpLoggingHandler(new HttpClientHandler(), RequestAction, ResponseAction);
			client = new HttpClient(loggingHandler);
#else
			client = new HttpClient(new NativeMessageHandler());
#endif
			var result = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon/1/");
		}

        private Task ResponseAction(HttpResponseMessage httpResponseMessage)
        {
            Device.BeginInvokeOnMainThread(() => { 


				var fs = new FormattedString();

				fs.Spans.Add(new Span { Text = "Status: " });
				fs.Spans.Add(new Span { Text = httpResponseMessage.StatusCode.ToString(), FontAttributes = FontAttributes.Bold });

				fs.Spans.Add(NewLine());

				if (httpResponseMessage.Headers.Any())
				{
					fs.Spans.Add(new Span { Text = "Headers:" });
					foreach (var header in httpResponseMessage.Headers)
					{
						fs.Spans.Add(new Span { Text = "\t" + header.Key });
						fs.Spans.Add(new Span { Text = String.Join(",",header.Value), FontAttributes = FontAttributes.Bold });
						fs.Spans.Add(NewLine());
					}
				}

				LabelResponse.FormattedText = fs;
			});
            return Task.FromResult(0);
        }

        private Task RequestAction(HttpRequestMessage httpRequestMessage)
        {
            Device.BeginInvokeOnMainThread(() => {

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
					foreach (var header in httpRequestMessage.Headers)
					{
						fs.Spans.Add(new Span { Text = "\t" + header.Key });
						fs.Spans.Add(new Span { Text = String.Join(",",header.Value), FontAttributes = FontAttributes.Bold });
					}
				}


				LabelRequest.FormattedText = fs;
			});
            return Task.FromResult(0);
        }

		Span NewLine()
		{
			return new Span { Text = "\n" };
		}
    }
}
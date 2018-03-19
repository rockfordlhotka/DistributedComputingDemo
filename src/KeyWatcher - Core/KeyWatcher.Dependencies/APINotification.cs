using KeyWatcher.Messages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	internal sealed class APINotification
		: INotification
	{
		private readonly HttpClient httpClient;
		private readonly Uri uri;

		public APINotification(HttpClient httpClient, Uri uri)
		{
			this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
			this.uri = uri ?? throw new ArgumentNullException(nameof(uri));
		}

		public async Task SendAsync(string recipient, string title, string message)
		{
			var jsonMessage = JsonConvert.SerializeObject(
				new NotificationMessage(recipient, title, message), Formatting.Indented);
			var content = new StringContent(jsonMessage,
				Encoding.Unicode, "application/json");
			await this.httpClient.PostAsync(this.uri, content);
		}
	}
}

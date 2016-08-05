using System;
using System.Threading.Tasks;

namespace KeyWatcher.Dependencies
{
	internal sealed class Email
		: IEmail
	{
		public async Task SendAsync(string recipient, string title, string message)
		{
			await Console.Out.WriteLineAsync(
				$"To: {recipient}, Title: {title}, Message: {message}");
			await Task.Delay(30);
		}
	}
}

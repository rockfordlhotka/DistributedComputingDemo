using System;

namespace KeyWatcher
{
	public static class Common
	{
		public static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";

		public static readonly Uri BaseUri = new Uri("http://localhost:63899");
		public static readonly Uri KeyWatcherApiUri = new Uri(Common.BaseUri, "/api/keywatcher");
		public static readonly Uri NotificationApiUri = new Uri(Common.BaseUri, "/api/notification");
		public static readonly string KeyWatcherPartialUri = "/kwh";
		public static readonly Uri KeyWatcherHubApiUri = new Uri(Common.BaseUri, Common.KeyWatcherPartialUri);
		public const string NotificationSent = nameof(Common.NotificationSent);
	}
}

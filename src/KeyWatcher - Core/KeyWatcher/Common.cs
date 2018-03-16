using System;

namespace KeyWatcher
{
	public static class Common
	{
		public static string GetUserName() =>
			$"{(!string.IsNullOrWhiteSpace(Environment.UserDomainName) ? $"{Environment.UserDomainName}-" : string.Empty)}{Environment.UserName}";

		public static readonly Uri ApiUri = new Uri("http://localhost:63899/api/keywatcher");
	}
}

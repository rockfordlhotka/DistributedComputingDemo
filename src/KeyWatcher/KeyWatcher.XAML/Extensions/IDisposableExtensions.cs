using System;

namespace KeyWatcher.XAML.Extensions
{
	public static class IDisposableExtensions
	{
		public static void SafeDispose(this IDisposable @this)
		{
			if (@this != null)
			{
				@this.Dispose();
			}
		}
	}
}

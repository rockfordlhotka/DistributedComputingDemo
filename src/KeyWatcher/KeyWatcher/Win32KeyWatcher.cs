using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyWatcher
{
	// Lifted from:
	// http://null-byte.wonderhowto.com/how-to/create-simple-hidden-console-keylogger-c-sharp-0132757/
	public abstract class Win32KeyWatcher
		: KeyWatcher
	{
		private Win32.LowLevelKeyboardProc keyboardProc;

		protected Win32KeyWatcher()
			: base()
		{
			this.HookId = this.SetHook();
		}

		public override void Dispose()
		{
			Win32.UnhookWindowsHookEx(this.HookId);
			this.keyboardProc = null;
		}

		private IntPtr KeyboardProcessor(
			 int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0 && wParam == (IntPtr)Win32.WM_KEYDOWN)
			{
				var keyCode = Marshal.ReadInt32(lParam);
				this.HandleKey(keyCode);
			}

			return Win32.CallNextHookEx(this.HookId, nCode, wParam, lParam);
		}

		private IntPtr SetHook()
		{
			this.keyboardProc = new Win32.LowLevelKeyboardProc(this.KeyboardProcessor);

			using (var process = Process.GetCurrentProcess())
			{
				using (var module = process.MainModule)
				{
					return Win32.SetWindowsHookEx(Win32.WH_KEYBOARD_LL, 
						this.keyboardProc,
						Win32.GetModuleHandle(module.ModuleName), 0);
				}
			}
		}

		public IntPtr HookId { get; private set; }
	}
}

using System;
using System.Runtime.InteropServices;

namespace KeyWatcher
{
	internal static class Win32
	{
		internal const int SW_HIDE = 0;
		internal const int WH_KEYBOARD_LL = 13;
		internal const int WM_KEYDOWN = 0x0100;

		internal delegate IntPtr LowLevelKeyboardProc(
			 int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
			 IntPtr wParam, IntPtr lParam);
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GetConsoleWindow();
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr GetModuleHandle(string lpModuleName);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetWindowsHookEx(int idHook,
			 Win32.LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
		[DllImport("user32.dll")]
		internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool UnhookWindowsHookEx(IntPtr hhk);
	}
}

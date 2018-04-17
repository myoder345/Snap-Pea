using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SnapPeaApp
{
    /// <summary>
    /// Class that encapsulates WinAPI methods and types
    /// </summary>
    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        internal enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        internal enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        internal delegate IntPtr CallbackDelegate(int code, IntPtr wParam, IntPtr lParam);

        internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax,
            IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        internal const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        internal const uint WINEVENT_OUTOFCONTEXT = 0;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

        internal const uint SWP_NOZORDER = 0x0004;
        internal const uint SWP_SHOWWINDOW = 0x0040;
    }
}

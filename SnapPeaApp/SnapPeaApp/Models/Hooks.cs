using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapPeaApp
{
    class Hooks
    {
        [StructLayout(LayoutKind.Sequential)]
        struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        #region Dll_Imports
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax,
            IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        const uint WINEVENT_OUTOFCONTEXT = 0;

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

        const uint SWP_NOZORDER = 0x0004;
        const uint SWP_SHOWWINDOW = 0x0040;

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        #endregion

        IntPtr hhook;
        WinEventDelegate procDelegate;

        public Hooks(Action<IntPtr, uint, IntPtr, object, int, int, uint, uint> winEventProc)
        {
            procDelegate = new WinEventDelegate(winEventProc);
            hhook = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                   procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        public Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public string GetWindowName(object wrapper, IntPtr handle)
        {
            int capacity = GetWindowTextLength(new HandleRef(wrapper, handle)) + 1;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(wrapper, handle), stringBuilder, stringBuilder.Capacity);
            return stringBuilder.ToString();
        }

        public void MoveWindow(IntPtr hwnd, int x, int y, int width, int height)
        {
            SetWindowPos(hwnd, 0, x, y, width, height, SWP_NOZORDER | SWP_SHOWWINDOW);
        }

        public void Unhook()
        {
            UnhookWinEvent(hhook);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SnapPeaApp
{
    /// <summary>
    /// Contains P/invoke and WinAPI methods
    /// </summary>
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
        bool isWin10;

        public Layout CurrentLayout { get; set; }

        public Hooks()
        {
            procDelegate = new WinEventDelegate(WinEventProc);
            hhook = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                   procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
            isWin10 = System.Environment.OSVersion.Version.Major == 10;
        }

        Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        string GetWindowName(object wrapper, IntPtr handle)
        {
            int capacity = GetWindowTextLength(new HandleRef(wrapper, handle)) + 1;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(wrapper, handle), stringBuilder, stringBuilder.Capacity);
            return stringBuilder.ToString();
        }

        void MoveWindow(IntPtr hwnd, int x, int y, int width, int height)
        {
            if(isWin10)
            {
                // Win10 places an invisible border around window which we have to account for.
                SetWindowPos(hwnd, 0, x - 8, y, width + 16, height + 8, SWP_NOZORDER | SWP_SHOWWINDOW);
            }
            else
            {
                SetWindowPos(hwnd, 0, x, y, width, height, SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }

        public void Unhook()
        {
            UnhookWinEvent(hhook);
        }

        /// <summary>
        /// Callback function called whenever a window drag stops.
        /// Resizes the dragged window if it is released within a region
        /// </summary>
        /// <param name="hWinEventHook">Handle to event hook function</param>
        /// <param name="eventType">Specifies event that occurred (window moved)</param>
        /// <param name="hwnd">Handle to window moved</param>
        /// <param name="sender">Object that triggered event</param>
        /// <param name="idObject">ID of object associated with event</param>
        /// <param name="idChild">Specifies whether event was trigger by object or child of object</param>
        /// <param name="dwEventThread">ID of thread that generated event</param>
        /// <param name="dwmsEventTime">Time in milliseconds that the event was generated</param>
        void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            var windowName = GetWindowName(sender, hwnd);
            var cursorPos = GetMousePosition();
            Console.WriteLine($"Window {windowName} Moved, {cursorPos.ToString()}");

            // Check if window released within a region. If so, resize.
            foreach (Region r in CurrentLayout.Regions)
            {
                if (r.IsPointIn(new Point(cursorPos.X, cursorPos.Y)))
                {
                    MoveWindow(hwnd, r.Left, r.Top, r.Width, r.Height);
                }
            }
        }
    }
}

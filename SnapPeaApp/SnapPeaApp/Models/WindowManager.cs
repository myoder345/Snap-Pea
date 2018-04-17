using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using static SnapPeaApp.NativeMethods;

namespace SnapPeaApp
{
    /// <summary>
    /// Contains P/invoke and WinAPI methods
    /// </summary>
    class WindowManager : IDisposable
    {
        SafeHandle safeHandle;
        WinEventDelegate procDelegate;
        bool isWin10;
        
        public Layout CurrentLayout { get; set; }

        public WindowManager()
        {
            procDelegate = new WinEventDelegate(WinEventProc);

            var handle = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
               procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);

            safeHandle = new SafeHandle(handle,UnhookWinEvent);
            isWin10 = System.Environment.OSVersion.Version.Major == 10;
        }

        static Point GetMousePosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
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
            Point cursorPos = GetMousePosition();
            // Check if window released within a region. If so, resize.
            foreach (Region r in CurrentLayout.Regions)
            {
                if (r.IsPointIn(new Point(cursorPos.X, cursorPos.Y)))
                {
                    MoveWindow(hwnd, r.Left, r.Top, r.Width, r.Height);
                }
            }
        }

        #region IDisposable Support
         bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    procDelegate = null;
                    safeHandle.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapPeaApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        Hooks winHook;
        public MainWindowViewModel()
        {
            winHook = new Hooks(WinEventProc);
        }

        /// <summary>
        /// Callback function called whenever a window drag stops
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
            var windowName = winHook.GetWindowName(sender, hwnd);
            var cursorPos = winHook.GetMousePosition();
            Console.WriteLine($"Window {windowName} Moved, {cursorPos.ToString()}");

            // region hit test here
            if (cursorPos.X < 100 && cursorPos.Y < 100)
            {
                winHook.MoveWindow(hwnd, 0, 0, 600, 600);
            }
        }

        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            winHook.Unhook();
        }
    }
}

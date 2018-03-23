using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace SnapPeaApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        Layout currentLayout;
        Hooks winHook;

        public MainWindowViewModel()
        {
            winHook = new Hooks(WinEventProc);
            currentLayout = LoadDefaultLayout();
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

            foreach(Region r in currentLayout.Regions)
            {
                if (r.IsPointIn(new Point(cursorPos.X, cursorPos.Y)))
                {
                    winHook.MoveWindow(hwnd, r.Left, r.Top, r.Width, r.Height);
                }
            }
        }

        private Layout LoadLayout(string filepath)
        {
            throw new NotImplementedException();
        }

        private Layout LoadDefaultLayout()
        {
            return JsonConvert.DeserializeObject<Layout>(File.ReadAllText(Config.Configuration.getStringSetting(Config.ConfigKeys.DefaultLayout)));
        }

        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            winHook.Unhook();
        }
    }
}

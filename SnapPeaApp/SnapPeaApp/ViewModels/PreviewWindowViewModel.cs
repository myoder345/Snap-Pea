using SnapPeaApp.Config;
using SnapPeaApp.WinAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// ViewModel for PreviewWindow
    /// </summary>
    class PreviewWindowViewModel : ViewModelBase, IDisposable
    {
        NativeMethods.CallbackDelegate cbDelegate;
        MySafeHandle safeHandle;

        public PreviewWindowViewModel(Layout layout)
        {
            Regions = layout.Regions;
            cbDelegate = new NativeMethods.CallbackDelegate(KeyboardProc);
            IntPtr hook = NativeMethods.SetWindowsHookEx(NativeMethods.HookType.WH_KEYBOARD_LL, cbDelegate, IntPtr.Zero, 0);
            safeHandle = new MySafeHandle(hook, NativeMethods.UnhookWindowsHookEx);
            Layout.LayoutChanged += LayoutChangedEventHandler;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public IList<Region> Regions { get; private set; }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }

        public ICommand ClosingCommand
        {
            get { return new RelayCommand(OnClosing); }
        }

        void OnClosing(object o)
        {
            Layout.LayoutChanged -= LayoutChangedEventHandler;
            Dispose();
        }

        void LayoutChangedEventHandler(object sender, LayoutEventArgs e)
        {
            Regions = e.Layout.Regions;
            OnPropertyChanged(nameof(Regions));
        }

        /// <summary>
        /// This is the keyboard hook.
        /// We will listen for the preview key and display
        /// the preview window when it's pressed.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        private IntPtr KeyboardProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                return NativeMethods.CallNextHookEx(safeHandle.DangerousGetHandle(), code, wParam, lParam);
            }

            NativeMethods.KeyEvents kEvent = (NativeMethods.KeyEvents)wParam;

            Int32 vkCode = Marshal.ReadInt32((IntPtr)lParam);

            if (vkCode == Configuration.GetIntSetting(ConfigKeys.PreviewKey))
            {
                if (kEvent == NativeMethods.KeyEvents.KeyDown)
                {
                    if (((int)lParam & 0x40000000) == 0)
                    {
                        IsVisible = true;
                    }
                }
                else if (kEvent == NativeMethods.KeyEvents.KeyUp)
                {
                    IsVisible = false;
                }
            }

            return NativeMethods.CallNextHookEx(safeHandle.DangerousGetHandle(), code, wParam, lParam);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cbDelegate = null;
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

using SnapPeaApp.Config;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// ViewModel for PreviewWindow
    /// </summary>
    class PreviewWindowViewModel : ViewModelBase, IDisposable
    {
        NativeMethods.CallbackDelegate cbDelegate;
        SafeHandle safeHandle;

        public PreviewWindowViewModel(Layout layout)
        {
            Regions = layout.Regions;
            cbDelegate = new NativeMethods.CallbackDelegate(KeyboardProc);
            IntPtr hook = NativeMethods.SetWindowsHookEx(NativeMethods.HookType.WH_KEYBOARD_LL, cbDelegate, IntPtr.Zero, 0);
            safeHandle = new SafeHandle(hook, NativeMethods.UnhookWindowsHookEx);
            Layout.LayoutLoaded += LayoutChangedEventHandler;
        }

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
            Layout.LayoutLoaded -= LayoutChangedEventHandler;
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

            Int32 vkCode = Marshal.ReadInt32(lParam);

            // Key Down message
            if (kEvent == NativeMethods.KeyEvents.KeyDown)
            {
                if (vkCode == Configuration.GetIntSetting(ConfigKeys.PreviewKey) && (int)Keyboard.Modifiers == Configuration.GetIntSetting(ConfigKeys.PreviewKeyModifiers))
                {
                    if (((int)lParam & 0x40000000) == 0)
                    {
                        IsVisible = true;
                    }
                }
            }
            // Key up message
            if (kEvent == NativeMethods.KeyEvents.KeyUp)
            {
                if(vkCode == Configuration.GetIntSetting(ConfigKeys.PreviewKey) || (int)Keyboard.Modifiers != Configuration.GetIntSetting(ConfigKeys.PreviewKeyModifiers))
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

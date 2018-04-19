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

            // subscribe to LayoutLoaded event
            Layout.LayoutLoaded += LayoutChangedEventHandler;
        }

        /// <summary>
        /// Collection of regions. Bound to window ItemControl.ItemSource
        /// </summary>
        public IList<Region> Regions { get; private set; }

        /// <summary>
        /// Bound to window Visibility property
        /// </summary>
        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set { SetProperty(ref isVisible, value); }
        }

        /// <summary>
        /// Bound to window Closing event
        /// </summary>
        public ICommand ClosingCommand
        {
            get { return new RelayCommand(OnClosing); }
        }

        /// <summary>
        /// Closing event handler
        /// </summary>
        /// <param name="o"></param>
        void OnClosing(object o)
        {
            Layout.LayoutLoaded -= LayoutChangedEventHandler;
            Dispose();
        }

        /// <summary>
        /// Event handler for LayoutChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LayoutChangedEventHandler(object sender, LayoutEventArgs e)
        {
            Regions = e.Layout.Regions;
            OnPropertyChanged(nameof(Regions));
        }

        /// <summary>
        /// Keyboard hook callback
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

            // get tigger source key (virtual keyboard code)
            Int32 vkCode = Marshal.ReadInt32(lParam);

            // Key Down message
            if (kEvent == NativeMethods.KeyEvents.KeyDown)
            {
                // check that pressed key and modifiers match hotkey in settings
                if (vkCode == Configuration.GetIntSetting(ConfigKeys.PreviewKey) && (int)Keyboard.Modifiers == Configuration.GetIntSetting(ConfigKeys.PreviewKeyModifiers))
                {
                    // ignroe keyboard auto-repeat messages
                    if (((int)lParam & 0x40000000) == 0)
                    {
                        IsVisible = true;
                    }
                }
            }
            // Key up message
            if (kEvent == NativeMethods.KeyEvents.KeyUp)
            {
                // check that pressed key and modifiers match hotkey in settings
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

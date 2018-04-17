using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using SnapPeaApp.Config;
using SnapPeaApp.Views;
using SnapPeaApp.WinAPI;

namespace SnapPeaApp.ViewModels
{
    /// <summary>
    /// Contains interaction logic for MainWindow view
    /// </summary>
    class MainWindowViewModel : ViewModelBase, IDisposable
    {
        WindowManager winHook;
        NativeMethods.CallbackDelegate cbDelegate;
        private PreviewWindow previewWindow = null;
        private IntPtr hookId = IntPtr.Zero;

        public MainWindowViewModel()
        {
            winHook = new WindowManager();
            cbDelegate = new NativeMethods.CallbackDelegate(KeyboardProc);
            hookId = NativeMethods.SetWindowsHookEx(NativeMethods.HookType.WH_KEYBOARD_LL, cbDelegate, IntPtr.Zero, 0);
            Config.Configuration.LoadConfig();
            LoadDefaultLayout();
        }

        /// <summary>
        /// String for layout name text block
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string LayoutName
        {
            get
            {
                return String.IsNullOrEmpty(winHook.CurrentLayout.Name) ? "No layout loaded" : winHook.CurrentLayout.Name;
            }
        }

        #region Commands
        /// <summary>
        /// Bound to Create Layout button
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public ICommand CreateLayoutCommand
        {
            get { return new RelayCommand(o => OpenLayoutEditor(new Layout())); }
        }
        
        /// <summary>
        /// Bound to Edit Layout button
        /// </summary>
        public ICommand EditLayoutCommand
        {
            get { return new RelayCommand(o => OpenLayoutEditor(winHook.CurrentLayout)); }
        }

        /// <summary>
        /// Bound to Load Layout button
        /// </summary>
        public ICommand LoadLayoutCommand
        {
            get { return new RelayCommand(o => BrowseLayout()); }
        }

        /// <summary>
        /// Bound to Settings button
        /// </summary>
        public ICommand SettingsWindowCommand
        {
            get { return new RelayCommand(o => OpenSettingsWindow()); }
        }

        #endregion

        /// <summary>
        /// Sets winHook.CurrentLayout to the default layout
        /// </summary>
        private void LoadDefaultLayout()
        {
            winHook.CurrentLayout = Layout.LoadLayout(Config.Configuration.GetStringSetting(Config.ConfigKeys.DefaultLayout));

            if (previewWindow != null)
            {
                previewWindow.Close();
            }

            previewWindow = new PreviewWindow(winHook.CurrentLayout);
        }

        /// <summary>
        /// Opens file browser and loads selected layout
        /// </summary>
        private void BrowseLayout()
        {
            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Config.Configuration.GetStringSetting(Config.ConfigKeys.LayoutsPath),
                Filter = "json (*.json)|*.json|All files (*.*)|*.*"
            };

            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                winHook.CurrentLayout = Layout.LoadLayout(fileDialog.FileName);
                OnPropertyChanged("LayoutName");

                if (previewWindow != null)
                {
                    previewWindow.Close();
                }

                previewWindow = new PreviewWindow(winHook.CurrentLayout);
            }
        }

        /// <summary>
        /// Opens settings window
        /// </summary>
        private void OpenSettingsWindow()
        {
            var window = new SettingsWindow();
            window.ShowDialog();

            // temp 
            //var window = new PreviewWindow(winHook.CurrentLayout);
            //window.Show();
        }

        /// <summary>
        /// Opens layout editer window
        /// </summary>
        /// <param name="layout"></param>
        private static void OpenLayoutEditor(Layout layout)
        {
            var window = new LayoutEditorWindow(layout);
            window.Show();
            window.Activate();
        }

        /// <summary>
        /// Event handler for MainWindow closing.
        /// Unhooks message hooks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Dispose();
        }

        /// <summary>
        /// This is the keyboard hook.
        /// We will listen for the preview key and display
        /// the preview window when it's pressed.
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="W"></param>
        /// <param name="L"></param>
        private IntPtr KeyboardProc(int Code, IntPtr W, IntPtr L)
        {
            NativeMethods.KBDLLHookStruct LS = new NativeMethods.KBDLLHookStruct();

            if(Code < 0)
            {
                return NativeMethods.CallNextHookEx(hookId, Code, W, L);
            }

            NativeMethods.KeyEvents kEvent = (NativeMethods.KeyEvents)W;

            Int32 vkCode = Marshal.ReadInt32((IntPtr)L);

            if (vkCode == Configuration.GetIntSetting(ConfigKeys.PreviewKey))
            {
                if (kEvent == NativeMethods.KeyEvents.KeyDown)
                {
                    if (previewWindow != null)
                    {
                        previewWindow.Show();
                        //previewWindow.Activate();
                    }
                }
                else if (kEvent == NativeMethods.KeyEvents.KeyUp)
                {
                    if (previewWindow != null)
                    {
                        previewWindow.Hide();
                    }
                }
            }

            return NativeMethods.CallNextHookEx(hookId, Code, W, L);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    winHook.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}

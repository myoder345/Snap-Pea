using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SplitScreenWindows
{
    class Hooks
    {
        #region Dll_Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(HandleRef hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, 
            IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
            uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        const uint EVENT_SYSTEM_MOVESIZEEND = 0x000B;
        const uint WINEVENT_OUTOFCONTEXT = 0;

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        #endregion

        IntPtr hhook;
        WinEventDelegate procDelegate;
        
        public Hooks()
        {
            procDelegate = new WinEventDelegate(WinEventProc);
            hhook = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero,
                   procDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        }

        public string CursorPos
        {
            get { return System.Windows.Forms.Control.MousePosition.ToString(); }
        }

        void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, object sender, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            int capacity = GetWindowTextLength(new HandleRef(sender, hwnd)) + 1;
            StringBuilder stringBuilder = new StringBuilder(capacity);
            GetWindowText(new HandleRef(sender, hwnd), stringBuilder, stringBuilder.Capacity);
            Console.WriteLine($"Window {stringBuilder.ToString()} Moved, {CursorPos}");
        }

        public void Unhook()
        {
            UnhookWinEvent(hhook);
        }
    }
}

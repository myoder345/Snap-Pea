using System;
using System.Runtime.InteropServices;

namespace SnapPeaApp
{
    /// <summary>
    /// Wrapper for unmanaged type IntPtr deriving from SafeHandle
    /// </summary>
    public class SafeHandle : System.Runtime.InteropServices.SafeHandle
    {
        Func<IntPtr, bool> FreeHandle;

        internal SafeHandle(IntPtr intPtr, Func<IntPtr, bool> freeHandleOperation) : base(intPtr, true)
        {
            FreeHandle = freeHandleOperation;
            handle = intPtr;
        }

        bool isInvalid;
        public override bool IsInvalid => isInvalid;

        protected override bool ReleaseHandle()
        {
            FreeHandle(handle);
            handle = IntPtr.Zero;
            isInvalid = true;
            return true;
        }
    }
}

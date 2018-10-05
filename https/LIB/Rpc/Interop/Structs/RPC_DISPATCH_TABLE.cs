﻿using System;
using System.Runtime.InteropServices;

namespace Rpc.Interop.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RPC_DISPATCH_TABLE
    {
        public uint DispatchTableCount;
        public IntPtr DispatchTable;
        public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RPC_DISPATCH_TABLE_Entry
    {
        public IntPtr DispatchMethod;
        public IntPtr Zero;
    }
}
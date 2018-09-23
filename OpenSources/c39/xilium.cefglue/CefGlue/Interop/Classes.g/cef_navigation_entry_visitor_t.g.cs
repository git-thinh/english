//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    
    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal unsafe struct cef_navigation_entry_visitor_t
    {
        internal cef_base_t _base;
        internal IntPtr _visit;
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void add_ref_delegate(cef_navigation_entry_visitor_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int release_delegate(cef_navigation_entry_visitor_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int has_one_ref_delegate(cef_navigation_entry_visitor_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int visit_delegate(cef_navigation_entry_visitor_t* self, cef_navigation_entry_t* entry, int current, int index, int total);
        
        private static int _sizeof;
        
        static cef_navigation_entry_visitor_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_navigation_entry_visitor_t));
        }
        
        internal static cef_navigation_entry_visitor_t* Alloc()
        {
            var ptr = (cef_navigation_entry_visitor_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_navigation_entry_visitor_t();
            ptr->_base._size = (UIntPtr)_sizeof;
            return ptr;
        }
        
        internal static void Free(cef_navigation_entry_visitor_t* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        
    }
}

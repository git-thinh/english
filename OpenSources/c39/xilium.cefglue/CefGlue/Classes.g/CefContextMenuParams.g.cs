//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Xilium.CefGlue.Interop;
    
    // Role: PROXY
    public sealed unsafe partial class CefContextMenuParams : IDisposable
    {
        internal static CefContextMenuParams FromNative(cef_context_menu_params_t* ptr)
        {
            return new CefContextMenuParams(ptr);
        }
        
        internal static CefContextMenuParams FromNativeOrNull(cef_context_menu_params_t* ptr)
        {
            if (ptr == null) return null;
            return new CefContextMenuParams(ptr);
        }
        
        private cef_context_menu_params_t* _self;
        
        private CefContextMenuParams(cef_context_menu_params_t* ptr)
        {
            if (ptr == null) throw new ArgumentNullException("ptr");
            _self = ptr;
        }
        
        ~CefContextMenuParams()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
        }
        
        public void Dispose()
        {
            if (_self != null)
            {
                Release();
                _self = null;
            }
            GC.SuppressFinalize(this);
        }
        
        internal void AddRef()
        {
            cef_context_menu_params_t.add_ref(_self);
        }
        
        internal bool Release()
        {
            return cef_context_menu_params_t.release(_self) != 0;
        }
        
        internal bool HasOneRef
        {
            get { return cef_context_menu_params_t.has_one_ref(_self) != 0; }
        }
        
        internal cef_context_menu_params_t* ToNative()
        {
            AddRef();
            return _self;
        }
    }
}

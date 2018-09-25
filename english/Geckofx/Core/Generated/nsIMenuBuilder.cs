// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIMenuBuilder.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    /// An interface used to construct native toolbar or context menus from <menu>
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("93F4A48F-D043-4F45-97FD-9771EA1AF976")]
	public interface nsIMenuBuilder
	{
		
		/// <summary>
        /// Create the top level menu or a submenu. The implementation should create
        /// a new context for this menu, so all subsequent methods will add new items
        /// to this newly created menu.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OpenContainer([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aLabel);
		
		/// <summary>
        /// Add a new menu item. All menu item details can be obtained from
        /// the element. This method is not called for hidden elements or elements
        /// with no or empty label. The icon should be loaded only if aCanLoadIcon
        /// is true.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddItemFor([MarshalAs(UnmanagedType.Interface)] nsIDOMHTMLMenuItemElement aElement, [MarshalAs(UnmanagedType.U1)] bool aCanLoadIcon);
		
		/// <summary>
        /// Create a new separator.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddSeparator();
		
		/// <summary>
        /// Remove last added separator.
        /// Sometimes it's needed to remove last added separator, otherwise it's not
        /// possible to implement the postprocessing in one pass.
        /// See http://www.whatwg.org/specs/web-apps/current-work/multipage/interactive-elements.html#building-menus-and-toolbars
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UndoAddSeparator();
		
		/// <summary>
        /// Set the context to the parent menu.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CloseContainer();
		
		/// <summary>
        /// Returns a JSON string representing the menu hierarchy. For a context menu,
        /// it will be of the form:
        /// {
        /// type: "menu",
        /// children: [
        /// {
        /// type: "menuitem",
        /// label: "label",
        /// icon: "image.png"
        /// },
        /// {
        /// type: "separator",
        /// },
        /// ];
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ToJSONString([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Invoke the action of the menuitem with assigned id aGeneratedItemId.
        ///
        /// @param aGeneratedItemId the menuitem id
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Click([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aGeneratedItemId);
	}
}

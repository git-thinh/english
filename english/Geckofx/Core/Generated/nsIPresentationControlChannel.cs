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
// Generated by IDLImporter from file nsIPresentationControlChannel.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
    /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("ae318e05-2a4e-4f85-95c0-e8b191ad812c")]
	public interface nsIPresentationChannelDescription
	{
		
		/// <summary>
        /// Type of transport channel.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		byte GetTypeAttribute();
		
		/// <summary>
        /// Should only be used while type == TYPE_TCP.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIArray GetTcpAddressAttribute();
		
		/// <summary>
        /// Should only be used while type == TYPE_TCP.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetTcpPortAttribute();
		
		/// <summary>
        /// Should only be used while type == TYPE_DATACHANNEL.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetDataChannelSDPAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aDataChannelSDP);
	}
	
	/// <summary>nsIPresentationChannelDescriptionConsts </summary>
	public class nsIPresentationChannelDescriptionConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this file,
        // You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const ushort TYPE_TCP = 1;
		
		// 
		public const ushort TYPE_DATACHANNEL = 2;
	}
	
	/// <summary>
    /// The callbacks for events on control channel.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("96dd548f-7d0f-43c1-b1ad-28e666cf1e82")]
	public interface nsIPresentationControlChannelListener
	{
		
		/// <summary>
        /// Callback for receiving offer from remote endpoint.
        /// @param offer The received offer.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnOffer([MarshalAs(UnmanagedType.Interface)] nsIPresentationChannelDescription offer);
		
		/// <summary>
        /// Callback for receiving answer from remote endpoint.
        /// @param answer The received answer.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnAnswer([MarshalAs(UnmanagedType.Interface)] nsIPresentationChannelDescription answer);
		
		/// <summary>
        /// Callback for receiving ICE candidate from remote endpoint.
        /// @param answer The received answer.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnIceCandidate([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase candidate);
		
		/// <summary>
        /// The callback for notifying channel opened.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyOpened();
		
		/// <summary>
        /// The callback for notifying channel closed.
        /// @param reason The reason of channel close, NS_OK represents normal close.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyClosed(int reason);
	}
	
	/// <summary>
    /// The control channel for establishing RTCPeerConnection for a presentation
    /// session. SDP Offer/Answer will be exchanged through this interface.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e60e208c-a9f5-4bc6-9a3e-47f3e4ae9c57")]
	public interface nsIPresentationControlChannel
	{
		
		/// <summary>
        /// All the events should be pending until listener is assigned.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIPresentationControlChannelListener GetListenerAttribute();
		
		/// <summary>
        /// All the events should be pending until listener is assigned.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetListenerAttribute([MarshalAs(UnmanagedType.Interface)] nsIPresentationControlChannelListener aListener);
		
		/// <summary>
        /// Send offer to remote endpoint. |onOffer| should be invoked on remote
        /// endpoint.
        /// @param offer The offer to send.
        /// @throws  NS_ERROR_FAILURE on failure
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendOffer([MarshalAs(UnmanagedType.Interface)] nsIPresentationChannelDescription offer);
		
		/// <summary>
        /// Send answer to remote endpoint. |onAnswer| should be invoked on remote
        /// endpoint.
        /// @param answer The answer to send.
        /// @throws  NS_ERROR_FAILURE on failure
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendAnswer([MarshalAs(UnmanagedType.Interface)] nsIPresentationChannelDescription answer);
		
		/// <summary>
        /// Send ICE candidate to remote endpoint. |onIceCandidate| should be invoked
        /// on remote endpoint.
        /// @param candidate The candidate to send
        /// @throws NS_ERROR_FAILURE on failure
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendIceCandidate([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase candidate);
		
		/// <summary>
        /// Close the transport channel.
        /// @param reason The reason of channel close; NS_OK represents normal.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Close(int reason);
	}
}

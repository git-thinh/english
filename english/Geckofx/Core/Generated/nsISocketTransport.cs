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
// Generated by IDLImporter from file nsISocketTransport.idl
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
    /// nsISocketTransport
    ///
    /// NOTE: Connection setup is triggered by opening an input or output stream,
    /// it does not start on its own. Completion of the connection setup is
    /// indicated by a STATUS_CONNECTED_TO notification to the event sink (if set).
    ///
    /// NOTE: This is a free-threaded interface, meaning that the methods on
    /// this interface may be called from any thread.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("79221831-85e2-43a8-8152-05d77d6fde31")]
	public interface nsISocketTransport : nsITransport
	{
		
		/// <summary>
        /// Open an input stream on this transport.
        ///
        /// Flags have the following meaning:
        ///
        /// OPEN_BLOCKING
        /// If specified, then the resulting stream will have blocking stream
        /// semantics.  This means that if the stream has no data and is not
        /// closed, then reading from it will block the calling thread until
        /// at least one byte is available or until the stream is closed.
        /// If this flag is NOT specified, then the stream has non-blocking
        /// stream semantics.  This means that if the stream has no data and is
        /// not closed, then reading from it returns NS_BASE_STREAM_WOULD_BLOCK.
        /// In addition, in non-blocking mode, the stream is guaranteed to
        /// support nsIAsyncInputStream.  This interface allows the consumer of
        /// the stream to be notified when the stream can again be read.
        ///
        /// OPEN_UNBUFFERED
        /// If specified, the resulting stream may not support ReadSegments.
        /// ReadSegments is only gauranteed to be implemented when this flag is
        /// NOT specified.
        ///
        /// @param aFlags
        /// optional transport specific flags.
        /// @param aSegmentSize
        /// if OPEN_UNBUFFERED is not set, then this parameter specifies the
        /// size of each buffer segment (pass 0 to use default value).
        /// @param aSegmentCount
        /// if OPEN_UNBUFFERED is not set, then this parameter specifies the
        /// maximum number of buffer segments (pass 0 to use default value).
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIInputStream OpenInputStream(uint aFlags, uint aSegmentSize, uint aSegmentCount);
		
		/// <summary>
        /// Open an output stream on this transport.
        ///
        /// Flags have the following meaning:
        ///
        /// OPEN_BLOCKING
        /// If specified, then the resulting stream will have blocking stream
        /// semantics.  This means that if the stream is full and is not closed,
        /// then writing to it will block the calling thread until ALL of the
        /// data can be written or until the stream is closed.  If this flag is
        /// NOT specified, then the stream has non-blocking stream semantics.
        /// This means that if the stream is full and is not closed, then writing
        /// to it returns NS_BASE_STREAM_WOULD_BLOCK.  In addition, in non-
        /// blocking mode, the stream is guaranteed to support
        /// nsIAsyncOutputStream.  This interface allows the consumer of the
        /// stream to be notified when the stream can again accept more data.
        ///
        /// OPEN_UNBUFFERED
        /// If specified, the resulting stream may not support WriteSegments and
        /// WriteFrom.  WriteSegments and WriteFrom are only guaranteed to be
        /// implemented when this flag is NOT specified.
        ///
        /// @param aFlags
        /// optional transport specific flags.
        /// @param aSegmentSize
        /// if OPEN_UNBUFFERED is not set, then this parameter specifies the
        /// size of each buffer segment (pass 0 to use default value).
        /// @param aSegmentCount
        /// if OPEN_UNBUFFERED is not set, then this parameter specifies the
        /// maximum number of buffer segments (pass 0 to use default value).
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIOutputStream OpenOutputStream(uint aFlags, uint aSegmentSize, uint aSegmentCount);
		
		/// <summary>
        /// Close the transport and any open streams.
        ///
        /// @param aReason
        /// the reason for closing the stream.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Close(int aReason);
		
		/// <summary>
        /// Set the transport event sink.
        ///
        /// @param aSink
        /// receives transport layer notifications
        /// @param aEventTarget
        /// indicates the event target to which the notifications should
        /// be delivered.  if NULL, then the notifications may occur on
        /// any thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetEventSink([MarshalAs(UnmanagedType.Interface)] nsITransportEventSink aSink, [MarshalAs(UnmanagedType.Interface)] nsIEventTarget aEventTarget);
		
		/// <summary>
        /// Get the peer's host for the underlying socket connection.
        /// For Unix domain sockets, this is a pathname, or the empty string for
        /// unnamed and abstract socket addresses.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetHostAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aHost);
		
		/// <summary>
        /// Get the port for the underlying socket connection.
        /// For Unix domain sockets, this is zero.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPortAttribute();
		
		/// <summary>
        /// The platform-specific network interface id that this socket
        /// associated with. Note that this attribute can be only accessed
        /// in the socket thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNetworkInterfaceIdAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aNetworkInterfaceId);
		
		/// <summary>
        /// The platform-specific network interface id that this socket
        /// associated with. Note that this attribute can be only accessed
        /// in the socket thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNetworkInterfaceIdAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aNetworkInterfaceId);
		
		/// <summary>
        /// Returns the IP address of the socket connection peer. This
        /// attribute is defined only once a connection has been established.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetPeerAddr();
		
		/// <summary>
        /// Returns the IP address of the initiating end. This attribute
        /// is defined only once a connection has been established.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetSelfAddr();
		
		/// <summary>
        /// Bind to a specific local address.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Bind(System.IntPtr aLocalAddr);
		
		/// <summary>
        /// Returns a scriptable version of getPeerAddr. This attribute is defined
        /// only once a connection has been established.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsINetAddr GetScriptablePeerAddr();
		
		/// <summary>
        /// Returns a scriptable version of getSelfAddr. This attribute is defined
        /// only once a connection has been established.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsINetAddr GetScriptableSelfAddr();
		
		/// <summary>
        /// Security info object returned from the secure socket provider.  This
        /// object supports nsISSLSocketControl, nsITransportSecurityInfo, and
        /// possibly other interfaces.
        ///
        /// This attribute is only available once the socket is connected.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsISupports GetSecurityInfoAttribute();
		
		/// <summary>
        /// Security notification callbacks passed to the secure socket provider
        /// via nsISSLSocketControl at socket creation time.
        ///
        /// NOTE: this attribute cannot be changed once a stream has been opened.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIInterfaceRequestor GetSecurityCallbacksAttribute();
		
		/// <summary>
        /// Security notification callbacks passed to the secure socket provider
        /// via nsISSLSocketControl at socket creation time.
        ///
        /// NOTE: this attribute cannot be changed once a stream has been opened.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSecurityCallbacksAttribute([MarshalAs(UnmanagedType.Interface)] nsIInterfaceRequestor aSecurityCallbacks);
		
		/// <summary>
        /// Test if this socket transport is (still) connected.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsAlive();
		
		/// <summary>
        /// Socket timeouts in seconds.  To specify no timeout, pass UINT32_MAX
        /// as aValue to setTimeout.  The implementation may truncate timeout values
        /// to a smaller range of values (e.g., 0 to 0xFFFF).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetTimeout(uint aType);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTimeout(uint aType, uint aValue);
		
		/// <summary>
        /// connectionFlags is a bitmask that can be used to modify underlying
        /// behavior of the socket connection. See the flags below.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetConnectionFlagsAttribute();
		
		/// <summary>
        /// connectionFlags is a bitmask that can be used to modify underlying
        /// behavior of the socket connection. See the flags below.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetConnectionFlagsAttribute(uint aConnectionFlags);
		
		/// <summary>
        /// Socket QoS/ToS markings. Valid values are IPTOS_DSCP_AFxx or
        /// IPTOS_CLASS_CSx (or IPTOS_DSCP_EF, but currently no supported
        /// services require expedited-forwarding).
        /// Not setting this value will leave the socket with the default
        /// ToS value, which on most systems if IPTOS_CLASS_CS0 (formerly
        /// IPTOS_PREC_ROUTINE).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetQoSBitsAttribute();
		
		/// <summary>
        /// Socket QoS/ToS markings. Valid values are IPTOS_DSCP_AFxx or
        /// IPTOS_CLASS_CSx (or IPTOS_DSCP_EF, but currently no supported
        /// services require expedited-forwarding).
        /// Not setting this value will leave the socket with the default
        /// ToS value, which on most systems if IPTOS_CLASS_CS0 (formerly
        /// IPTOS_PREC_ROUTINE).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetQoSBitsAttribute(System.IntPtr aQoSBits);
		
		/// <summary>
        /// TCP send and receive buffer sizes. A value of 0 means OS level
        /// auto-tuning is in effect.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetRecvBufferSizeAttribute();
		
		/// <summary>
        /// TCP send and receive buffer sizes. A value of 0 means OS level
        /// auto-tuning is in effect.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetRecvBufferSizeAttribute(uint aRecvBufferSize);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetSendBufferSizeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSendBufferSizeAttribute(uint aSendBufferSize);
		
		/// <summary>
        /// TCP keepalive configuration (support varies by platform).
        /// Note that the attribute as well as the setter can only accessed
        /// in the socket thread.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetKeepaliveEnabledAttribute();
		
		/// <summary>
        /// TCP keepalive configuration (support varies by platform).
        /// Note that the attribute as well as the setter can only accessed
        /// in the socket thread.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetKeepaliveEnabledAttribute([MarshalAs(UnmanagedType.U1)] bool aKeepaliveEnabled);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetKeepaliveVals(int keepaliveIdleTime, int keepaliveRetryInterval);
	}
	
	/// <summary>nsISocketTransportConsts </summary>
	public class nsISocketTransportConsts
	{
		
		// <summary>
        // Values for the aType parameter passed to get/setTimeout.
        // </summary>
		public const ulong TIMEOUT_CONNECT = 0;
		
		// 
		public const ulong TIMEOUT_READ_WRITE = 1;
		
		// <summary>
        // nsITransportEventSink status codes.
        //
        // Although these look like XPCOM error codes and are passed in an nsresult
        // variable, they are *not* error codes.  Note that while they *do* overlap
        // with existing error codes in Necko, these status codes are confined
        // within a very limited context where no error codes may appear, so there
        // is no ambiguity.
        //
        // The values of these status codes must never change.
        //
        // The status codes appear in near-chronological order (not in numeric
        // order).  STATUS_RESOLVING may be skipped if the host does not need to be
        // resolved.  STATUS_WAITING_FOR is an optional status code, which the impl
        // of this interface may choose not to generate.
        //
        // In C++, these constants have a type of uint32_t, so C++ callers must use
        // the NS_NET_STATUS_* constants defined below, which have a type of
        // nsresult.
        // </summary>
		public const ulong STATUS_RESOLVING = 0x804b0003;
		
		// 
		public const ulong STATUS_RESOLVED = 0x804b000b;
		
		// 
		public const ulong STATUS_CONNECTING_TO = 0x804b0007;
		
		// 
		public const ulong STATUS_CONNECTED_TO = 0x804b0004;
		
		// 
		public const ulong STATUS_SENDING_TO = 0x804b0005;
		
		// 
		public const ulong STATUS_WAITING_FOR = 0x804b000a;
		
		// 
		public const ulong STATUS_RECEIVING_FROM = 0x804b0006;
		
		// <summary>
        // Values for the connectionFlags
        //
        // When making a new connection BYPASS_CACHE will force the Necko DNS
        // cache entry to be refreshed with a new call to NSPR if it is set before
        // opening the new stream.
        // </summary>
		public const ulong BYPASS_CACHE = (1<<0);
		
		// <summary>
        // When setting this flag, the socket will not apply any
        // credentials when establishing a connection. For example,
        // an SSL connection would not send any client-certificates
        // if this flag is set.
        // </summary>
		public const ulong ANONYMOUS_CONNECT = (1<<1);
		
		// <summary>
        // If set, we will skip all IPv6 addresses the host may have and only
        // connect to IPv4 ones.
        // </summary>
		public const ulong DISABLE_IPV6 = (1<<2);
		
		// <summary>
        // If set, indicates that the connection was initiated from a source
        // defined as being private in the sense of Private Browsing. Generally,
        // there should be no state shared between connections that are private
        // and those that are not; it is OK for multiple private connections
        // to share state with each other, and it is OK for multiple non-private
        // connections to share state with each other.
        // </summary>
		public const ulong NO_PERMANENT_STORAGE = (1<<3);
		
		// <summary>
        // If set, we will skip all IPv4 addresses the host may have and only
        // connect to IPv6 ones.
        // </summary>
		public const ulong DISABLE_IPV4 = (1<<4);
		
		// <summary>
        // If set, indicates that the socket should not connect if the hostname
        // resolves to an RFC1918 address or IPv6 equivalent.
        // </summary>
		public const ulong DISABLE_RFC1918 = (1<<5);
		
		// <summary>
        // This flag is an explicit opt-in that allows a normally secure socket
        // provider to use, at its discretion, an insecure algorithm. e.g.
        // a TLS socket without authentication.
        // </summary>
		public const ulong MITM_OK = (1<<6);
	}
}

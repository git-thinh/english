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
// Generated by IDLImporter from file nsIURIFixup.idl
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
    /// Interface indicating what we found/corrected when fixing up a URI
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4819f183-b532-4932-ac09-b309cd853be7")]
	public interface nsIURIFixupInfo
	{
		
		/// <summary>
        /// Consumer that asked for fixed up URI.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsISupports GetConsumerAttribute();
		
		/// <summary>
        /// Consumer that asked for fixed up URI.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetConsumerAttribute([MarshalAs(UnmanagedType.Interface)] nsISupports aConsumer);
		
		/// <summary>
        /// Our best guess as to what URI the consumer will want. Might
        /// be null if we couldn't salvage anything (for instance, because
        /// the input was invalid as a URI and FIXUP_FLAG_ALLOW_KEYWORD_LOOKUP
        /// was not passed)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetPreferredURIAttribute();
		
		/// <summary>
        /// The fixed-up original input, *never* using a keyword search.
        /// (might be null if the original input was not recoverable as
        /// a URL, e.g. "foo bar"!)
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetFixedURIAttribute();
		
		/// <summary>
        /// The name of the keyword search provider used to provide a keyword search;
        /// empty string if no keyword search was done.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetKeywordProviderNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aKeywordProviderName);
		
		/// <summary>
        /// The keyword as used for the search (post trimming etc.)
        /// empty string if no keyword search was done.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetKeywordAsSentAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aKeywordAsSent);
		
		/// <summary>
        /// Whether we changed the protocol instead of using one from the input as-is.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFixupChangedProtocolAttribute();
		
		/// <summary>
        /// Whether we created an alternative URI. We might have added a prefix and/or
        /// suffix, the contents of which are controlled by the
        /// browser.fixup.alternate.prefix and .suffix prefs, with the defaults being
        /// "www." and ".com", respectively.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetFixupCreatedAlternateURIAttribute();
		
		/// <summary>
        /// The original input
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetOriginalInputAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aOriginalInput);
	}
	
	/// <summary>
    /// Interface implemented by objects capable of fixing up strings into URIs
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1da7e9d4-620b-4949-849a-1cd6077b1b2d")]
	public interface nsIURIFixup
	{
		
		/// <summary>
        /// Converts an internal URI (e.g. a wyciwyg URI) into one which we can
        /// expose to the user, for example on the URL bar.
        ///
        /// @param  aURI       The URI to be converted
        /// @return nsIURI     The converted, exposable URI
        /// @throws NS_ERROR_MALFORMED_URI when the exposable portion of aURI is malformed
        /// @throws NS_ERROR_UNKNOWN_PROTOCOL when we can't get a protocol handler service
        /// for the URI scheme.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI CreateExposableURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI);
		
		/// <summary>
        /// Converts the specified string into a URI, first attempting
        /// to correct any errors in the syntax or other vagaries. Returns
        /// a wellformed URI or nullptr if it can't.
        ///
        /// @param aURIText    Candidate URI.
        /// @param aFixupFlags Flags that govern ways the URI may be fixed up.
        /// @param aPostData   The POST data to submit with the returned
        /// URI (see nsISearchSubmission).
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI CreateFixupURI([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aURIText, uint aFixupFlags, [MarshalAs(UnmanagedType.Interface)] ref nsIInputStream aPostData);
		
		/// <summary>
        /// Same as createFixupURI, but returns information about what it corrected
        /// (e.g. whether we could rescue the URI or "just" generated a keyword
        /// search URI instead).
        ///
        /// @param aURIText    Candidate URI.
        /// @param aFixupFlags Flags that govern ways the URI may be fixed up.
        /// @param aPostData   The POST data to submit with the returned
        /// URI (see nsISearchSubmission).
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURIFixupInfo GetFixupURIInfo([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aURIText, uint aFixupFlags, [MarshalAs(UnmanagedType.Interface)] ref nsIInputStream aPostData);
		
		/// <summary>
        /// Converts the specified keyword string into a URI.  Note that it's the
        /// caller's responsibility to check whether keywords are enabled and
        /// whether aKeyword is a sensible keyword.
        ///
        /// @param aKeyword  The keyword string to convert into a URI
        /// @param aPostData The POST data to submit to the returned URI
        /// (see nsISearchSubmission).
        ///
        /// @throws NS_ERROR_FAILURE if the resulting URI requires submission of POST
        /// data and aPostData is null.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURIFixupInfo KeywordToURI([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aKeyword, [MarshalAs(UnmanagedType.Interface)] ref nsIInputStream aPostData);
		
		/// <summary>
        /// Returns true if the specified domain is whitelisted and false otherwise.
        /// A whitelisted domain is relevant when we have a single word and can't be
        /// sure whether to treat the word as a host name or should instead be
        /// treated as a search term.
        ///
        /// @param aDomain A domain name to query.
        /// @param aDotPos The position of the first '.' character in aDomain, or
        /// -1 if no '.' character exists.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsDomainWhitelisted([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aDomain, uint aDotPos);
	}
	
	/// <summary>nsIURIFixupConsts </summary>
	public class nsIURIFixupConsts
	{
		
		// <summary>
        //No fixup flags. </summary>
		public const ulong FIXUP_FLAG_NONE = 0;
		
		// <summary>
        // Allow the fixup to use a keyword lookup service to complete the URI.
        // The fixup object implementer should honour this flag and only perform
        // any lengthy keyword (or search) operation if it is set.
        // </summary>
		public const ulong FIXUP_FLAG_ALLOW_KEYWORD_LOOKUP = 1;
		
		// <summary>
        // Tell the fixup to make an alternate URI from the input URI, for example
        // to turn foo into www.foo.com.
        // </summary>
		public const ulong FIXUP_FLAGS_MAKE_ALTERNATE_URI = 2;
		
		// <summary>
        // Fix common scheme typos.
        // </summary>
		public const ulong FIXUP_FLAG_FIX_SCHEME_TYPOS = 8;
	}
}

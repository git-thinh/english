using Gecko;
using Gecko.IO;
using Gecko.Net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Geckofx
{
    internal class MyStreamListener : nsIStreamListener
    {
        public MyStreamListener(/*...*/) { }
        public void OnStartRequest(nsIRequest aRequest, nsISupports aContext)
        {
            // This will get called once, when the download "begins".
            // You can initialize your things here.
        }

        public void OnStopRequest(nsIRequest aRequest, nsISupports aContext, int aStatusCode)
        {
            // This will also get called once, when the download is 
            // complete or interrupted. You can perform the post-download 
            // actions here.

            if (aStatusCode != GeckoError.NS_OK)
            {
                // download interrupted
            }
            else
            {
                // download completed
            }
        }

        public void OnDataAvailable(nsIRequest aRequest, nsISupports aContext, nsIInputStream aInputStream, ulong aOffset, uint aCount)
        {
            // This gets called several times with small chunks of data. 
            // Do what you need with the stream. In my case, I read it 
            // in a small buffer, which then gets written to an output 
            // filestream (not shown).
            // The aOffset parameter is the sum of all previously received data.

            var lInput = InputStream.Create(aInputStream);
            byte[] lBuffer = new byte[aCount];
            lInput.Read(lBuffer, 0, (int)aCount);

        }
    }


    public class MyExternalHelperAppService : nsIExternalHelperAppService
    {
        public MyExternalHelperAppService(/* ... */)
        {
            /* ... */
        }

        public nsIStreamListener DoContent(nsACStringBase aMimeContentType, nsIRequest aRequest, nsIInterfaceRequestor aWindowContext, bool aForceSave)
        {
            var request = Request.CreateRequest(aRequest);
            var lChannel = request as HttpChannel;
            try
            {
                if (lChannel != null)
                {
                    var uri = lChannel.OriginalUri;
                    var contentType = lChannel.ContentType;
                    var contentLength = lChannel.ContentLength;
                    var dispositionFilename = lChannel.ContentDispositionFilename;

                    // Do your contenttype validation, keeping only what you need.
                    // Make sure you clean dispositionFilename before using it.

                    // If you don't want to do anything with that file, you can return null;

                    return new MyStreamListener(/* ... */);
                }
            }
            catch (COMException)
            {
                /* ... */
            }
            return null;
        }

        [return: MarshalAs(UnmanagedType.U1)]
        public bool ApplyDecodingForExtension([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aExtension, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aEncodingType)
        {
            throw new NotImplementedException();
        }
    }

    public class MyNsFactory: nsIFactory
    {
        public IntPtr CreateInstance(nsISupports aOuter, ref Guid iid)
        {
            // This is called when the content dispatcher gets a DISPOSITION_ATTACHMENT 
            // on the channel, or when it doesn't have any builtin handler 
            // for the content type. It needs an external helper to handle 
            // the content, so it creates one and calls DoContent on it.

            MyExternalHelperAppService _myExternalHelperAppService = new MyExternalHelperAppService();
            IntPtr result;
            IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(_myExternalHelperAppService);
            Marshal.QueryInterface(iUnknownForObject, ref iid, out result);
            Marshal.Release(iUnknownForObject);
            return result;
        }

        public void LockFactory(bool @lock)
        {
            // do nothing here, it's not used, only kept for backwards compatibility.
        }
    }

}

// Using ...
// Xpcom.RegisterFactory(typeof(MyExternalHelperAppService).GUID, "MyExternalHelperAppService", "@mozilla.org/uriloader/external-helper-app-service;1", new MyNsFactory());

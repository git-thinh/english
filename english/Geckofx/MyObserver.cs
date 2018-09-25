using Gecko.Net;
using Gecko.Observers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gecko
{
    // https://bitbucket.org/geckofx/geckofx-29.0/issues/197/getting-content-on-observer-response
    /*

        MyObserver oObs = new MyObserver();
        oObs.TicketLoadedEvent += TicketLoadedEvent_Handling;
        ObserverService.AddObserver( oObs );

         */


    public class MyObserver : BaseHttpModifyRequestObserver
    {
        public delegate void TicketLoadedEventHandler(object sender, TicketLoadedEventArgs e);
        public event TicketLoadedEventHandler TicketLoadedEvent;
        protected virtual void OnTicketLoaded(TicketLoadedEventArgs e)
        {
            if (TicketLoadedEvent != null)
            {
                TicketLoadedEvent(this, e);
            }
        }

        protected override void ObserveRequest(HttpChannel p_HttpChannel)
        {
            if (p_HttpChannel != null)
            {
                //if (p_HttpChannel.Uri.AbsolutePath.Contains("/ticket.aspx"))
                //{
                TraceableChannel oTC = p_HttpChannel.CastToTraceableChannel();
                StreamListenerTee oStream = new StreamListenerTee();
                oStream.Completed += (se, ev) => { Stream_Completed(se, p_HttpChannel.Uri.ToString()); };
                oTC.SetNewListener(oStream);
                //}
            }
        }

        private void Stream_Completed(object sender, string url)
        {
            if (sender is StreamListenerTee)
            {
                StreamListenerTee oStream = sender as StreamListenerTee;
                byte[] aData = oStream.GetCapturedData();
                string sData1 = Encoding.UTF8.GetString(aData);
                string sData2 = Encoding.UTF7.GetString(aData);
                string sData3 = Encoding.ASCII.GetString(aData);
                 

                // Custom Event that returns the data
                OnTicketLoaded(new TicketLoadedEventArgs(url, sData1));
            }
        }
    }

    public class TicketLoadedEventArgs : EventArgs
    {
        public string Url { set; get; }
        public string Data { set; get; }
        public TicketLoadedEventArgs(string url, string data)
        {
            this.Url = url;
            this.Data = data;
        }
    }

    // ObserverService.AddObserver(new RequestObserver());

    public class RequestObserver : BaseHttpModifyRequestObserver
    {
        protected override void ObserveRequest(HttpChannel channel)
        {
            if (channel != null)
            {
                channel.SetResponseHeader("Access-Control-Allow-Origin", "*", true); //add allow header to response for all request
            }
        }
    }
}

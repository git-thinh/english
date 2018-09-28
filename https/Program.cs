using IpcChannel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace https
{

    class Program
    {
        static void f_getSource(string url)
        {
            try
            {
                //using (WebClient webClient = new WebClient())
                //{
                //    var stream = webClient.OpenRead(new Uri(url));
                //    using (StreamReader sr = new StreamReader(stream))
                //    {
                //        string data = url + "{&}" + sr.ReadToEnd();
                //        Console.WriteLine(data);
                //    }
                //}

                HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
                w.BeginGetResponse(asyncResult =>
                {
                    string _url = ((HttpWebRequest)asyncResult.AsyncState).RequestUri.ToString();
                    try
                    {
                        HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult);
                        if (rs.StatusCode == HttpStatusCode.OK)
                        {
                            using (StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8))
                            {
                                string data = _url + "\r\n" + sr.ReadToEnd();
                                f_sendMessageToUI(true, url, data);
                            }
                            rs.Close();
                        }
                    }
                    catch
                    {
                        f_sendMessageToUI(false, url);
                    }
                }, w);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                f_sendMessageToUI(false, url, msg);
            }
        }

        static void f_sendMessageToUI(bool isSuccess, params string[] arguments) {
            var ls = new List<string>() {isSuccess ? "OK" : "FAIL", EVENT_KEY };
            if (arguments != null && arguments.Length > 0) ls.AddRange(arguments);
            client.SendTo("UI", EVENT_NAME, ls.ToArray());
        }

        const string EVENT_KEY_PATH = @"English\Browser";
        static readonly string _channel = new Guid("{1B617C4B-BF68-4B8C-AE2B-A77E6A3ECEC5}").ToString();
        static IIpcChannelRegistrar _registrar = new IpcChannelRegistrar(Registry.CurrentUser, EVENT_KEY_PATH);
        static IpcEventChannel client = new IpcEventChannel(_registrar, _channel);
        static string EVENT_KEY = "CHANNEL_TEST";
        static string EVENT_NAME = "MSG";

        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(25, 25);
            ServicePointManager.DefaultConnectionLimit = 1000;

            /* Certificate validation callback */
            ServicePointManager.ServerCertificateValidationCallback += (object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error) =>
            {
                /* If the certificate is a valid, signed certificate, return true. */
                if (error == System.Net.Security.SslPolicyErrors.None) return true;
                //Console.WriteLine("X509Certificate [{0}] Policy Error: '{1}'", cert.Subject, error.ToString());
                return false;
            };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; //TLS 1.2
            
            ///////////////////////////////////////////////////////////////////////////////////////

            if (args.Length > 0) EVENT_KEY = args[0];
            client.StartListening(EVENT_KEY);
            client[EVENT_NAME].OnEvent += delegate (object o, IpcSignalEventArgs e)
            {
                //Console.WriteLine(string.Format("LISTENING_{0}: {1}", 1, String.Join(",", e.Arguments)));
                if (e.Arguments.Length > 0)
                    f_getSource(e.Arguments[0]);
            };

            Console.ReadKey();

            client.StopListening();
        }
    }
}

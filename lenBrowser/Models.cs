using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace lenBrowser
{
    public interface ICache
    {
        bool isExist(string key);
        string Set(string key, string data);
        string Get(string key);
        string getUrl(string key);
        string getKeyByUrl(string url);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct COPYDATASTRUCT
    {
        public int dwData;
        public int cbData;
        public int lpData;
    }

    public class oCmd
    {
        public string cmd { set; get; }
        public string url { set; get; }
    }
}

public class CertificateWebClient : WebClient
{
    //private readonly X509Certificate2 certificate;
    private readonly X509Certificate certificate;

    public CertificateWebClient(X509Certificate cert)
    {
        certificate = cert;
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
        HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate X509certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
        {
            return true;
        };

        request.ClientCertificates.Add(certificate);
        return request;
    }

    //public CertificateWebClient Create(string url) {
    //    return (new CertificateWebClient(new X509Certificate())).GetWebRequest(new Uri(url));
    //}
}
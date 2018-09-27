using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace https
{

    class Program
    {
        static void Main(string[] args)
        {
            string url;
            if (args.Length == 0) url = "https://dictionary.cambridge.org/grammar/british-grammar/above-or-over";
            else url = args[0];

            //url = "https://vnexpress.net";
            //url = "https://vuejs.org/v2/guide/";
            //url = "https://msdn.microsoft.com/en-us/library/ff361664(v=vs.110).aspx";
            //url = "https://developer.mozilla.org/en-US/docs/Web";
            //url = "https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/rest_parameters";
            //url = "https://www.myenglishpages.com/site_php_files/grammar-lesson-tenses.php";
            //url = "https://learnenglish.britishcouncil.org/en/english-grammar/pronouns";

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

            try
            {
                using (WebClient webClient = new WebClient())
                {
                    var stream = webClient.OpenRead(new Uri(url));
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string data = url + "{&}" + sr.ReadToEnd();
                        Console.WriteLine(data);
                    }
                }

                ////////HttpWebRequest w = (HttpWebRequest)WebRequest.Create(new Uri(url));
                ////////w.BeginGetResponse(asyncResult =>
                ////////{
                ////////    string _url = ((HttpWebRequest)asyncResult.AsyncState).RequestUri.ToString();
                ////////    try
                ////////    {
                ////////        HttpWebResponse rs = (HttpWebResponse)w.EndGetResponse(asyncResult);
                ////////        if (rs.StatusCode == HttpStatusCode.OK)
                ////////        {
                ////////            using (StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.UTF8))
                ////////            {
                ////////                string data = sr.ReadToEnd();
                ////////            }
                ////////            rs.Close();
                ////////        }
                ////////    }
                ////////    catch {
                ////////    }
                ////////}, w);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                Console.WriteLine(msg);
            }
            //Console.ReadKey();
        }
    }
}

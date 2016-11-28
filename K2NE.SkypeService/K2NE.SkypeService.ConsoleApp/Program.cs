using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using System.Linq;
using K2NE.SkypeService.ConsoleApp.SkypeClasses;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

namespace K2NE.SkypeService.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string _autodiscoverUrl = "https://lyncdiscover.k2.com/";
            string login = Console.ReadLine();
            string pass = Console.ReadLine();
            Uri restUrl;

            

            string oauthUrl = "";

            //Step 1 - Getting the reponse from Autodiscovery URL
            WebRequest req0 = System.Net.WebRequest.Create(_autodiscoverUrl);
            req0.ContentType = "application/json";
            req0.Method = WebRequestMethods.Http.Get;
            var resp = req0.GetResponse() as HttpWebResponse;
            var respStream = resp.GetResponseStream();
            TextReader reader = new StreamReader(respStream);
            var json = reader.ReadToEnd();
            var autodiscoveryResponse = new AutodiscoveryResponse();
            autodiscoveryResponse = JsonConvert.DeserializeObject<AutodiscoveryResponse>(json);
            restUrl = new Uri(autodiscoveryResponse._links["self"].Href);
            reader.Close();

            //Step 2 - Trying to authenticate the user
            var req1 = (HttpWebRequest)WebRequest.Create(autodiscoveryResponse._links["user"].Href);
            req1.ContentType = "application/json";
            req1.Method = WebRequestMethods.Http.Get;
            req1.Referer = autodiscoveryResponse._links["xframe"].Href;
            try
            {
                var resp1 = req1.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                //throwing exception if other from Unauthorized
                HttpWebResponse resp1 = (HttpWebResponse)ex.Response;
                if (resp1.StatusCode != HttpStatusCode.Unauthorized)
                {
                    throw ex;
                }
                //Trying to get the Oauth Url
                string[] headerArr = resp1.Headers["WWW-Authenticate"].Split(',');
                string val = Array.Find(headerArr, t => t.Contains("href"));
                Match match = Regex.Match(val, "\"([^\"]*)");
                if (match.Success)
                {
                    oauthUrl = match.Groups[1].Value;
                }
            }

            //Step 3 - Authenticating and getting Oauth Token
            var grantType = String.Format("grant_type=password&username={0}&password={1}", login, pass);
            var req2 = (HttpWebRequest)WebRequest.Create(oauthUrl);
            req2.ContentType = "application/x-www-form-urlencoded;charset='utf-8'";
            req2.Accept = "application/json";
            req2.Method = WebRequestMethods.Http.Post;
            req2.Referer = autodiscoveryResponse._links["xframe"].Href;
            byte[] buf = Encoding.UTF8.GetBytes(grantType);
            req2.ContentLength = buf.Length;
            req2.GetRequestStream().Write(buf, 0, buf.Length);

            var resp2 = req2.GetResponse() as HttpWebResponse;
            var respStream2 = resp2.GetResponseStream();
            TextReader reader2 = new StreamReader(respStream2);
            var json2 = reader2.ReadToEnd();
            var oauthToken = new OauthToken();
            oauthToken = JsonConvert.DeserializeObject<OauthToken>(json2);
            reader2.Close();

            //Step 4 - Getting the url to applications
            var req3 = (HttpWebRequest)WebRequest.Create(autodiscoveryResponse._links["user"].Href);
            req3.Accept = "application/json";
            req3.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req3.Method = WebRequestMethods.Http.Get;
            req3.Referer = autodiscoveryResponse._links["xframe"].Href;
            var resp3 = req3.GetResponse() as HttpWebResponse;
            var respStream3 = resp3.GetResponseStream();
            TextReader reader3 = new StreamReader(respStream3);
            var json3 = reader3.ReadToEnd();
            var autodiscoveryResponse3 = new AutodiscoveryResponse();
            autodiscoveryResponse3 = JsonConvert.DeserializeObject<AutodiscoveryResponse>(json3);
            reader3.Close();

            //Step 5 - Getting the url to applications
            var applicationSettings = new Dictionary<string, string>()
            {
                {"UserAgent", "UCWA Samples"},
                {"EndpointId", Guid.NewGuid().ToString()},
                {"Culture", "en-US"}
            };


            var req4Body = JsonConvert.SerializeObject(applicationSettings);

            var req4 = (HttpWebRequest)WebRequest.Create(autodiscoveryResponse3._links["applications"].Href);
            req4.Accept = "application/json";
            req4.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req4.Method = WebRequestMethods.Http.Post;
            req4.Referer = autodiscoveryResponse._links["xframe"].Href;

            byte[] buf4 = Encoding.UTF8.GetBytes(req4Body);
            req4.ContentLength = buf4.Length;
            req4.ContentType = "application/json";
            req4.GetRequestStream().Write(buf4, 0, buf4.Length);


            var resp4 = req4.GetResponse() as HttpWebResponse;
            var respStream4 = resp4.GetResponseStream();
            TextReader reader4 = new StreamReader(respStream4);
            var json4 = reader4.ReadToEnd();
            var applications = new ApplicationsResponse();
            applications = JsonConvert.DeserializeObject<ApplicationsResponse>(json4);
            reader4.Close();

            //Starting a messaging

            var req5Body = new Dictionary<string, string>()
            {
                 {"operationId", "5028e824-2268-4b14-9e59-1abad65ff393"},
                 {"to","sip:ruben@k2.com"},
                 {"subject", "Task Sample"},
                 {"sessionContext", "33dc0ef6-0570-4467-bb7e-49fcbea8e944"}
            };


            var req5BodyJson = JsonConvert.SerializeObject(req5Body);

            var req5 = (HttpWebRequest)WebRequest.Create(restUrl.Scheme + "://" + restUrl.Authority + applications._embedded.communication._links["startMessaging"].Href);
            req5.Accept = "application/json";
            req5.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req5.Method = WebRequestMethods.Http.Post;
            req5.Referer = autodiscoveryResponse._links["xframe"].Href;

            byte[] buf5 = Encoding.UTF8.GetBytes(req5BodyJson);
            req5.ContentLength = buf5.Length;
            req5.ContentType = "application/json";
            req5.GetRequestStream().Write(buf5, 0, buf5.Length);


            var resp5 = req5.GetResponse() as HttpWebResponse;
            var respStream5 = resp5.GetResponseStream();
            TextReader reader5 = new StreamReader(respStream5);
            var json5 = reader5.ReadToEnd();
            var application5 = new ApplicationsResponse();
            application5 = JsonConvert.DeserializeObject<ApplicationsResponse>(json4);
            reader5.Close();


            string _state = "Connecting";
            var eventResponse6 = new EventResponse();
            string relativeUrl = applications._links["events"].Href;

          
                var req6 = (HttpWebRequest)System.Net.WebRequest.Create(restUrl.Scheme + "://" + restUrl.Authority + relativeUrl);
                req6.Accept = "application/json";
                req6.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
                req6.Method = WebRequestMethods.Http.Get;
                var resp6 = req6.GetResponse() as HttpWebResponse;
                var respStream6 = resp6.GetResponseStream();
                TextReader reader6 = new StreamReader(respStream6);
                var json6 = reader6.ReadToEnd();
                eventResponse6 = JsonConvert.DeserializeObject<EventResponse>(json6);
                reader6.Close();
                relativeUrl = eventResponse6._links["next"].Href;
                _state = eventResponse6.Sender[0].Events[1]._Embedded.Conversation.State;

            

            //Getting Meeting status
            var req7 = (HttpWebRequest)System.Net.WebRequest.Create(restUrl.Scheme + "://" + restUrl.Authority + eventResponse6.Sender[1].Href);
            req7.Accept = "application/json";
            req7.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req7.Method = WebRequestMethods.Http.Get;
            var resp7 = req7.GetResponse() as HttpWebResponse;
            var respStream7 = resp7.GetResponseStream();
            TextReader reader7 = new StreamReader(respStream7);
            var json7 = reader7.ReadToEnd();
            var conversation7 = new Conversation();
            conversation7 = JsonConvert.DeserializeObject<Conversation>(json7);
            reader7.Close();


            //Getting Messaging data
            var req8 = (HttpWebRequest)System.Net.WebRequest.Create(restUrl.Scheme + "://" + restUrl.Authority + conversation7._links["messaging"].Href);
            req8.Accept = "application/json";
            req8.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req8.Method = WebRequestMethods.Http.Get;
            var resp8 = req8.GetResponse() as HttpWebResponse;
            var respStream8 = resp8.GetResponseStream();
            TextReader reader8 = new StreamReader(respStream8);
            var json8 = reader8.ReadToEnd();
            var mResponse8 = new MessagingResponse();
            mResponse8 = JsonConvert.DeserializeObject<MessagingResponse>(json8);
            reader8.Close();

            //Sending a message
            string req9Body = "Wazzzzz UUUUUP? You are supposed to receive this message. If yes, then I will go home:)";
            var req9 = (HttpWebRequest)WebRequest.Create(restUrl.Scheme + "://" + restUrl.Authority + mResponse8._links["sendMessage"].Href);
            req9.Accept = "application/json";
            req9.Headers["Authorization"] = oauthToken.Token_type + " " + oauthToken.Access_token;
            req9.Method = WebRequestMethods.Http.Post;
            req9.Referer = autodiscoveryResponse._links["xframe"].Href;

            byte[] buf9 = Encoding.UTF8.GetBytes(req9Body);
            req9.ContentLength = buf9.Length;
            req9.ContentType = "text/plain";
            req9.GetRequestStream().Write(buf9, 0, buf9.Length);


            var resp9 = req9.GetResponse() as HttpWebResponse;
            var respStream9 = resp9.GetResponseStream();
            TextReader reader9 = new StreamReader(respStream9);
            var json9 = reader9.ReadToEnd();
            reader9.Close();


            ////reader.Close();




            ////req.Headers["Authorization"] = "Basic " +
            ////                               Convert.ToBase64String(
            ////                                   Encoding.Default.GetBytes(login + ":" + password));
            
        }
    }
}


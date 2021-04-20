using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HackPetition
{
    class Program
    {

        static void Main()
        {
          
            Console.OutputEncoding = Encoding.UTF8;
            string CountryID = "166";
            string City = "Chisinau";
            string cid = "1508";
            string Postcode = "MD-2000";
            string petition_id = "1013685";
            string action = "sign";
            string postdata,lastname, firstname, email, fullname;
            const int requests = 1000;


            for (int i = 0; i < requests; i++)
            {
                try
                {
                    using (var str = new WebClient())
                    {
                        JObject json = JObject.Parse(str.DownloadString("https://api.namefake.com/romanian-moldova"));
                        fullname = json.GetValue("name").ToString();


                        var split_string = fullname.Split(' ');

                        if (split_string.Length > 2)
                        {
                            lastname = split_string[2];
                            firstname = split_string[1];
                        }
                        else
                        {
                            lastname = split_string[1];
                            firstname = split_string[0];
                        }


                        email = json.GetValue("email_u").ToString() + "@" + json.GetValue("email_d").ToString();
                    }


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://secure.avaaz.org/act/do.php");

                    request.Headers["X-Requested-With"] = "XMLHttpRequest";
                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.Timeout = 2000;
                    request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:19.0) Gecko/20100101 Firefox/19.0";
                    request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    request.Accept = "application/json, text/javascript, */*; q=0.01";
                    request.ServicePoint.Expect100Continue = false;
                    postdata = String.Format("First={0}&Last={1}&Email={2}&CountryID={3}&City={4}&Postcode={5}&petition_id={6}&action={7}&cid={8}",
                                             firstname, lastname, email, CountryID, City, Postcode, petition_id, action, cid);


                    byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                    request.ContentLength = byteArray.Length;


                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        Console.WriteLine(postdata);
                    }

                    var a = request.GetResponse();
                    a.Close();
                    // Stream receiveStream = a.GetResponseStream();


                    // StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                    //Console.WriteLine("Response stream received.");
                    // Console.WriteLine(readStream.ReadToEnd());

                    Console.WriteLine("Request Nr. " + i);
                    Thread.Sleep(500);
                }
                catch (Exception) { }
            }

            Console.ReadKey();
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace dcStatus
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            DataPatern status = new DataPatern();
        }

        public static string ChangeStatus(string token, fullDataPatern statusData)
        {
            var url = "https://discord.com/api/v9/users/@me/settings";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "PATCH";

            httpRequest.ContentType = "application/json";
            httpRequest.Headers["authorization"] = token;
            var data = JsonConvert.SerializeObject(statusData);

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            return httpResponse.StatusCode.ToString();
        }
        
    }

    class fullDataPatern
    {
        public List<DataPatern> custom_status;
    }
    class DataPatern
    {
        public string emoji;
        public string status;

    }
}
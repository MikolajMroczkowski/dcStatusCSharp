using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace dcStatus
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var config = new Config();
            fullStatusPatern statusy = new fullStatusPatern();
            using (var sr = new StreamReader("config.json"))
            {
                string content = sr.ReadToEnd();
                config = JsonConvert.DeserializeObject<Config>(content);
            }
            using (var sr = new StreamReader("messages.json"))
            {
                string content = sr.ReadToEnd();
                statusy = JsonConvert.DeserializeObject<fullStatusPatern>(content);
            }
            Console.WriteLine(config.token);
            while (true)
            {
                foreach (var obj in statusy.messages)
                {
                    fullDataPatern fullData = new fullDataPatern();
                    fullData.custom_status.emoji_name = obj.emoji;
                    fullData.custom_status.text = obj.message;
                    Console.WriteLine(ChangeStatus(config.token, fullData));
                    Thread.Sleep(obj.time);
                }
            }

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

            return data.ToString();
        }
        
    }

    class fullDataPatern
    {
        public DataPatern custom_status = new DataPatern();
    }
    class DataPatern
    {
        public string emoji_name;
        public string text;

    }

    class Config
    {
        public string token;
    }

    class StatusPatern
    {
        public string emoji;
        public string message;
        public int time;
    }

    class fullStatusPatern
    {
        public List<StatusPatern> messages;
    }
}
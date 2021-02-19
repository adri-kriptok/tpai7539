using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Fiuba.App7539.Helpers
{
    public static class HttpHelper
    {
        private const string urlBase = "http://www.prismasoft.com.ar/demos/Fiuba7539_2";

        public static T Post<T>(string url)
        {
            return JsonHelper.Parse<T>(Post(url));
        }

        public static string Post(string url)
        {
            var response = string.Empty;

            if (!url.ToLower().StartsWith("http"))
            {
                url = $"{urlBase}/{url}";
            }

            // var url3 = new URL(url);
            // var conn = (HttpURLConnection)url3.OpenConnection();
            // 
            // conn.RequestMethod = "POST";
            // 
            // if (conn.ResponseCode == HttpStatus.Ok)
            // {
            //     var reader = new BufferedReader(new InputStreamReader(conn.InputStream));
            //     var line = reader.ReadLine();
            // 
            //     while (line != null)
            //     {
            //         response += line;
            //         line = reader.ReadLine();
            //     }
            // 
            //     reader.Close();
            // }
            // conn.Disconnect();
            // return response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);

                var result = client.PostAsync(url, null).Result;

                result.EnsureSuccessStatusCode();

                var text = result.Content.ReadAsStringAsync().Result;

                return text;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Fiuba.App7539.Helpers;
using Java.IO;
using Java.Net;

namespace Fiuba7539.Droid.Helpers
{
    public class RestHelper
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

            var url3 = new URL(url);
            var conn = (HttpURLConnection)url3.OpenConnection();

            conn.RequestMethod = "POST";

            if (conn.ResponseCode == HttpStatus.Ok)
            {
                var reader = new BufferedReader(new InputStreamReader(conn.InputStream));
                var line = reader.ReadLine();

                while (line != null)
                {
                    response += line;
                    line = reader.ReadLine();
                }

                reader.Close();
            }
            conn.Disconnect();
            return response;
        }
    }
}
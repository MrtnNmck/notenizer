using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nsServices.WebServices
{
    public static class WebParser
    {
        public static bool UrlExists(Uri uri)
        {
            return WebParser.UrlExists(uri.ToString());
        }

        public static bool UrlExists(String url)
        {
            HttpWebRequest request;
            HttpWebResponse response;

            request = (HttpWebRequest)WebRequest.Create("http://www.example.com");
            request.Method = WebRequestMethods.Http.Head;
            response = (HttpWebResponse)request.GetResponse();

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}

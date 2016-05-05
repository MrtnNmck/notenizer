using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace nsServices.WebServices
{
    /// <summary>
    /// Parser for web pages.
    /// </summary>
    public static class WebParser
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checks if URL exists.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static bool UrlExists(Uri uri)
        {
            return WebParser.UrlExists(uri.ToString());
        }

        /// <summary>
        /// Checks if URL exists.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlExists(String url)
        {
            HttpWebRequest request;
            HttpWebResponse response;

            request = (HttpWebRequest)WebRequest.Create("http://www.example.com");
            request.Method = WebRequestMethods.Http.Head;
            response = (HttpWebResponse)request.GetResponse();

            return response.StatusCode == HttpStatusCode.OK;
        }

        #endregion Methods
    }
}

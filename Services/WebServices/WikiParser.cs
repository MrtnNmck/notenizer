using HtmlAgilityPack;
using nsConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nsServices.WebServices
{
    /// <summary>
    /// Parser for wikipadia's articles.
    /// </summary>
    public static class WikiParser
    {
        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// Parses country from wikipedia.
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static String ParseCountry(String countryName)
        {
            return WikiParser.Parse(WikiParser.CreateUrl(countryName).ToString());
        }

        /// <summary>
        /// Parser article from URL.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static String Parse(String url)
        {
            String source;
            byte[] response;
            HtmlNode content;
            HttpClient client;
            HtmlDocument document;
            List<String> sentences;

            sentences = new List<String>();
            client = new HttpClient();
            response = client.GetByteArrayAsync(url).Result;
            source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            document = new HtmlDocument();
            document.LoadHtml(source);

            content = document.DocumentNode.Descendants().Where(
                x => x.Attributes["id"] != null && x.Attributes["id"].Value.Equals("mw-content-text")).FirstOrDefault();

            if (content == null)
                return "Unable to parse the source.";

            foreach (HtmlNode childNoteLoop in content.ChildNodes)
            {
                if (childNoteLoop.Name == "div" && childNoteLoop.Id == "toc")
                    break;

                if (childNoteLoop.Name == "p")
                    sentences.Add(childNoteLoop.InnerText.Trim());
            }

            return String.Join(NotenizerConstants.WordDelimeter, sentences);
        }

        /// <summary>
        /// Creates URL from country.
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        private static Uri CreateUrl(String countryName)
        {
            return new Uri(new Uri(NotenizerConstants.SimpleWikiUrl), countryName.Replace(" ", "_"));
        }

        #endregion Methods
    }
}

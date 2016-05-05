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
    public static class WikiParser
    {
        #region Properties

        #endregion Properties

        #region Methods

        public static String ParseCountry(String countryName)
        {
            return WikiParser.Parse(WikiParser.CreateUrl(countryName).ToString());
        }

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

        private static Uri CreateUrl(String countryName)
        {
            return new Uri(new Uri(NotenizerConstants.SimpleWikiUrl), countryName.Replace(" ", "_"));
        }

        #endregion Methods
    }
}

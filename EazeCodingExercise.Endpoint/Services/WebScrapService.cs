using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace EazeCodingExercise.Endpoint.Services
{
    internal class WebScrapService : IWebScrapService
    {
        //private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public async Task<string> GetHtmlAsync(Uri uri, string xPath)
        {
            using (var myClient = new WebClient())
            {
                string content;

                Stream response = await myClient.OpenReadTaskAsync(uri);

                using (var reader = new StreamReader(response))
                {
                    content = await reader.ReadToEndAsync();
                }

                var document = new HtmlDocument();
                document.LoadHtml(content);

                var innerHtmls = document.DocumentNode.SelectNodes(xPath).Select(node => node.InnerHtml);
                return string.Concat(innerHtmls);
            }
        }
    }
}

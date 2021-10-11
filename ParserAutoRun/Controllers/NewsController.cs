using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using ParserAutoRun.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParserAutoRun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly string _url;
        private readonly HttpClient _client;
        private static readonly int newsPerPage = 10;

        public NewsController()
        {
            _client = new HttpClient();
            _url = $"https://autorun.by/novosti/soligorsk/";
        }

        [HttpGet("news")]
        public async Task<List<Article>> GetHtmlRequest(int offset = 0, int count = 5)
        {
            var page = CalculatePage(offset);
            var document = await GetDocument(page);
            var articles = new List<Article>();

            foreach (var node in document.QuerySelectorAll("div.elem-66 div.news-item"))
            {
                var item = new Article
                {
                    Href = "https://autorun.by/" + node.QuerySelector("div.item a.thumb").GetAttribute("href"),
                    Image = "https://autorun.by/" + node.QuerySelector("div.item a.thumb img").GetAttribute("src"),
                    Title = node.QuerySelector("div.item a.thumb img").GetAttribute("alt")
                };
                articles.Add(item);
            }
            return articles.GetRange(offset % newsPerPage, count);
        }

        private static int CalculatePage(int offset)
        {
            return offset / newsPerPage + 1;
        }

        private async Task<IHtmlDocument> GetDocument(int page)
        {
            var response = await _client.GetAsync($"{_url}?page={page}");
            if (!response.IsSuccessStatusCode)
                return null;
            var source = await response.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            return await parser.ParseDocumentAsync(source);

        }
    }
}
using AngleSharp;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParserAutoRun.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace ParserAutoRun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly string _url;
        private readonly HttpClient _client;
        private readonly List<Article> _articles;

        public NewsController()
        {
            _client = new HttpClient();
            _url = $"https://autorun.by/novosti/soligorsk/";
            _articles = new List<Article>();
        }

        [HttpGet("/getNews")]
        public async Task<List<Article>> GetHtmlRequest(int page)
        {
            _articles.Clear();
            var document = await GetDocument();

            foreach (var node in document.QuerySelectorAll("div.elem-66 div.news-item"))
            {
                var item = new Article
                {
                    Href = "https://autorun.by/" + node.QuerySelector("div.item a.thumb").GetAttribute("href"),
                    Image = "https://autorun.by/" + node.QuerySelector("div.item a.thumb img").GetAttribute("src"),
                    Title = node.QuerySelector("div.item a.thumb img").GetAttribute("alt")
                };
                _articles.Add(item);
            }
            return _articles;
        }

        [HttpGet("GetLastPage")]
        public async Task<int> GetLastPage()
        {
            var document = await GetDocument();
            var linkToTheLastPage = document.QuerySelector("div.elem-66 div#pdopage ul.pagination")
                .LastElementChild.InnerHtml;
            return Convert.ToInt32(Regex.Match(linkToTheLastPage, @"page=(\d+)").Groups[1].Value);
        }

        private async Task<IHtmlDocument> GetDocument()
        {
            var response = await _client.GetAsync($"{_url}");
            if (!response.IsSuccessStatusCode)           
                return null;           
            var source = await response.Content.ReadAsStringAsync();
            var parser = new HtmlParser();
            return await parser.ParseDocumentAsync(source);

        }
    }
}
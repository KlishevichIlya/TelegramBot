using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.UnitOfWork;

namespace TelegramBot.BLL.Services
{
    public class ParserService : IParser
    {
        private readonly string _url;
        private readonly HttpClient _client;
        private static readonly int newsPerPage = 10;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ParserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _client = new HttpClient();
            _url = $"https://autorun.by/novosti/soligorsk/";
            _mapper = mapper;
        }

        public async IAsyncEnumerable<NewsDTO> MakeHtmlRequest(int offset = 0, int count = 0)
        {
            var page = CalculatePage(offset);
            var document = await GetDocument(page);
            var listOfArticles = new List<NewsDTO>();

            var news = document.QuerySelectorAll("div.elem-66 div.news-item").ToList();
            
            foreach (var node in news.GetRange(offset % newsPerPage, count))
            {
                var item = new NewsDTO
                {
                    Href = "https://autorun.by/" + node.QuerySelector("div.item a.thumb").GetAttribute("href"),
                    Image = "https://autorun.by/" + node.QuerySelector("div.item a.thumb img").GetAttribute("src"),
                    Title = node.QuerySelector("div.item a.thumb img").GetAttribute("alt"),
                    DateOfCreating = DateTime.Parse(node.QuerySelector("div.item div.title ul li span").TextContent,
                            new CultureInfo("ru-Ru"), DateTimeStyles.NoCurrentDateDefault)
                };               
                listOfArticles.Add(item);
                yield return item;
            }
            await SaveArticlesAsync(listOfArticles);

           // return listOfArticles.GetRange(offset % newsPerPage, count);
        }

        public async Task<IEnumerable<NewsDTO>> MakeRequestWithoutSaving()
        {
            var page = CalculatePage(0);
            var document = await GetDocument(page);
            var listOfArticles = new List<NewsDTO>();

            foreach (var node in document.QuerySelectorAll("div.elem-66 div.news-item"))
            {
                var item = new NewsDTO
                {
                    Href = "https://autorun.by/" + node.QuerySelector("div.item a.thumb").GetAttribute("href"),
                    Image = "https://autorun.by/" + node.QuerySelector("div.item a.thumb img").GetAttribute("src"),
                    Title = node.QuerySelector("div.item a.thumb img").GetAttribute("alt")
                };
                listOfArticles.Add(item);
            }
            return listOfArticles.GetRange(0, 5);
        }

        public async Task SaveArticlesAsync(IEnumerable<NewsDTO> articlesDTO)
        {
            var articles = _mapper.Map<IEnumerable<NewsDTO>, IEnumerable<News>>(articlesDTO);
            var articlesFromDb = await _unitOfWork.News.GetAllAsync();

            if (!articlesFromDb.Any())
            {                
                await _unitOfWork.News.AddRangeAsync(articles);
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                var filteredArticles = articles.Where(a => !articlesFromDb.Any(db => a.Title == db.Title)).ToList();                
                await _unitOfWork.News.AddRangeAsync(filteredArticles);
                await _unitOfWork.CompleteAsync();
            }
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

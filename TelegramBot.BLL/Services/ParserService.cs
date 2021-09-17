using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using AutoMapper;
using System.Collections.Generic;
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

        public async Task<IEnumerable<NewsDTO>> MakeHtmlRequest(int offset = 0, int count = 0)
        {
            var page = CalculatePage(offset);
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
            SaveArticles(listOfArticles);
            
            return listOfArticles.GetRange(offset % newsPerPage, count);
        }

        private void SaveArticles(IEnumerable<NewsDTO> articlesDTO)
        {            
            var articles = _mapper.Map<IEnumerable<NewsDTO>, IEnumerable<News>>(articlesDTO);
            _unitOfWork.News.Add(articles.First());
            _unitOfWork.Complete();
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

using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.UnitOfWork;

namespace TelegramBot.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork _db;
        public ArticleService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NewsDTO>> GetLasFiveNewsAsync()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>());
            var mapper = new Mapper(config);
            return mapper.Map<IEnumerable<News>, IEnumerable<NewsDTO>>(await _db.News.GetLastFiveNewsAsync());
        }
    }
}

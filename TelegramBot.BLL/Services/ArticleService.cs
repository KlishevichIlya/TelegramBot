using AutoMapper;
using System.Collections.Generic;
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

        public IEnumerable<NewsDTO> GetLasFiveNews()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<News, NewsDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<News>, IEnumerable<NewsDTO>>(_db.News.GetLastFiveNews());
        }
    }
}

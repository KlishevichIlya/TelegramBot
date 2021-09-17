using AutoMapper;
using TelegramBot.BLL.DTO;
using TelegramBot.DAL.Entities;

namespace TelegramBot.BLL.Mapping
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<NewsDTO, News>()
                .ForMember(x => x.Id, f => f.MapFrom(f => f.Id))
                .ForMember(x => x.Image, f => f.MapFrom(f => f.Image))
                .ForMember(x => x.Title, f => f.MapFrom(f => f.Title));
                
        }
    }
}

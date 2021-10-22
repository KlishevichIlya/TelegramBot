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
                .ForMember(x => x.Image, f => f.MapFrom(u => u.Image))
                .ForMember(x => x.Title, f => f.MapFrom(u => u.Title));

            CreateMap<UserDTO, User>()
                //.ForMember(x => x.Id, f => f.MapFrom(u => u.Id))
                .ForMember(x => x.UserId, f => f.MapFrom(u => u.UserId))
                .ForMember(x => x.UserName, f => f.MapFrom(u => u.UserName));
        }
    }
}

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
                .ForMember(x => x.Title, f => f.MapFrom(u => u.Title))
                .ForMember(x => x.Date, f => f.MapFrom(u => u.DateOfCreating));

            CreateMap<UserDTO, User>()
                .ForMember(x => x.UserId, f => f.MapFrom(u => u.UserId))
                .ForMember(x => x.UserName, f => f.MapFrom(u => u.UserName))
                .ForMember(x => x.DateOfStartSubscription, f => f.MapFrom(u => u.DateOfStartSubscription))
                .ForMember(x => x.isUnsubscribe, f => f.MapFrom(u => u.isUnsubscribe))
                .ReverseMap();
            CreateMap<EditorDTO, Editor>()
                .ForMember(e => e.Email, f => f.MapFrom(e => e.Email))
                .ForMember(e => e.Password, f => f.MapFrom(e => e.Password))
                .ForMember(e => e.isAdminRule, f => f.MapFrom(e => e.isAdminRule))
                .ReverseMap();
        }
    }
}

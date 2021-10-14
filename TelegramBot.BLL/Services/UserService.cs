
using System;
using System.Threading.Tasks;
using AutoMapper;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.UnitOfWork;

namespace TelegramBot.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;

        public UserService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task StartSubscribeAsync(UserDTO userDTO)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>());
            var mapper = new Mapper(config);
            var user = mapper.Map<UserDTO, User>(userDTO);
            user.DateOfStartSubscription = DateTime.Now;
            await _db.Users.AddAsync(user);
            await _db.CompleteAsync();
        }

        public async Task StopSubscribeAsync(UserDTO userDTO)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>());
            var mapper = new Mapper(config);
            var user = mapper.Map<UserDTO, User>(userDTO);
            _db.Users.Remove(user);
            await _db.CompleteAsync();
        }
    }
}

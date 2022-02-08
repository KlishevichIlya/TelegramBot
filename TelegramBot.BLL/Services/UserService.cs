using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
           return await _db.Users.GetAllAsync();
        }

        public async Task StartSubscribeAsync(UserDTO userDTO)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, User>());
            var mapper = new Mapper(config);
            var user = mapper.Map<UserDTO, User>(userDTO);
            user.DateOfStartSubscription = DateTime.Now;
            var checkUserInDb = await _db.Users.GetByIdAsync(userDTO.UserId);
            if (checkUserInDb is not null)
            {
                checkUserInDb.isUnsubscribe = false;
            }
            else
            {
                user.isUnsubscribe = false;
                await _db.Users.AddAsync(user);
            }

            await _db.CompleteAsync();
            await Task.CompletedTask;
        }

        public async Task StopSubscribeAsync(UserDTO userDTO)
        {
            var userFromDb = await _db.Users.GetByIdAsync(userDTO.UserId);
            if (userFromDb is not null)
            {
                userFromDb.isUnsubscribe = true;
            }
            await _db.CompleteAsync();
        }
    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;
using TelegramBot.BLL.Mapping;
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

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
            var mapper = new Mapper(config);
            var users = await _db.Users.GetAllAsync();
            var usersDTO = mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
            return usersDTO;
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

        public async Task<IEnumerable<UserDTO>> UpdateUserAsync(UserDTO updatedUser)
        {
            var cfg = new MapperConfiguration(cfg => cfg.AddProfile(new CommonMappingProfile()));
            var mapper = new Mapper(cfg);
            var user = mapper.Map<UserDTO, User>(updatedUser);
            var newUsers = await _db.Users.UpdateUser(user);
            var userDTO = mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(newUsers);
            return userDTO;
        }
               
    }
}

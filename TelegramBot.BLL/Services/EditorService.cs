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
    public class EditorService: IEditorService
    {
        private readonly IUnitOfWork _context;
        public EditorService(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EditorDTO>> SignUpAsync(EditorDTO editor)
        {
            var cfg = new MapperConfiguration(cfg => cfg.AddProfile(new CommonMappingProfile()));
            var mapper = new Mapper(cfg);
            var newEditor = mapper.Map<Editor>(editor);
            newEditor.Password = BCrypt.Net.BCrypt.HashPassword(newEditor.Password);
            var checkEditor = await _context.Editors.GetEditorByEmailAsync(newEditor.Email);
            if (checkEditor == null)
            {
                newEditor.Password = BCrypt.Net.BCrypt.HashPassword(newEditor.Password);
                await _context.Editors.AddAsync(newEditor);
            }
            else
            {
                throw new ArgumentException($"User with email {newEditor.Email} already exsist");
            }
            await _context.CompleteAsync();
            var listEditors = await _context.Editors.GetAllAsync();
            var listEditorsDTO = mapper.Map<IEnumerable<EditorDTO>>(listEditors);
            return listEditorsDTO;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TelegramBot.DAL.Data;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.EditorRepository
{
    public class EditorRepository: GenericRepository<Editor>, IEditorRepository
    {
        public EditorRepository(ApplicationContext context) : base(context)
        { }

        public async Task<Editor> GetEditorByEmailAsync(string email)
        {            
            var targetUser = await _context.Editors.SingleOrDefaultAsync(x => x.Email == email);      
            return targetUser != null ? targetUser : null;
        }
    }
}

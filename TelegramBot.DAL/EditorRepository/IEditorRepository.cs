using System.Threading.Tasks;
using TelegramBot.DAL.Entities;
using TelegramBot.DAL.GenericRepository;

namespace TelegramBot.DAL.EditorRepository
{
    public interface IEditorRepository: IGenericRepository<Editor>
    {
        Task<Editor> GetEditorByEmailAsync(string email);
    }
}

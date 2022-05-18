using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;

namespace TelegramBot.BLL.Interfaces
{
    public interface IEditorService
    {
        Task<IEnumerable<EditorDTO>> SignUpAsync(EditorDTO editor);
    }
}

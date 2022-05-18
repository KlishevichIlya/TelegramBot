using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelegramBot.BLL.DTO;
using TelegramBot.BLL.Interfaces;

namespace TelegramBot.Service.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IEditorService _repo;
        public AuthController(IEditorService repo)
        {
            _repo = repo;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUpEditor(EditorDTO editor)
        {
            IEnumerable<EditorDTO> actualEditors;
            try
            {
                actualEditors = await _repo.SignUpAsync(editor);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(actualEditors);
        }  
    }
}

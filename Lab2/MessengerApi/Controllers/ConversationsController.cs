using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using MessengerApi.Models;

namespace MessengerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationsController : ControllerBase
    {
        private readonly IDbConnection _db;

        public ConversationsController(IDbConnection db)
        {
            _db = db;
        }

        // POST: api/conversations
        [HttpPost]
        public IActionResult CreateConversation([FromBody] Conversation conversation)
        {
            if (string.IsNullOrEmpty(conversation.Id) || string.IsNullOrEmpty(conversation.Type))
            {
                return BadRequest("Поля Id та Type є обов'язковими.");
            }

            string sql = "INSERT INTO conversations (id, type) VALUES (@Id, @Type)";
            
            try
            {
                _db.Execute(sql, conversation);
                return StatusCode(201, new { message = "Чат успішно створено!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}
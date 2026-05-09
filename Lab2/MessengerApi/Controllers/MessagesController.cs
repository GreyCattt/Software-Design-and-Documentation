using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using MessengerApi.Models;

namespace MessengerApi.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDbConnection _db;

        public MessagesController(IDbConnection db)
        {
            _db = db;
        }

        // POST: api/messages
        [HttpPost("api/messages")]
        public IActionResult SendMessage([FromBody] Message message)
        {
            // Вимога лаби: базова обробка помилок
            if (string.IsNullOrEmpty(message.Id) || string.IsNullOrEmpty(message.SenderId) || string.IsNullOrEmpty(message.Ciphertext))
            {
                return BadRequest("Поля Id, SenderId та Ciphertext не можуть бути порожніми.");
            }

            // Встановлюємо статус та час генерації на сервері
            message.Status = "Sent";
            message.CreatedAt = DateTime.UtcNow;

            string sql = @"INSERT INTO messages (id, conversation_id, sender_id, ciphertext, status, created_at) 
                           VALUES (@Id, @ConversationId, @SenderId, @Ciphertext, @Status, @CreatedAt)";
            try
            {
                _db.Execute(sql, message);
                return StatusCode(201, new { message = "Повідомлення збережено (відправлено)!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка бази даних: {ex.Message}");
            }
        }

        // GET: api/conversations/{conversationId}/messages
        [HttpGet("api/conversations/{conversationId}/messages")]
        public IActionResult GetMessages(string conversationId)
        {
            // Використовуємо 'AS' у запиті, щоб Dapper правильно співставив snake_case з баз з PascalCase моделі C#
            string sql = @"
                SELECT id AS Id, 
                       conversation_id AS ConversationId, 
                       sender_id AS SenderId, 
                       ciphertext AS Ciphertext, 
                       status AS Status, 
                       created_at AS CreatedAt 
                FROM messages 
                WHERE conversation_id = @ConvId 
                ORDER BY created_at ASC";

            var messages = _db.Query<Message>(sql, new { ConvId = conversationId });
            return Ok(messages);
        }
    }
}
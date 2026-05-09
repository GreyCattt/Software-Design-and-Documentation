using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using MessengerApi.Models;

namespace MessengerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IDbConnection _db;

        public UsersController(IDbConnection db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (string.IsNullOrEmpty(user.Id) || string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("Id та Name є обов'язковими.");
            }

            string sql = "INSERT INTO users (id, name, public_key) VALUES (@Id, @Name, @PublicKey)";
            
            try
            {
                _db.Execute(sql, user);
                return StatusCode(201, new { message = "Користувача успішно створено!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
        // GET: api/users/{id}/public-key
        [HttpGet("{id}/public-key")]
        public IActionResult GetPublicKey(string id)
        {
            string sql = "SELECT public_key FROM users WHERE id = @Id";
            
            // QuerySingleOrDefault повертає один запис або null, якщо юзера немає
            var publicKey = _db.QuerySingleOrDefault<string>(sql, new { Id = id });

            if (publicKey == null)
            {
                return NotFound(new { error = "Користувача не знайдено." });
            }

            return Ok(new { PublicKey = publicKey });
        }
    }
}
namespace MessengerApi.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Ciphertext { get; set; }
        
        // Знак питання означає, що поле може бути null при отриманні з Postman
        public string? Status { get; set; } 
        public DateTime? CreatedAt { get; set; } 
    }
}
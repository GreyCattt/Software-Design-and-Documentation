namespace MessengerApi.Models
{
    public class Conversation
    {
        public string Id { get; set; }
        public string Type { get; set; } // "direct" або "group"
    }
}
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using MessengerApi.Models;

namespace MessengerApi.Tests
{
    public class MessengerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MessengerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task TestFullMessageFlow()
        {
            string testUserId = Guid.NewGuid().ToString();
            string testChatId = Guid.NewGuid().ToString();
            string testMsgId = Guid.NewGuid().ToString();

            var user = new { Id = testUserId, Name = "TestUser", PublicKey = "key123" };
            var response1 = await _client.PostAsJsonAsync("/api/users", user);
            response1.EnsureSuccessStatusCode();

            var chat = new { Id = testChatId, Type = "direct" };
            var response2 = await _client.PostAsJsonAsync("/api/conversations", chat);
            response2.EnsureSuccessStatusCode();

            var msg = new { Id = testMsgId, ConversationId = testChatId, SenderId = testUserId, Ciphertext = "secret_encrypted_data" };
            var response3 = await _client.PostAsJsonAsync("/api/messages", msg);
            response3.EnsureSuccessStatusCode();

            var response4 = await _client.GetAsync($"/api/conversations/{testChatId}/messages");
            response4.EnsureSuccessStatusCode();

            var messages = await response4.Content.ReadFromJsonAsync<List<Message>>();
            
            Assert.NotNull(messages);
            Assert.Single(messages); 
            Assert.Equal("secret_encrypted_data", messages[0].Ciphertext);
        }
    }
}
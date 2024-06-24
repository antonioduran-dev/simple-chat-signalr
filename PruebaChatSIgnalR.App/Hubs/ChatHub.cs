using DB.Data;
using DB.Models;
using Microsoft.AspNetCore.SignalR;

namespace PruebaChatSIgnalR.App.Hubs
{
    // inherit from Hub
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        // inject DBContext in constructor.
        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        // create method to send a message by an user
        public async Task SendMessage(string username, string message)
        {
            // search the user if exists.
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            // if user doesnt exist, create a new user.
            if (user == null)
            {
                user = new User { Username = username };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // create a new message with the user an content send.
            var newMessage = new Message
            {
                Content = message,
                Timestamp = DateTime.UtcNow,
                UserId = user.Id
            };

            // save the message in DB.
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", username, message, newMessage.Timestamp);
        }
    }
}

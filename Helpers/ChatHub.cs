using FreshmanCSForum.API.Data.Interfaces;
using FreshmanCSForum.API.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FreshmanCSForum.API.Helpers
{
  public class ChatHub : Hub
  {

    private readonly ICodeLabsService _codeLabsService;

    public ChatHub(ICodeLabsService codeLabsService)
    {
      _codeLabsService = codeLabsService;
    }
    public async Task SendMessage(User user, string message, string codeLabId)
    {
      await Clients.Group(codeLabId).SendAsync("ReceiveMessage", user.Username, message);
      await _codeLabsService.AddMessageToChat(codeLabId, new ChatMessage
      {
        Message = message,
        SenderId = user.Id,
        SenderUsername = user.Username
      });
    }

    public async Task JoinGroup(string codeLabId)
    {
      await Groups.AddToGroupAsync(Context.ConnectionId, codeLabId);

      await Clients.Group(codeLabId).SendAsync("ReceiveMessage", "someone", $"{Context.ConnectionId} has joined the group {codeLabId}.");
    }

  }
}
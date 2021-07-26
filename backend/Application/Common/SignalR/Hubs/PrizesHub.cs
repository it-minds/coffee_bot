using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Common.SignalR.Hub
{
  [Authorize]
  public class PrizeHub : Hub<IPrizeHubService>
  {
    private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

    public override Task OnConnectedAsync()
    {
      string name = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      _connections.Add(name, Context.ConnectionId);
      return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
      string name = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      _connections.Remove(name, Context.ConnectionId);
      return base.OnDisconnectedAsync(exception);
    }

    public static IEnumerable<string> GetConnectedSlackUserIds { get => _connections.GetKeys();}
    public static IEnumerable<string> GetUserConnectionIds(string key) => _connections.GetConnections(key);
  }
}

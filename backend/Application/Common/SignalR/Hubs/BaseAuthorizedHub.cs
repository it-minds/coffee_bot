using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Common.SignalR.Hub
{
  [Authorize]
  public abstract class BaseAuthorizedHub<T> : Hub<T> where T : class
  {
    private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

    public override Task OnConnectedAsync()
    {
      string identifier = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      _connections.Add(identifier, Context.ConnectionId);
      return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
      string identifier = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
      _connections.Remove(identifier, Context.ConnectionId);
      return base.OnDisconnectedAsync(exception);
    }

    public static IEnumerable<string> GetConnectedIdentifiers { get => _connections.GetKeys();}
    public static IEnumerable<string> GetConnectedClientIds(string identifier) => _connections.GetConnections(identifier);
    public static IEnumerable<string> GetConnectedClientIds(IEnumerable<string> identifiers) =>
      identifiers.SelectMany(identifier => _connections.GetConnections(identifier));
  }
}

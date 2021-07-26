using System.Collections.Generic;
using Application.Common;

namespace Application.User.Common
{
  public class UserDTO : AuthUser
  {
    public IEnumerable<int> ChannelsToAdmin { get; set; }
  }
}

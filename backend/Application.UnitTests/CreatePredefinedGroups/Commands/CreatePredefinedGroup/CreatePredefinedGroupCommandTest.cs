using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CreatePredefinedGroups.Commands.CreatePredefinedGroup;
using Xunit;

namespace Application.UnitTests.CreatePredefinedGroups.Commands.CreatePredefinedGroup
{
  public class CreatePredefinedGroupCommandTest : CommandTestBase
  {

    [Fact]
    public async Task Handle_Create()
    {
      var cmd = new CreatePredefinedGroupCommand {
        ChannelId = 1,
        MemberIds = new int[]{ 1, 2, 3}
      };

      var handler = new CreatePredefinedGroupCommand.CreatePredefinedGroupCommandHandler(Context);

      var result = await handler.Handle(cmd, CancellationToken.None);

      Console.WriteLine("result $1", result);
    }

    [Fact]
    public async Task Handle_Create_2()
    {
      var cmd = new CreatePredefinedGroupCommand {
        ChannelId = 1,
        MemberIds = new int[]{ 1, 2, 3}
      };

      var handler = new CreatePredefinedGroupCommand.CreatePredefinedGroupCommandHandler(Context);

      var result1 = await handler.Handle(cmd, CancellationToken.None);
      var result2 = await handler.Handle(cmd, CancellationToken.None);

      Console.WriteLine("result $1", result2);
    }
  }
}

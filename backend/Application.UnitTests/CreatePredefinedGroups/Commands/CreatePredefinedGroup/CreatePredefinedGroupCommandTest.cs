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

      Assert.Equal(1, result);
    }

    [Fact]
    public async Task Handle_CreatePartial()
    {
      var cmd = new CreatePredefinedGroupCommand {
        ChannelId = 1,
        MemberIds = new int[]{ 1, 2}
      };

      var handler = new CreatePredefinedGroupCommand.CreatePredefinedGroupCommandHandler(Context);

      var result = await handler.Handle(cmd, CancellationToken.None);

      Assert.Equal(1, result);
    }

    [Fact]
    public async Task Handle_CreateNotExistingMemeberl()
    {
      var cmd = new CreatePredefinedGroupCommand {
        ChannelId = 1,
        MemberIds = new int[]{ 1, 2, 3, 999 }
      };

      var handler = new CreatePredefinedGroupCommand.CreatePredefinedGroupCommandHandler(Context);

      await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
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

      await Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(cmd, CancellationToken.None));
    }
  }
}

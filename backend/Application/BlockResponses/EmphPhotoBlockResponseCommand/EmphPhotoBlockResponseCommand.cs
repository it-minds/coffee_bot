using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Slack.DTO;
using Slack.Interfaces;
using SlackNet.Blocks;

namespace Application.BlockResponses
{
  public class EmphPhotoBlockResponseCommand : IRequest<BlockResponse>
  {
    public string Payload { get; set; }

    protected SlashInput PayloadParsed { get => JsonConvert.DeserializeObject<SlashInput>(Payload); }

    protected string Value { get => PayloadParsed.Actions[0].Value; }
    protected string ChannelId { get => PayloadParsed.Channel.Id; }
    protected string UserId { get => PayloadParsed.User.Id; }

    public class EmphPhotoBlockResponseCommandHandler : IRequestHandler<EmphPhotoBlockResponseCommand, BlockResponse>
    {
      private readonly IApplicationDbContext applicationDbContext;
      private readonly ISlackClient slackClient;

      public EmphPhotoBlockResponseCommandHandler(IApplicationDbContext applicationDbContext, ISlackClient slackClient)
      {
        this.applicationDbContext = applicationDbContext;
        this.slackClient = slackClient;
      }

      public async Task<BlockResponse> Handle(EmphPhotoBlockResponseCommand request, CancellationToken cancellationToken)
      {
        var group = await applicationDbContext.CoffeeRoundGroups
          .Include(x => x.CoffeeRound)
          .Include(x => x.CoffeeRoundGroupMembers)
          .Where(x => x.CoffeeRound.SlackChannelId == request.ChannelId && x.CoffeeRoundGroupMembers.Any(
            y => y.SlackMemberId == request.UserId
          ))
          .FirstOrDefaultAsync();

        if (group != null) {
          if (request.Value == "Yes")
          {
            group.HasMet = true;
          } else if (request.Value == "No")
          {
            group.PhotoUrl = "";
          }
        }

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return new BlockResponse
        {
          Replace = true,
          Delete = false,
          Type = "ephemeral",
          Text = "Thank you!"
        };
      }
    }
  }
}

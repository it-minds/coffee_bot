using System.Threading;
using System.Threading.Tasks;
using Domain.Defaults;
using MediatR;

namespace Application.ChannelSetting.Queries.GetRequiredMessageTags
{
    public class GetRequiredMessageTags : IRequest<RequiredTagsDto>
    {
    public class GetRequiredMessageTagsHandler : IRequestHandler<GetRequiredMessageTags, RequiredTagsDto>
    {
      public async Task<RequiredTagsDto> Handle(GetRequiredMessageTags request, CancellationToken cancellationToken)
      {
        var requiredTags = new RequiredTagsDto {
          StartChannelMessageRequiredTags = ChannelMessageDefaults.StartChannelMessageRequiredTags,
          StartGroupMessageRequiredTags = ChannelMessageDefaults.StartGroupMessageRequiredTags,
          MidwayMessageRequiredTags = ChannelMessageDefaults.MidwayMessageRequiredTags,
          FinisherMessageRequiredTags = ChannelMessageDefaults.FinisherMessageRequiredTags,
          TagToPredicate = ChannelMessageDefaults.TagToPredicate
        };

        return requiredTags;
      }
    }
  }
}

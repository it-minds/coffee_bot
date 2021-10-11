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
      public Task<RequiredTagsDto> Handle(GetRequiredMessageTags request, CancellationToken cancellationToken)
      {
        var requiredTags = new RequiredTagsDto();
        requiredTags.StartChannelMessageRequiredTags = ChannelMessageDefaults.StartChannelMessageRequiredTags;
        requiredTags.StartGroupMessageRequiredTags = ChannelMessageDefaults.StartGroupMessageRequiredTags;
        requiredTags.MidwayMessageRequiredTags = ChannelMessageDefaults.MidwayMessageRequiredTags;
        requiredTags.FinisherMessageRequiredTags = ChannelMessageDefaults.FinisherMessageRequiredTags;
        requiredTags.TagToPredicate = ChannelMessageDefaults.TagToPredicate;

        return new Task<RequiredTagsDto>(() => requiredTags);
      }
    }
  }
}

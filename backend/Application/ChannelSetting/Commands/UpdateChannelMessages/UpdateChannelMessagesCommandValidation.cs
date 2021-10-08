using Domain.Defaults;
using FluentValidation;

namespace Application.ChannelSetting.Commands.UpdateChannelMessages
{
  public class UpdateChannelMessagesCommandValidation : AbstractValidator<UpdateChannelMessagesCommand>
  {
    public UpdateChannelMessagesCommandValidation()
    {
      foreach (string predicate in ChannelMessageDefaults.StartChannelMessageRequiredTags)
      {
        RuleFor(e => e.Settings.RoundStartChannelMessage)
          .Matches(predicate);
      }
      RuleFor(e => e.Settings.RoundStartChannelMessage)
        .NotNull();

      foreach (string predicate in ChannelMessageDefaults.StartGroupMessageRequiredTags)
      {
        RuleFor(e => e.Settings.RoundStartGroupMessage)
          .Matches(predicate);
      }
      RuleFor(e => e.Settings.RoundStartGroupMessage)
        .NotNull();

      foreach (string predicate in ChannelMessageDefaults.MidwayMessageRequiredTags)
      {
        RuleFor(e => e.Settings.RoundMidwayMessage)
          .Matches(predicate);
      }
      RuleFor(e => e.Settings.RoundMidwayMessage)
        .NotNull();

      foreach (string predicate in ChannelMessageDefaults.FinisherMessageRequiredTags)
      {
        RuleFor(e => e.Settings.RoundFinisherMessage)
          .Matches(predicate);
      }
      RuleFor(e => e.Settings.RoundFinisherMessage)
        .NotNull();
    }
  }
}

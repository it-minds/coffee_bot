using FluentValidation;

namespace Application.ChannelSetting.Commands.UpdateChannelMessages
{
  public class UpdateChannelMessagesCommandValidation : AbstractValidator<UpdateChannelMessagesCommand>
  {
    public UpdateChannelMessagesCommandValidation()
    {
      RuleFor(e => e.Settings.RoundStartChannelMessage)
        .Matches(@"{{\s*[rR]ound[sS]tart[tT]ime\s*}}")
        .Matches(@"{{\s*[rR]ound[eE]nd[tT]ime\s*}}")
        .Matches(@"{{\s*[gG]roups\s*}}")
        .NotNull();

      RuleFor(e => e.Settings.RoundStartGroupMessage)
        .Matches(@"{{\s*[rR]ound[sS]tart[tT]ime\s*}}")
        .Matches(@"{{\s*[rR]ound[eE]nd[tT]ime\s*}}")
        .NotNull();

      RuleFor(e => e.Settings.RoundMidwayMessage)
        .Matches(@"{{\s*[yY]es[bB]utton\s*}}")
        .Matches(@"{{\s*[nN]o[bB]utton\s*}}")
        .NotNull();

      RuleFor(e => e.Settings.RoundFinisherMessage)
        .NotNull()
        .NotEmpty();
    }
  }
}

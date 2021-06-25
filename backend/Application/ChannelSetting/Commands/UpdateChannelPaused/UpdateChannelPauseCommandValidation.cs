using FluentValidation;

namespace Application.ChannelSetting.Commands.UpdateChannelPaused
{
  public class UpdateChannelPauseCommandValidation : AbstractValidator<UpdateChannelPauseCommand>
  {
    public UpdateChannelPauseCommandValidation()
    {
      RuleFor(e => e.Input)
        .NotNull();
    }
  }
}

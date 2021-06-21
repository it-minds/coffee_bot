using FluentValidation;

namespace Application.ChannelSetting.Commands.UpdateChannelSettings
{
  public class UpdateChannelSettingsCommandValidation : AbstractValidator<UpdateChannelSettingsCommand>
  {
    public UpdateChannelSettingsCommandValidation()
    {
      RuleFor(e => e.Id)
        .NotNull();
      RuleFor(e => e.Settings)
        .NotNull();
    }
  }
}

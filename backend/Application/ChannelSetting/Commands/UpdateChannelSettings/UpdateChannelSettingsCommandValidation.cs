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
      RuleFor(e => e.Settings.InitializeRoundHour)
        .InclusiveBetween(0, 23);
      RuleFor(e => e.Settings.MidwayRoundHour)
        .InclusiveBetween(0, 23);
      RuleFor(e => e.Settings.FinalizeRoundHour)
        .InclusiveBetween(0, 23);
    }
  }
}

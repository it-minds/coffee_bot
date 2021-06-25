using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace Application.ChannelSetting.Commands.UpdateChannelSettings
{
  public class UpdateChannelSettingsCommand : IRequest
  {
    [JsonIgnore]
    public int Id { get; set; }
    public ChannelSettingsDto Settings { get; set; }

    public class UpdateChannelSettingsCommandHandler : IRequestHandler<UpdateChannelSettingsCommand>
    {
      private readonly IApplicationDbContext _context;
      private readonly ICurrentUserService _currentUserService;

      public UpdateChannelSettingsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
      {
        _context = context;
        _currentUserService = currentUserService;
      }

      public async Task<Unit> Handle(UpdateChannelSettingsCommand request, CancellationToken cancellationToken)
      {
        var localSettings = _context.ChannelSettings.Find(request.Id);
        if (localSettings == null) throw new NotFoundException(nameof(ChannelSettings), request.Id);

        localSettings.GroupSize = request.Settings.GroupSize;
        localSettings.StartsDay = request.Settings.StartsDay;
        localSettings.WeekRepeat = request.Settings.WeekRepeat;
        localSettings.DurationInDays = request.Settings.DurationInDays;
        localSettings.IndividualMessage = request.Settings.IndividualMessage;

        _context.ChannelSettings.Update(localSettings);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
      }
    }
  }
}

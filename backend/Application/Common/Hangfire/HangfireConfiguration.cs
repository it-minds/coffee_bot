using Application.ChannelSync.Commands;
using Application.Common.Hangfire.MediatR;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.SqlServer;
using Rounds.Commands.RoundFinisherCommand;
using Rounds.Commands.RoundInitiatorCommand;
using Rounds.Commands.RoundMidwayCheckupCommand;
using Application.User.Commands.CheckParticipationStatus;
using Application.ChannelSync.Commands.ChannelMemberPointsSync;

namespace Application
{
  public static class HangfireConfiguration
  {
    public static IServiceCollection AddHangfire(this IServiceCollection services, string connString)
    {
      services.AddHangfire(configuration =>
      {
        configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
        configuration.UseSimpleAssemblyNameTypeSerializer();
        configuration.UseRecommendedSerializerSettings();

        configuration.UseSqlServerStorage(connString, new SqlServerStorageOptions
        {
          UseRecommendedIsolationLevel = true,
          DisableGlobalLocks = true,
        });

        configuration.UseMediatR();
      });

      services.AddHangfireServer();

      return services;
    }

    public static IMediator SetupHangfireJobs(this IMediator mediator, bool isDev)
    {
      mediator.RecurringJob(
        new SyncronizeChannelsCommand { },
        "Syncronize Channels",
        isDev ? Cron.Never() : Cron.Hourly(minute: 30)
      );

      mediator.RecurringJob(
        new RoundFinisherCommand { },
        "Round Finisher",
        isDev ? Cron.Never() : "0 12-17 * * *"
      );

      mediator.RecurringJob(
        new RoundInitiatorCommand { },
        "Round Initiator",
        isDev ? Cron.Never() : "0 8-12 * * *"
      );

      mediator.RecurringJob(
        new RoundMidwayCheckupCommand { },
        "Round Midway",
        isDev ? Cron.Never() : "0 11 * * *"
      );

      mediator.RecurringJob(
        new CheckParticipationStatusCommand { },
        "Check Repaticipation",
        isDev ? Cron.Never() : Cron.Daily(0)
      );

      mediator.RecurringJob(
        new ChannelMemberPointsSyncCommand { },
        "Check Member Points",
        isDev ? Cron.Never() : Cron.Daily(0)
      );



      return mediator;
    }
  }
}

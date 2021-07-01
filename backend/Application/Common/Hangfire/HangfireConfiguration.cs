using Application.ChannelSync.Commands;
using Application.Common.Hangfire.MediatR;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.SqlServer;
using Rounds.Commands.RoundFinisherCommand;
using Rounds.Commands.RoundInitiatorCommand;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

    public static IMediator SetupHangfireJobs(this IMediator mediator, IWebHostEnvironment env)
    {
      mediator.RecurringJob(
        new SyncronizeChannelsCommand { },
        "Syncronize Channels",
        Cron.Hourly()
      );

      mediator.RecurringJob(
        new RoundFinisherCommand { },
        "Round Finisher",
        env.IsDevelopment() ? Cron.Never() : "0 12-17 * * *"
      );

      mediator.RecurringJob(
        new RoundInitiatorCommand { },
        "Round Initiator",
        env.IsDevelopment() ? Cron.Never() : "0 8-12 * * *"
      );

      return mediator;
    }
  }
}

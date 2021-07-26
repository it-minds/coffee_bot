using Application.Common;
using Application.Common.Behaviours;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Slack.Clients;
using Slack.Interfaces;
using SlackNet;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Application
{
  [ExcludeFromCodeCoverage]
  public static class DependencyInjection
  {
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
      services.AddAutoMapper(Assembly.GetExecutingAssembly());
      services.AddMediatR(Assembly.GetExecutingAssembly());
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

      services.AddScoped<DownloadImage>();
      services.AddScoped<DeleteSlackMessage>();
      services.AddScoped<ChannelUserPoints>();
      services.AddScoped<WordStrings>();

      services.AddScoped<ISlackClient, BotClient>();
      services.AddScoped<ISlackOAuthClient, OAuthClient>();

      return services;
    }
  }
}

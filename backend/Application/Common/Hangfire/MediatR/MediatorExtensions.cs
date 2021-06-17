using MediatR;
using Hangfire;
using System;

namespace Application.Common.Hangfire.MediatR
{
  public static class MediatorExtensions
  {
    public static void Enqueue(this IMediator mediator, IBaseRequest request)
    {
      var client = new BackgroundJobClient();
      client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    public static void RecurringJob(this IMediator mediator, IBaseRequest request, string name, Func<string> cronExpression)
    {
      var client = new RecurringJobManager();
      client.AddOrUpdate<MediatorHangfireBridge>(name, bridge => bridge.Send(name, request), cronExpression);
    }

    public static void RecurringJob(this IMediator mediator, IBaseRequest request, string name, string cronExpression)
    {
      var client = new RecurringJobManager();
      client.AddOrUpdate<MediatorHangfireBridge>(name, bridge => bridge.Send(name, request), cronExpression);
    }
  }
}

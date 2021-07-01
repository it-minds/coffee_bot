using Hangfire.Dashboard;

namespace Application.Common.Hangfire
{
  public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
  {
    public bool Authorize(DashboardContext context)
    {
      return true;
    }
  }
}

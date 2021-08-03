namespace Application.Common.Interfaces
{
  public interface ICurrentUserService
  {
    string UserEmail { get; }
    string UserSlackId { get; }
    string SlackToken { get; }
  }
}

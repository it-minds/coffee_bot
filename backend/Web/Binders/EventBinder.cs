using System.Threading.Tasks;
using SlackNet.Events;

namespace Web.Binders
{

  public class EventCallback<T> : EventCallback where T : Event
  {
    public new T Event { get; set; }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChannelSetting
{
  public class ChannelSettingsIdDto : ChannelSettingsDto
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; }
    public string SlackChannelName { get; set; }
    public bool Paused { get; set; }
  }
}

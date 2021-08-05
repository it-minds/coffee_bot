using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Prizes.Common
{
  public class PrizeDTO : IAutoMap<Prize> {

    public int PointCost { get; set; }
    public bool IsMilestone { get; set; }
    public bool IsRepeatable { get; set; }
    public int ChannelSettingsId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageName { get; set; }

  }
}

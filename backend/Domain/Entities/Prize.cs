using System.Collections.Generic;

namespace Domain.Entities
{
  public class Prize {

    public int Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public int PointCost { get; set; }

    // public bool AnnounceOnClaim { get; set; }

    /// <summary>
    /// This prize can be claimed after the total amount of points for a member has been reached.
    /// <br/>
    /// Does *not* consume points.
    /// </summary>
    public bool IsMilestone { get; set; }
    /// <summary>
    ///  This prize can be claimed multiple times.
    /// <br/>
    ///  Does consume points.
    /// </summary>
    public bool IsRepeatable { get; set; }

    public bool IsValid { get => IsMilestone ^ IsRepeatable; } // Exclusive OR


    public int ChannelSettingsId { get; set; }
    public ChannelSettings ChannelSettings { get; set; }

    public ICollection<ClaimedPrize> ClaimedPrizes { get; set; }
  }
}

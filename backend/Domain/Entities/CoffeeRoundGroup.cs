using System.Collections.Generic;

namespace Domain.Entities
{
  public class CoffeeRoundGroup
  {
    public int Id { get; set; }

    public string SlackMessageId { get; set; }

    public bool HasMet { get; set; }

    public string PhotoUrl { get; set; }

    public int CoffeeRoundId { get; set; }
    public CoffeeRound CoffeeRound { get; set; }

    public ICollection<CoffeeRoundGroupMember> CoffeeRoundGroupMembers { get; set; }
  }
}

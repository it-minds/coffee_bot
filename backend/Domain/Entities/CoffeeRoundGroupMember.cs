using System.Collections.Generic;

namespace Domain.Entities
{
  public class CoffeeRoundGroupMember
  {
    public int Id { get; set; }
    public string SlackMemberId { get; set; }
    public int CoffeeRoundGroupId { get; set; }
    public CoffeeRoundGroup CoffeeRoundGroup { get; set; }
  }
}

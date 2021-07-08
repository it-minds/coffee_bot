using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Stats.Query.GetMemberStats
{
  public class StatsDto
  {
    public string SlackMemberId { get; set; }
    public decimal MeepupPercent { get; set; }
    public decimal PhotoPercent { get; set; }
    public int TotalParticipation { get; set; }


  }
}

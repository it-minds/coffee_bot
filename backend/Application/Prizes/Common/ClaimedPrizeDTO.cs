using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Prizes.Common
{
  public class ClaimedPrizeDTO : IAutoMap<ClaimedPrize>
  {
    public int Id {get; set;}
    public System.DateTimeOffset DateClaimed {get; set;}
    public int PointCost {get; set;}
    public int PrizeId {get; set;}
    public string PrizeTitle {get; set;}
    public bool WasMilestone {get; set;}
    public bool WasRepeatable {get; set;}


    public void Mapping(Profile profile)
    {
      profile.CreateMap<ClaimedPrize, ClaimedPrizeDTO>()
        .ForMember(x => x.PrizeTitle, opts => opts.MapFrom(y => y.Prize.Title))
        .ForMember(x => x.WasMilestone, opts => opts.MapFrom(y => y.Prize.IsMilestone))
        .ForMember(x => x.WasRepeatable, opts => opts.MapFrom(y => y.Prize.IsRepeatable))
      ;
    }
  }
}

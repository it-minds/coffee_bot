using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Prizes.Common
{
  public class PrizeIdDTO : PrizeDTO, IAutoMap<Prize> {

    public int Id { get; set; }

    public void Mapping(Profile profile) {
      profile.CreateMap<Prize, PrizeIdDTO>()
        .IncludeBase<Prize, PrizeDTO>();
    }
  }
}

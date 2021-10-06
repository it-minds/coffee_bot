#nullable enable
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Common.Interfaces
{
  public interface IBuildMessageService
  {
    string BuildMessage(string text, CoffeeRound roundInfo, IEnumerable<IEnumerable<string>>? groups = null);
  }
}
#nullable disable

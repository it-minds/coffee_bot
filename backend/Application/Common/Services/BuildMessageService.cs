#nullable enable
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.Common.Interfaces;
using Domain.Entities;
using System.Linq;
using Application.Common.Linq;

namespace Infrastructure.Services
{
  public class BuildMessageService : IBuildMessageService
  {
    public string BuildMessage(string text, CoffeeRound roundInfo, IEnumerable<IEnumerable<string>>? groups = null)
    {
      var meetupPercent = roundInfo.CoffeeRoundGroups.Percent(x => x.HasMet);
      text = Regex.Replace(text, @"{{\s*[rR]ound[sS]tart[tT]ime\s*}}", roundInfo.StartDate.ToString("dddd, dd/MMMM"));
      text = Regex.Replace(text, @"{{\s*[rR]ound[eE]nd[tT]ime\s*}}", roundInfo.EndDate.ToString("dddd, dd/MMMM"));
      text = Regex.Replace(text, @"{{\s*[mM]eetup[pP]ercentage\s*}}", meetupPercent.ToString());
      text = Regex.Replace(text, @"{{\s*[mM]eetup[cC]ondition\s*}}", meetupPercent < 100m ? "Next time, let's try for 100% shall we?" : "");

      if (Regex.Matches(text, @"{{\s*[gG]roups\s*}}").Count > 0 && groups != null) {
        var sb = new StringBuilder();
        for (int i = 0; i < groups.Count(); i++)
        {
          var group = groups.ToList()[i];

          sb.Append("Group "+(i+1))
            .Append(": ")
            .AppendJoin(", ", group.Select(x => "<@" + x + ">" ))
            .AppendLine("");
        }
        text = Regex.Replace(text, @"{{\s*[gG]roups\s*}}", sb.ToString());
      }

      return text;
    }
  }
}
#nullable disable

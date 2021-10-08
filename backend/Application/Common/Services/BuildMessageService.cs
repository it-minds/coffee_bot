#nullable enable
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Application.Common.Interfaces;
using Domain.Entities;
using System.Linq;
using Application.Common.Linq;
using Domain.Defaults;

namespace Infrastructure.Services
{
  public class BuildMessageService : IBuildMessageService
  {
    private static IList<string> predicates = ChannelMessageDefaults.TagToPredicate.Values.ToList();
    public string BuildMessage(string text, CoffeeRound roundInfo, IEnumerable<IEnumerable<string>>? groups = null)
    {
      var meetupPercent = roundInfo.CoffeeRoundGroups.Percent(x => x.HasMet);
      text = Regex.Replace(text, predicates[0], roundInfo.StartDate.ToString("dddd, dd/MMMM"));
      text = Regex.Replace(text, predicates[1], roundInfo.EndDate.ToString("dddd, dd/MMMM"));
      text = Regex.Replace(text, predicates[2], meetupPercent.ToString());
      text = Regex.Replace(text, predicates[3], meetupPercent < 100m ? "Next time, let's try for 100% shall we?" : "");

      if (Regex.Matches(text, predicates[4]).Count > 0 && groups != null) {
        var sb = new StringBuilder();
        for (int i = 0; i < groups.Count(); i++)
        {
          var group = groups.ToList()[i];

          sb.Append("Group "+(i+1))
            .Append(": ")
            .AppendJoin(", ", group.Select(x => "<@" + x + ">" ))
            .AppendLine("");
        }
        text = Regex.Replace(text, predicates[4], sb.ToString());
      }

      return text;
    }
  }
}
#nullable disable

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Domain.Defaults
{
  public static class ChannelMessageDefaults
  {
    public static string RoundStartChannelMessage { get; } =  "Time to drink coffee <!channel>\n" +
                                                            "The round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}." +
                                                            "The groups are:" +
                                                            "{{ Groups }}";
    public static readonly List<string> StartChannelMessageRequiredTags = new List<string>() {
      AllTags[0],
      AllTags[1],
      AllTags[4]
    };
    public static string RoundStartGroupMessage { get; } =  "Time for your coffee!\n" +
                                                          "The round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\n" +
                                                          "Have fun!";
    public static readonly List<string> StartGroupMessageRequiredTags = new List<string>() {
      AllTags[0],
      AllTags[1]
    };
    public static string RoundMidwayMessage { get; } =  "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n" +
                                                      "{{ YesButton: 'Yes, We have met!' }}" +
                                                      "{{ NoButton: 'No, we haven't met yet' }}";
    public static readonly List<string> MidwayMessageRequiredTags = new List<string>() {
      AllTags[5],
      AllTags[6]
    };
    public static string RoundFinisherMessage { get; } =  "Curtain call ladies and gentlefolk. <!channel>.\n" +
                                                        "Your success has been measured and I give you a solid 10! (For effort.) Your points have been given.\n"+
                                                        "The total meetup rate of the round was: {{ MeetupPercentage }}%\n"+
                                                        "{{ MeetupCondition: Next time, let's try for 100% shall we? }}\n"+
                                                        "Information regarding your next round TBA. Have a wonderful day :heart:";
    public static readonly List<string> FinisherMessageRequiredTags = new List<string>() {
    };
    public static List<string> AllTags { get; } = new List<string>() {
        "{{ RoundStartTime }}",
        "{{ RoundEndTime }}",
        "{{ MeetupPercentage }}",
        "{{ MeetupCondition }}",
        "{{ Groups }}",
        "{{ YesButton }}",
        "{{ NoButton }}"
    };
    public static ReadOnlyDictionary<string, string> TagToPredicate { get; } = new ReadOnlyDictionary<string, string>(
      (IDictionary<string, string>)(new Dictionary<string, string>()
      {
        { AllTags[0], @"{{\s*[rR]ound[sS]tart[tT]ime\s*}}" },
        { AllTags[1], @"{{\s*[rR]ound[eE]nd[tT]ime\s*}}" },
        { AllTags[2], @"{{\s*[mM]eetup[pP]ercentage\s*}}" },
        { AllTags[3], @"{{\s*[mM]eetup[cC]ondition\s*}}" },
        { AllTags[4], @"{{\s*[gG]roups\s*}}" },
        { AllTags[5], @"{{\s*[yY]es[bB]utton\s*}}" },
        { AllTags[6], @"{{\s*[nN]o[bB]utton\s*}}" }
      }));
  }
}

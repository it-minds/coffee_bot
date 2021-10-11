using System.Collections.Generic;

namespace Domain.Defaults
{
  public static class ChannelMessageDefaults
  {
    static ChannelMessageDefaults() {
      AllTags = new List<string>() {
          "{{ RoundStartTime }}",
          "{{ RoundEndTime }}",
          "{{ MeetupPercentage }}",
          "{{ MeetupCondition }}",
          "{{ Groups }}",
          "{{ YesButton }}",
          "{{ NoButton }}"
      };
      TagToPredicate = new Dictionary<string, string>() {
          { AllTags[0], @"{{\s*[rR]ound[sS]tart[tT]ime\s*}}" },
          { AllTags[1], @"{{\s*[rR]ound[eE]nd[tT]ime\s*}}" },
          { AllTags[2], @"{{\s*[mM]eetup[pP]ercentage\s*}}" },
          { AllTags[3], @"{{\s*[mM]eetup[cC]ondition\s*}}" },
          { AllTags[4], @"{{\s*[gG]roups\s*}}" },
          { AllTags[5], @"{{\s*[yY]es[bB]utton\s*}}" },
          { AllTags[6], @"{{\s*[nN]o[bB]utton\s*}}" }
      };
      StartChannelMessageRequiredTags = new List<string>() {
        AllTags[0],
        AllTags[1],
        AllTags[4]
      };
      StartGroupMessageRequiredTags = new List<string>() {
        AllTags[0],
        AllTags[1]
      };
      MidwayMessageRequiredTags = new List<string>() {
        AllTags[5],
        AllTags[6]
      };
      FinisherMessageRequiredTags = new List<string>() {
      };
    }
    public static List<string> AllTags { get; }
    public static Dictionary<string, string> TagToPredicate { get; }
    public static string RoundStartChannelMessage { get; } =  "Time to drink coffee <!channel>\n" +
                                                            "The round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}." +
                                                            "The groups are:" +
                                                            "{{ Groups }}";
    public static List<string> StartChannelMessageRequiredTags { get; }
    public static string RoundStartGroupMessage { get; } =  "Time for your coffee!\n" +
                                                          "The round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\n" +
                                                          "Have fun!";
    public static List<string> StartGroupMessageRequiredTags { get; }
    public static string RoundMidwayMessage { get; } =  "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n" +
                                                      "{{ YesButton }}" +
                                                      "{{ NoButton }}";
    public static List<string> MidwayMessageRequiredTags { get; }
    public static string RoundFinisherMessage { get; } =  "Curtain call ladies and gentlefolk. <!channel>.\n" +
                                                        "Your success has been measured and I give you a solid 10! (For effort.) Your points have been given.\n"+
                                                        "The total meetup rate of the round was: {{ MeetupPercentage }}%\n"+
                                                        "{{ MeetupCondition }}\n"+
                                                        "Information regarding your next round TBA. Have a wonderful day :heart:";
    public static List<string> FinisherMessageRequiredTags { get; }
  }
}

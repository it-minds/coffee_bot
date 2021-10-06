using System;
using System.Collections.Generic;

namespace Domain.Entities
{
  public class ChannelSettings
  {
    public int Id { get; set; }
    public string SlackChannelId { get; set; } // SlackChannelId
    public string SlackChannelName { get; set; }
    public string SlackWorkSpaceId { get; set; }

    public int GroupSize { get; set; } = 3;
    public DayOfWeek StartsDay { get; set; } = DayOfWeek.Monday;
    public int WeekRepeat { get; set; } = 2;
    public int DurationInDays { get; set; } = 11;
    public bool IndividualMessage { get; set; } = false;

    public string RoundStartChannelMessage { get; set; } =  "Time to drink coffee <!channel>\n" +
                                                            "The round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}." +
                                                            "The groups are:" +
                                                            "{{ Groups }}";
    public string RoundStartGroupMessage { get; set; } =  "Time for your coffee!\n" +
                                                          "The round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\n" +
                                                          "Have fun!";
    public string RoundMidwayMessage { get; set; } =  "Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n" +
                                                      "{{ YesButton: 'Yes, We have met!' }}" +
                                                      "{{ NoButton: 'No, we haven't met yet' }}";
    public string RoundFinisherMessage { get; set; } =  "Curtain call ladies and gentlefolk. <!channel>.\n" +
                                                        "Your success has been measured and I give you a solid 10! (For effort.) Your points have been given.\n"+
                                                        "The total meetup rate of the round was: {{ MeetupPercentage }}%\n"+
                                                        "{{ MeetupCondition: Next time, let's try for 100% shall we? }}\n"+
                                                        "Information regarding your next round TBA. Have a wonderful day :heart:";


    public ICollection<ChannelMember> ChannelMembers { get; set; }
    public ICollection<CoffeeRound> CoffeeRounds { get; set; }
    public ICollection<Prize> Prizes { get; set; }
    public ICollection<ChannelNotice> ChannelNotices { get; set; }
  }
}

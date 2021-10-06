using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
  public class ChannelSettingsConfig : IEntityTypeConfiguration<ChannelSettings>
  {
    public void Configure(EntityTypeBuilder<ChannelSettings> builder)
    {
      builder.Property(e => e.RoundStartChannelMessage)
        .IsRequired()
        .HasDefaultValue("Time to drink coffee <!channel>\n" +
                          "The round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.\n" +
                          "The groups are:\n" +
                          "{{ Groups }}");

      builder.Property(e => e.RoundStartGroupMessage)
        .IsRequired()
        .HasDefaultValue("Time for your coffee!\n" +
                          "The round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\n" +
                          "Have fun!");

      builder.Property(e => e.RoundMidwayMessage)
        .IsRequired()
        .HasDefaultValue("Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n" +
                         "{{ YesButton }}" +
                         "{{ NoButton }}");

      builder.Property(e => e.RoundFinisherMessage)
        .IsRequired()
        .HasDefaultValue("Curtain call ladies and gentlefolk. <!channel>.\n" +
                         "Your success has been measured and I give you a solid 10! (For effort.) Your points have been given.\n" +
                         "The total meetup rate of the round was: {{ MeetupPercentage }}%\n" +
                         "{{ MeetupCondition }}\n" +
                         "Information regarding your next round TBA. Have a wonderful day :heart:");
    }
  }
}

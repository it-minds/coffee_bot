﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.ChannelMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelSettingsId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRemoved")
                        .HasColumnType("bit");

                    b.Property<bool>("OnPause")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("ReturnFromPauseDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("SlackName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlackUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelSettingsId");

                    b.ToTable("ChannelMembers");
                });

            modelBuilder.Entity("Domain.Entities.ChannelNotice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelSettingsId")
                        .HasColumnType("int");

                    b.Property<int>("DaysInRound")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoticeType")
                        .HasColumnType("int");

                    b.Property<bool>("Personal")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ChannelSettingsId");

                    b.ToTable("ChannelNotices");
                });

            modelBuilder.Entity("Domain.Entities.ChannelSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DurationInDays")
                        .HasColumnType("int");

                    b.Property<int>("FinalizeRoundHour")
                        .HasColumnType("int");

                    b.Property<int>("GroupSize")
                        .HasColumnType("int");

                    b.Property<bool>("IndividualMessage")
                        .HasColumnType("bit");

                    b.Property<string>("RoundFinisherMessage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Curtain call ladies and gentlefolk. <!channel>.\nYour success has been measured and I give you a solid 10! (For effort.) Your points have been given.\nThe total meetup rate of the round was: {{ MeetupPercentage }}%\n{{ MeetupCondition }}\nInformation regarding your next round TBA. Have a wonderful day :heart:");

                    b.Property<string>("RoundMidwayMessage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Hello, guys! I am cheking in to see if you met for a cup of coffee this round.\n{{ YesButton }}{{ NoButton }}");

                    b.Property<string>("RoundStartChannelMessage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Time to drink coffee <!channel>\nThe round starts: {{ RoundStartTime }}. The round ends {{ RoundEndTime }}.\nThe groups are:\n{{ Groups }}");

                    b.Property<string>("RoundStartGroupMessage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Time for your coffee!\nThe round starts: {{ RoundStartTime }}. The round ends: {{ RoundEndTime }}.\nHave fun!");
                    b.Property<int>("InitializeRoundHour")
                        .HasColumnType("int");

                    b.Property<int>("MidwayRoundHour")
                        .HasColumnType("int");

                    b.Property<string>("SlackChannelId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlackChannelName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlackWorkSpaceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StartsDay")
                        .HasColumnType("int");

                    b.Property<int>("WeekRepeat")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ChannelSettings");
                });

            modelBuilder.Entity("Domain.Entities.ClaimedPrize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelMemberId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("DateClaimed")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDelivered")
                        .HasColumnType("bit");

                    b.Property<int>("PointCost")
                        .HasColumnType("int");

                    b.Property<int>("PrizeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChannelMemberId");

                    b.HasIndex("PrizeId");

                    b.ToTable("ClaimedPrizes");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRound", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("EndDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("CoffeeRounds");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRoundGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoffeeRoundId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("FinishedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("HasMet")
                        .HasColumnType("bit");

                    b.Property<bool>("HasPhoto")
                        .HasColumnType("bit");

                    b.Property<string>("LocalPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NotificationCount")
                        .HasColumnType("int");

                    b.Property<string>("SlackMessageId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SlackPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoffeeRoundId");

                    b.ToTable("CoffeeRoundGroups");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRoundGroupMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoffeeRoundGroupId")
                        .HasColumnType("int");

                    b.Property<bool>("Participated")
                        .HasColumnType("bit");

                    b.Property<string>("SlackMemberId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoffeeRoundGroupId");

                    b.ToTable("CoffeeRoundGroupMembers");
                });

            modelBuilder.Entity("Domain.Entities.Prize", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelSettingsId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsMilestone")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRepeatable")
                        .HasColumnType("bit");

                    b.Property<int>("PointCost")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelSettingsId");

                    b.ToTable("Prizes");
                });

            modelBuilder.Entity("Domain.Entities.ChannelMember", b =>
                {
                    b.HasOne("Domain.Entities.ChannelSettings", "ChannelSettings")
                        .WithMany("ChannelMembers")
                        .HasForeignKey("ChannelSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelSettings");
                });

            modelBuilder.Entity("Domain.Entities.ChannelNotice", b =>
                {
                    b.HasOne("Domain.Entities.ChannelSettings", "ChannelSettings")
                        .WithMany("ChannelNotices")
                        .HasForeignKey("ChannelSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelSettings");
                });

            modelBuilder.Entity("Domain.Entities.ClaimedPrize", b =>
                {
                    b.HasOne("Domain.Entities.ChannelMember", "ChannelMember")
                        .WithMany("ClaimedPrizes")
                        .HasForeignKey("ChannelMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Prize", "Prize")
                        .WithMany("ClaimedPrizes")
                        .HasForeignKey("PrizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelMember");

                    b.Navigation("Prize");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRound", b =>
                {
                    b.HasOne("Domain.Entities.ChannelSettings", "ChannelSettings")
                        .WithMany("CoffeeRounds")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelSettings");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRoundGroup", b =>
                {
                    b.HasOne("Domain.Entities.CoffeeRound", "CoffeeRound")
                        .WithMany("CoffeeRoundGroups")
                        .HasForeignKey("CoffeeRoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CoffeeRound");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRoundGroupMember", b =>
                {
                    b.HasOne("Domain.Entities.CoffeeRoundGroup", "CoffeeRoundGroup")
                        .WithMany("CoffeeRoundGroupMembers")
                        .HasForeignKey("CoffeeRoundGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CoffeeRoundGroup");
                });

            modelBuilder.Entity("Domain.Entities.Prize", b =>
                {
                    b.HasOne("Domain.Entities.ChannelSettings", "ChannelSettings")
                        .WithMany("Prizes")
                        .HasForeignKey("ChannelSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChannelSettings");
                });

            modelBuilder.Entity("Domain.Entities.ChannelMember", b =>
                {
                    b.Navigation("ClaimedPrizes");
                });

            modelBuilder.Entity("Domain.Entities.ChannelSettings", b =>
                {
                    b.Navigation("ChannelMembers");

                    b.Navigation("ChannelNotices");

                    b.Navigation("CoffeeRounds");

                    b.Navigation("Prizes");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRound", b =>
                {
                    b.Navigation("CoffeeRoundGroups");
                });

            modelBuilder.Entity("Domain.Entities.CoffeeRoundGroup", b =>
                {
                    b.Navigation("CoffeeRoundGroupMembers");
                });

            modelBuilder.Entity("Domain.Entities.Prize", b =>
                {
                    b.Navigation("ClaimedPrizes");
                });
#pragma warning restore 612, 618
        }
    }
}

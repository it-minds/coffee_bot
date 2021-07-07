using SlackNet.Blocks;
using SlackNet.WebApi;

namespace Slack.Messages
{
  public static class MidwayMessage
  {
    /// <summary>
    /// Generates a message meant for the halfway point or end of the active round.
    ///
    /// WARNING: This message does not contain a conversation id, you need to se it yourself
    /// <code>
    ///  var message = MidwayMessage.Generate(4);
    ///  message.Channel = "MY_CHANNEL_ID";
    /// </code>
    ///
    /// </summary>
    /// <param name="deadlineDays"></param>
    /// <returns></returns>
    public static Message Generate(int deadlineDays)
    {
      // TODO: check if the deadlineDays is less than 1 then set the text on the buttons to indicate that they missed the round regrettably.

      var message = new Message
      {
        Blocks = {
          new SectionBlock {
            Text = new Markdown {
              Type = "mrkdwn",
              Text = "Hello, guys! I am checking in to see if you met for a cup of coffee this round."
            }
          },
          new ActionsBlock {
            Elements = {
              new Button {
                Style = ButtonStyle.Primary,
                Text = new PlainText {
                  Emoji= false,
                  Text = "Yes, We have met!",
                },
                Value = "Yes"
              },
              new Button {
                Style = ButtonStyle.Danger,
                Text = new PlainText {
                  Emoji= false,
                  Text = "No, We haven't met yet."
                },
                Value = "No"
              }
            }
          }
        }
      };

      return message;
    }
  }
}

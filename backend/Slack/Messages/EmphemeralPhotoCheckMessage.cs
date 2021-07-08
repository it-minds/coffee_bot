using SlackNet.Blocks;
using SlackNet.WebApi;

namespace Slack.Messages
{
  public static class EmphemeralPhotoCheckMessage
  {
    /// <summary>
    /// Generates a message meant for when an image is posted in the channel.
    ///
    /// WARNING: This message does not contain a conversation id, you need to se it yourself
    /// <code>
    ///  var message = EmphemeralPhotoCheckMessage.Generate();
    ///  message.Channel = "MY_CHANNEL_ID";
    /// </code>
    ///
    /// </summary>
    /// <param name="deadlineDays"></param>
    /// <returns></returns>
    public static Message Generate()
    {

      var message = new Message
      {
        Blocks = {
          new SectionBlock {
            Text = new Markdown {
              Type = "mrkdwn",
              Text = "Hey! I noticed you posted an image. Is this your group selfie?"
            }
          },
          new ActionsBlock {
            BlockId = ActionTypes.EmphemeralPhoto,
            Elements = {
              new Button {
                Style = ButtonStyle.Primary,
                Text = new PlainText {
                  Emoji= false,
                  Text = "Yes",
                },
                Value = "Yes"
              },
              new Button {
                Style = ButtonStyle.Danger,
                Text = new PlainText {
                  Emoji= false,
                  Text = "No"
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

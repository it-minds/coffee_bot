using System.Collections.Generic;
using System.Text.RegularExpressions;
using SlackNet.Blocks;
using SlackNet.WebApi;
using Defaults = Domain.Defaults.ChannelMessageDefaults;

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
    public static Message Generate(string text, int deadlineDays)
    {
      // TODO: check if the deadlineDays is less than 1 then set the text on the buttons to indicate that they missed the round regrettably.

      string yesPattern = Defaults.TagToPredicate[Defaults.AllTags[5]];
      string noPattern = Defaults.TagToPredicate[Defaults.AllTags[6]];

      string combinedPattern = yesPattern + @"|" + noPattern;

      var yesIndex = Regex.Match(text, yesPattern).Index;
      var noIndex = Regex.Match(text, noPattern).Index;

      var parts = Regex.Split(text, combinedPattern);
      parts[0] = parts[0].Trim();
      parts[1] = parts[1].Trim();
      parts[2] = parts[2].Trim();

      int partsWithText = 0;
      foreach (var part in parts)
        if (part.Length > 0)
          partsWithText++;

      bool buttonsTogether = partsWithText == 1 && parts[1].Length == 0;

      var Blocks = new List<Block>();

      if (buttonsTogether) {
          var buttons = new ActionsBlock {
            BlockId = ActionTypes.MidWay,
            Elements = {
              createButton(yesIndex < noIndex),
              createButton(yesIndex > noIndex)
            }
          };
          if (parts[0].Length > 0) {
            Blocks.Add(createTextBlock(parts[0]));
            Blocks.Add(buttons);
          } else {
            Blocks.Add(buttons);
            Blocks.Add(createTextBlock(parts[2]));
          }
      } else {
        if (parts[0].Length != 0) {
          Blocks.Add(createTextBlock(parts[0]));
        }
        Blocks.Add(new ActionsBlock {
          BlockId = ActionTypes.MidWay,
          Elements = {
            createButton(yesIndex < noIndex)
          }
        });
        Blocks.Add(createTextBlock(parts[1]));
        Blocks.Add(new ActionsBlock {
          BlockId = ActionTypes.MidWay,
          Elements = {
            createButton(yesIndex > noIndex)
          }
        });
        if (parts[2].Length != 0) {
          Blocks.Add(createTextBlock(parts[2]));
        }
      }

      return new Message{
        Blocks = Blocks
      };
    }

    private static Block createTextBlock(string text) {
      return new SectionBlock {
        Text = new Markdown {
          Type = "mrkdwn",
          Text = text
        }
      };
    }

    private static Button createButton(bool yesButton) {
      return new Button {
        Style = ButtonStyle.Danger,
        Text = new PlainText {
          Emoji= false,
          Text = yesButton ? "Yes, We have met!" : "No, We haven't met yet."
        },
        Value = yesButton ? "Yes" : "No"
      };
    }
  }
}

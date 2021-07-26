using System.Collections.Generic;
using System.Linq;
using Slack.DTO;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.WebApi;

namespace Slack.Views
{
  public static class MemberParticipationView
  {
    public static ModalViewDefinition Generate(IList<string> users)
    {
      var modal = new ModalViewDefinition {
        Title = "Select users",
        Blocks = {
          new SectionBlock {
            Text = new Markdown {
              Text = "Please confirm the participants."
            }
          },
          new InputBlock {
            Label = "User",
            Element = new UserMultiSelectMenu {
              InitialUsers = users,
              Placeholder = "Select Members",
              Confirm = null
            },
          }
        },
        Close = "Close",
        Submit = "OK",
        CallbackId = ActionTypes.MemberParticipation,
        PrivateMetadata = "Martins Stuff",
        NotifyOnClose = true
      };


      return modal;
    }
  }
}

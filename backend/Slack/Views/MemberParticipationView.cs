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
            BlockId = BlockIds.MemberParticipation,
            Label = "User",
            Element = new UserMultiSelectMenu {
              ActionId = ActionIds.MemberParticipation,
              InitialUsers = users,
              Placeholder = "Select Members",
              Confirm = null
            },
          }
        },
        Close = "Close",
        Submit = "OK",
        CallbackId = CallbackIds.MemberParticipation,
        PrivateMetadata = "Martins Stuff",
        NotifyOnClose = true
      };


      return modal;
    }

    public static IEnumerable<string> GetSelectedParticipants(this SlackNet.ViewState state) {
      return state.GetValue<UserMultiSelectValue>(BlockIds.MemberParticipation, ActionIds.MemberParticipation)?
          .SelectedUsers ?? new List<string>();
    }
  }
}

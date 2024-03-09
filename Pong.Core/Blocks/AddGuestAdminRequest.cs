using Pong.Core.Models;
using SlackNet.Blocks;

namespace Pong.Core.Blocks;

public static class AddGuestAdminRequest
{
	public const string AdminDenyRequestActionId = "admin_deny_request_action";
	public const string AdminApproveRequestActionId = "admin_approve_request_action";
	public const string AdminDenyValue = "Deny";
	public const string AdminApproveValue = "Approve";
	
	internal static IList<Block> Blocks(string message, bool renderActionsBlock, string requestAction)
	{
		var blocks = new List<Block>
		{
			new HeaderBlock
			{
				Text = "New Guest Request :incoming_envelope:",
			},
			new SectionBlock
			{
				Text = new Markdown(message),
			},
		};

		if (renderActionsBlock)
		{
			blocks.Add(new ActionsBlock
			{
				Elements =
				{
					new Button
					{
						ActionId = AdminDenyRequestActionId,
						Value = AdminDenyValue,
						Text = "Deny",
					},
					new Button
					{
						ActionId = AdminApproveRequestActionId,
						Value = AdminApproveValue,
						Text = "Approve",
					},
				},
			});
		}
		else
		{
			blocks.Add(new SectionBlock
			{
				Text = $"This request has been {requestAction}.",
			});
		}
		
		return blocks;
	}
}
using Pong.Core.Models;
using SlackNet.Blocks;

namespace Pong.Core.Blocks;

internal static class AddGuestAdminRequest
{
	internal const string AdminDenyRequestActionId = "admin_deny_request_action";
	internal const string AdminApproveRequestActionId = "admin_approve_request_action";
	
	internal static List<Block> Blocks(string message, AddGuestForm addGuestForm)
	{
		return
		[
			new HeaderBlock
			{
				Text = "New Guest Request :incoming_envelope:",
			},
			new SectionBlock
			{
				Text = new Markdown(message),
			},
			new ActionsBlock
			{
				Elements =
				{
					new Button
					{
						ActionId = AdminDenyRequestActionId,
						Value = "Deny",
						Text = "Deny",
					},
					new Button
					{
						ActionId = AdminApproveRequestActionId,
						Value = "Approve",
						Text = "Approve",
					},
				},
			},
		];
	}
}
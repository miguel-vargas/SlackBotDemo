using Pong.Core.Models;
using SlackNet.Blocks;

namespace Pong.Core.Blocks;

public static class AddGuestAdminRequest
{
	public const string AdminDenyRequestActionId = "admin_deny_request_action";
	public const string AdminApproveRequestActionId = "admin_approve_request_action";
	public const string AdminDenyValue = "Deny";
	public const string AdminApproveValue = "Approve";
	
	internal static List<Block> Blocks(string message)
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
			},
		];
	}
}
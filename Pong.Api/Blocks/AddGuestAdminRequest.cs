using Pong.Api.Models;
using SlackNet.Blocks;

namespace Pong.Api.Blocks;

internal static class AddGuestAdminRequest
{
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
		];
	}
}
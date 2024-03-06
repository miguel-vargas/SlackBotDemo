using SlackNet;
using SlackNet.Blocks;

namespace Pong.Api.Modals;

internal static class ErrorModal
{
	internal static ModalViewDefinition ModalView(string error)
	{
		return new ModalViewDefinition
		{
			Title = "Invalid Input",
			CallbackId = "error_modal",
			Blocks =
			[
				new SectionBlock
				{
					Text = error,
				},
			],
			Submit = "Dismiss",
			NotifyOnClose = false,
		};
	}
}
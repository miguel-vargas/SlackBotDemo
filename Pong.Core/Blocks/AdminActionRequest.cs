using SlackNet.Blocks;

namespace Pong.Core.Blocks;

public static class AdminActionRequest
{
	internal static List<Block> Blocks(string message)
	{
		return
		[
			new SectionBlock
			{
				Text = new Markdown(message),
			},
		];
	}
}
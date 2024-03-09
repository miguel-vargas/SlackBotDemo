using System.Text.Json;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services.Interfaces;
using SlackNet;
using SlackNet.Blocks;

namespace Pong.Core.Services;

public class SlackModalService : ISlackModalService
{
	public const string AddGuestModalCallbackId = "add_guest_modal";

	public ModalViewDefinition CreateAddGuestFormModal(string channelId, string channelName)
	{
		return new ModalViewDefinition
		{
			Title = "Add Guest",
			CallbackId = AddGuestModalCallbackId,
			Blocks = AddGuestRequest.Blocks,
			Submit = "Submit",
			NotifyOnClose = false,
			PrivateMetadata = JsonSerializer.Serialize(new ChannelMetadata(channelId, channelName)),
		};
	}
	
	public ModalViewDefinition CreateErrorModal(string error)
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
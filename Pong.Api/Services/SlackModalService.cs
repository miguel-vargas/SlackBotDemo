using System.Text.Json;
using Pong.Api.Blocks;
using Pong.Api.Models;
using Pong.Api.Services.Interfaces;
using SlackNet;
using SlackNet.Blocks;

namespace Pong.Api.Services;

public class SlackModalService : ISlackModalService
{
	internal const string AddGuestModalCallbackId = "add_guest_modal";

	public ModalViewDefinition CreateAddGuestFormModal(string channelId, string channelName)
	{
		return new ModalViewDefinition
		{
			Title = "Add Guest",
			CallbackId = AddGuestModalCallbackId,
			Blocks = AddGuestRequest.Blocks,
			Submit = "Submit",
			NotifyOnClose = false,
			PrivateMetadata = JsonSerializer.Serialize(new ModalMetadata(channelId, channelName)),
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
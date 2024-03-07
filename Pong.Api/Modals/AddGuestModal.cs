using System.Text.Json;
using Pong.Api.Models;
using SlackNet;
using SlackNet.Blocks;

namespace Pong.Api.Modals;

internal static class AddGuestModal
{
	internal const string AddGuestModalCallbackId = "add_guest_modal";
	internal const string GuestEmailInputActionId = "guest_email_input";
	internal const string ChannelSelectActionId = "channel_select_menu";
	internal const string BusinessJustificationInputActionId = "guest_reason_input";
	internal const string ExpirationDatePickerActionId = "expiration_date_picker";

	internal static ModalViewDefinition ModalView(string channelId, string channelName) => new()
	{
		Title = "Add Guest",
		CallbackId = AddGuestModalCallbackId,
		Blocks = Blocks,
		Submit = "Submit",
		NotifyOnClose = false,
		PrivateMetadata = JsonSerializer.Serialize(new ModalMetadata(channelId, channelName)),
	};

	private static List<Block> Blocks =>
	[
		new InputBlock
		{
			Label = "Guest Email",
			BlockId = "guest_email_block",
			Optional = false,
			Element = new PlainTextInput
			{
				ActionId = GuestEmailInputActionId,
				Placeholder = "Enter the guest email.",
			},
		},

		new InputBlock
		{
			Label = "Channel",
			BlockId = "channel_select_block",
			Optional = false,
			Element = new ChannelSelectMenu
			{
				ActionId = ChannelSelectActionId,
				Placeholder = "Select the channel.",
			},
		},

		new InputBlock
		{
			Label = "Business Justification",
			BlockId = "guest_reason_block",
			Optional = true,
			Element = new PlainTextInput
			{
				ActionId = BusinessJustificationInputActionId,
				Placeholder = "Please enter a business justification.",
			},
		},

		new InputBlock
		{
			Label = "Date",
			BlockId = "expiration_date_block",
			Optional = false,
			Element = new DatePicker
			{
				ActionId = ExpirationDatePickerActionId,
				InitialDate = DateTime.Now.AddYears(1),
			},
			DispatchAction = true,
		},
	];
}
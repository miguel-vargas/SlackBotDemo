using SlackNet;
using SlackNet.Blocks;

namespace Pong.Api.Modals;

internal static class AddGuestModal
{
	internal const string ExpirationDatePickerActionId = "expiration_date_picker";

	internal static ModalViewDefinition ModalView => new()
	{
		Title = "Add Guest",
		CallbackId = "add_guest_modal",
		Blocks = Blocks,
		Submit = "Submit",
		NotifyOnClose = false,
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
				ActionId = "guest_email_input",
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
				ActionId = "channel_select_menu",
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
				ActionId = "guest_email_input",
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
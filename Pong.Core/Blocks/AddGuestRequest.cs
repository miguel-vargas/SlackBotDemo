using SlackNet.Blocks;

namespace Pong.Core.Blocks;

public static class AddGuestRequest
{
	public const string GuestEmailInputActionId = "guest_email_input";
	public const string ChannelSelectActionId = "channel_select_menu";
	public const string BusinessJustificationInputActionId = "guest_reason_input";
	public const string ExpirationDatePickerActionId = "expiration_date_picker";

	internal static List<Block> Blocks =>
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
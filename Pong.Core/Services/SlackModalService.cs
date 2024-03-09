using System.Text;
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
	public const string AdminApprovalModalCallbackId = "admin_approval_modal";
	public const string AdminDenialModalCallbackId = "admin_denial_modal";

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
	
	public ModalViewDefinition CreateAdminDenialModal(AddGuestForm addGuestForm)
	{
		return new ModalViewDefinition
		{
			Title = $"{AddGuestAdminRequest.AdminDenyValue} Request",
			CallbackId = AdminDenialModalCallbackId,
			Blocks = AdminActionRequest.Blocks(addGuestForm.ToMarkdownString()),
			Submit = "Submit",
			NotifyOnClose = false,
			PrivateMetadata = JsonSerializer.Serialize(addGuestForm),
		};
	}
	
	public ModalViewDefinition CreateAdminApprovalModal(AddGuestForm addGuestForm)
	{
		return new ModalViewDefinition
		{
			Title = $"{AddGuestAdminRequest.AdminApproveValue} Request",
			CallbackId = AdminApprovalModalCallbackId,
			Blocks = AdminActionRequest.Blocks(addGuestForm.ToMarkdownString()),
			Submit = "Submit",
			NotifyOnClose = false,
			PrivateMetadata = JsonSerializer.Serialize(addGuestForm),
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
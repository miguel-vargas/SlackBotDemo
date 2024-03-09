using System.Text.Json;
using Microsoft.Extensions.Options;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services;
using Pong.Core.Services.Interfaces;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace Pong.Core.Handlers;

public class AddGuestAdminHandler(
	ISlackApiClient slackApiClient,
	ISlackMessageService slackMessageService,
	ISlackModalService slackModalService,
	IOptions<SlackConfiguration> options)
	: IBlockActionHandler<ButtonAction>, IViewSubmissionHandler
{
	public async Task Handle(ButtonAction action, BlockActionRequest request)
	{
		switch (action.Value)
		{
			case AddGuestAdminRequest.AdminDenyValue:
				await HandleGuestDenial(request);
				break;
			case AddGuestAdminRequest.AdminApproveValue:
				await HandleGuestApproval(request);
				break;
			default:
				break;
		}
	}
	
	public async Task<ViewSubmissionResponse> Handle(ViewSubmission viewSubmission)
	{
		var adminConfirmationMetadata = JsonSerializer.Deserialize<AdminConfirmationMetadata>(viewSubmission.View.PrivateMetadata);

		if (adminConfirmationMetadata == null)
		{
			throw new InvalidOperationException(nameof(adminConfirmationMetadata));
		}

		switch (viewSubmission.View.CallbackId)
		{
			case SlackModalService.AdminDenialModalCallbackId:
				await HandleGuestDenialConfirmation(viewSubmission, adminConfirmationMetadata);
				break;
			case SlackModalService.AdminApprovalModalCallbackId:
				await HandleGuestApprovalConfirmation(viewSubmission, adminConfirmationMetadata);
				break;
		}

		return ViewSubmissionResponse.Null;
	}
	
	public Task HandleClose(ViewClosed viewClosed)
	{
		Console.WriteLine($"{viewClosed.User.Name} cancelled the admin confirmation modal view.");
		return Task.CompletedTask;
	}

	private async Task HandleGuestDenial(BlockActionRequest request)
	{
		var adminMetadata = new ChannelMetadata(request.Channel.Id, request.Channel.Name);
		var adminDenialModal =
			slackModalService.CreateAdminDenialModal(request.Message.Ts, request.Message.Metadata.ToObject<AddGuestForm>());
		await slackApiClient.Views.Open(request.TriggerId, adminDenialModal);
	}
	
	private async Task HandleGuestDenialConfirmation(ViewSubmission viewSubmission, AdminConfirmationMetadata adminConfirmationMetadata)
	{
		var updatedAdminRequestMessage = slackMessageService.CreateUpdatedAdminRequest(options.Value.AddGuestAdminChannel, adminConfirmationMetadata.AdminRequestTs, adminConfirmationMetadata.AddGuestForm, "denied");
		await slackApiClient.Chat.Update(updatedAdminRequestMessage);
		await slackApiClient.Chat.PostMessage(slackMessageService.CreateRequestReplyMessage(
			adminConfirmationMetadata.AddGuestForm.RequestorId, adminConfirmationMetadata.AddGuestForm.RequestorChannel,
			adminConfirmationMetadata.AddGuestForm.RequestThreadTimestamp, "denied"));
	}
	
	private async Task HandleGuestApproval(BlockActionRequest request)
	{
		var adminApprovalModal =
			slackModalService.CreateAdminApprovalModal(request.Message.Ts, request.Message.Metadata.ToObject<AddGuestForm>());
		await slackApiClient.Views.Open(request.TriggerId, adminApprovalModal);
	}
	
	private async Task HandleGuestApprovalConfirmation(ViewSubmission viewSubmission, AdminConfirmationMetadata adminConfirmationMetadata)
	{
		var updatedAdminRequestMessage = slackMessageService.CreateUpdatedAdminRequest(options.Value.AddGuestAdminChannel, adminConfirmationMetadata.AdminRequestTs, adminConfirmationMetadata.AddGuestForm, "approved");
		await slackApiClient.Chat.Update(updatedAdminRequestMessage);
		await slackApiClient.Chat.PostMessage(slackMessageService.CreateRequestReplyMessage(
			adminConfirmationMetadata.AddGuestForm.RequestorId, adminConfirmationMetadata.AddGuestForm.RequestorChannel,
			adminConfirmationMetadata.AddGuestForm.RequestThreadTimestamp, "approved"));
	}
}
using Microsoft.Extensions.Options;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services.Interfaces;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace Pong.Core.Handlers;

public class AddGuestAdminHandler(
	ISlackApiClient slackApiClient,
	ISlackMessageService slackMessageService,
	ISlackModalService slackModalService)
	: IBlockActionHandler<ButtonAction>
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

	private async Task HandleGuestDenial(BlockActionRequest request)
	{
		var adminDenialModal =
			slackModalService.CreateAdminDenialModal(request.Message.Metadata.ToObject<AddGuestForm>());
		await slackApiClient.Views.Open(request.TriggerId, adminDenialModal);
	}
	
	private async Task HandleGuestApproval(BlockActionRequest request)
	{
		var adminApprovalModal =
			slackModalService.CreateAdminApprovalModal(request.Message.Metadata.ToObject<AddGuestForm>());
		await slackApiClient.Views.Open(request.TriggerId, adminApprovalModal);
	}
}
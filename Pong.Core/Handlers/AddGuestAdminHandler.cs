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
				await HandleGuestDenial(request.Channel.Id);
				break;
			case AddGuestAdminRequest.AdminApproveValue:
				await HandleGuestApproval(request.Channel.Id);
				break;
			default:
				break;
		}
	}

	private async Task HandleGuestDenial(string channelId)
	{
		await slackApiClient.Chat.PostMessage(new Message
		{
			Channel = channelId,
			Text = "Deny Hit",
		});
	}
	
	private async Task HandleGuestApproval(string channelId)
	{
		await slackApiClient.Chat.PostMessage(new Message
		{
			Channel = channelId,
			Text = "Approve Hit",
		});
	}
}
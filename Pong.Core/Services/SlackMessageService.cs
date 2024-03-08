using System.Text;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services.Interfaces;
using SlackNet.WebApi;

namespace Pong.Core.Services;

public class SlackMessageService : ISlackMessageService
{
	public Message CreateRequestSubmissionMessage(string channelId, string requestorId)
	{
		return new Message
		{
			Channel = channelId,
			Text =
				$"<@{requestorId}> your request has been submitted and will be reviewed. This thread will be updated once a decision has been made.",
		};
	}

	public Message CreateAdminRequestMessage(string addGuestAdminChannel, AddGuestForm addGuestForm)
	{
		var sb = new StringBuilder();

		sb.Append($"<@{addGuestForm.RequestorId}> has requested to add a guest. \n");
		sb.Append($"*Thread:* <{addGuestForm.RequestThreadPermalink}| Request Thread> \n");
		sb.Append($"*Guest Email:* {addGuestForm.GuestEmail} \n");
		sb.Append($"*Channel Id To Add Guest:* <#{addGuestForm.ChannelIdToAddGuest}> \n");
		sb.Append($"*Business Justification:* {addGuestForm.BusinessJustification} \n");
		sb.Append($"*Expiration Date:* {DateOnly.FromDateTime(addGuestForm.ExpirationDate)} \n");

		return new Message
		{
			Channel = addGuestAdminChannel,
			Blocks = AddGuestAdminRequest.Blocks(sb.ToString(), addGuestForm),
		};
	}
}
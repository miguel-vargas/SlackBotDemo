using System.Text;
using System.Text.Json;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services.Interfaces;
using SlackNet;
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
		return new Message
		{
			Channel = addGuestAdminChannel,
			Blocks = AddGuestAdminRequest.Blocks(addGuestForm.ToMarkdownString()),
			MetadataJson = MessageMetadata.FromObject(addGuestForm),
		};
	}
}
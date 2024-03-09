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
			Blocks = AddGuestAdminRequest.Blocks(addGuestForm.ToMarkdownString(), true, ""),
			MetadataJson = MessageMetadata.FromObject(addGuestForm),
		};
	}

	public Message CreateRequestReplyMessage(string requestorId, string channelId, string threadTs, string requestAction)
	{
		return new Message
		{
			Channel = channelId,
			ThreadTs = threadTs,
			Text =
				$"<@{requestorId}> your request has been {requestAction}.",
		};
	}

	public MessageUpdate CreateUpdatedAdminRequest(string addGuestAdminChannelId, string originalMessageTs,
		AddGuestForm addGuestForm, string requestAction)
	{
		return new MessageUpdate
		{
			ChannelId = addGuestAdminChannelId,
			Ts = originalMessageTs,
			Blocks = AddGuestAdminRequest.Blocks(addGuestForm.ToMarkdownString(), false, requestAction),
		};
	}
}
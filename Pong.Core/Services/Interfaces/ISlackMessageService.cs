using Pong.Core.Models;
using SlackNet.WebApi;

namespace Pong.Core.Services.Interfaces;

public interface ISlackMessageService
{
	Message CreateRequestSubmissionMessage(string channelId, string requestorId);
	Message CreateAdminRequestMessage(string addGuestAdminChannel, AddGuestForm addGuestForm);
	Message CreateRequestReplyMessage(string requestorId, string channelId, string threadTs, string requestAction);
	MessageUpdate CreateUpdatedAdminRequest(string addGuestAdminChannelId, string originalMessageTs,
		AddGuestForm addGuestForm, string requestAction);
}
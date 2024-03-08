using Pong.Api.Models;
using SlackNet.WebApi;

namespace Pong.Api.Services.Interfaces;

public interface ISlackMessageService
{
	Message CreateRequestSubmissionMessage(string channelId, string requestorId);
	Message CreateAdminRequestMessage(string addGuestAdminChannel, AddGuestForm addGuestForm);
}
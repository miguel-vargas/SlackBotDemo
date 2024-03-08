using SlackNet;

namespace Pong.Api.Services.Interfaces;

public interface ISlackModalService
{
	ModalViewDefinition CreateAddGuestFormModal(string channelId, string channelName);
	ModalViewDefinition CreateErrorModal(string error);
}
using SlackNet;

namespace Pong.Core.Services.Interfaces;

public interface ISlackModalService
{
	ModalViewDefinition CreateAddGuestFormModal(string channelId, string channelName);
	ModalViewDefinition CreateErrorModal(string error);
}
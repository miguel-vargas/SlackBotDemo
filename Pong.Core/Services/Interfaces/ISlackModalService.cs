using Pong.Core.Models;
using SlackNet;

namespace Pong.Core.Services.Interfaces;

public interface ISlackModalService
{
	ModalViewDefinition CreateAddGuestFormModal(string channelId, string channelName);
	ModalViewDefinition CreateAdminDenialModal(string adminRequestTs, AddGuestForm addGuestForm);
	ModalViewDefinition CreateAdminApprovalModal(string adminRequestTs, AddGuestForm addGuestForm);
	ModalViewDefinition CreateErrorModal(string error);
}
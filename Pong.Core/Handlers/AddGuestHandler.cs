using System.Text.Json;
using Microsoft.Extensions.Options;
using Pong.Core.Blocks;
using Pong.Core.Models;
using Pong.Core.Services.Interfaces;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace Pong.Core.Handlers;

public class AddGuestHandler(
	ISlackApiClient slackApiClient,
	ISlackMessageService slackMessageService,
	ISlackModalService slackModalService,
	IOptions<SlackConfiguration> options)
	: ISlashCommandHandler, IBlockActionHandler<DatePickerAction>, IViewSubmissionHandler
{
	public const string AddGuestCommand = "/add-guest";
	private const string InvalidDateErrorPattern = "Please select a date on or before {0}";

	public async Task<SlashCommandResponse> Handle(SlashCommand command)
	{
		var addGuestFormModal = slackModalService.CreateAddGuestFormModal(command.ChannelId, command.ChannelName);
		await slackApiClient.Views.Open(command.TriggerId, addGuestFormModal);
		return new SlashCommandResponse{ Message = new Message{ Text = "Initiated Add Guest Command"}};
	}

	public async Task Handle(DatePickerAction action, BlockActionRequest request)
	{
		var oneYearFromToday = DateTime.Now.AddYears(1);
		if (action.SelectedDate > oneYearFromToday)
		{
			var error = string.Format(InvalidDateErrorPattern, DateOnly.FromDateTime(oneYearFromToday));
			var errorModal = slackModalService.CreateErrorModal(error);
			await slackApiClient.Views.Push(request.TriggerId, errorModal);
		}
	}

	public async Task<ViewSubmissionResponse> Handle(ViewSubmission viewSubmission)
	{
		var metadata = JsonSerializer.Deserialize<ChannelMetadata>(viewSubmission.View.PrivateMetadata);

		if (metadata == null)
		{
			throw new InvalidOperationException(nameof(metadata));
		}

		var state = viewSubmission.View.State;
		var expirationDate = state.GetValue<DatePickerValue>(AddGuestRequest.ExpirationDatePickerActionId).SelectedDate;
		var requestorId = viewSubmission.User.Id;

		var requestSubmissionMessage =
			await slackApiClient.Chat.PostMessage(
				slackMessageService.CreateRequestSubmissionMessage(metadata.ChannelId, requestorId));

		var permalink = await slackApiClient.Chat.GetPermalink(metadata.ChannelId, requestSubmissionMessage.Ts);

		var formValues = new AddGuestForm
		{
			RequestorId = requestorId,
			RequestorChannel = requestSubmissionMessage.Channel,
			RequestThreadTimestamp = requestSubmissionMessage.Ts,
			RequestThreadPermalink = permalink.Permalink,
			GuestEmail = state.GetValue<PlainTextInputValue>(AddGuestRequest.GuestEmailInputActionId).Value,
			ChannelIdToAddGuest = state.GetValue<ChannelSelectValue>(AddGuestRequest.ChannelSelectActionId)
				.SelectedChannel,
			BusinessJustification =
				state.GetValue<PlainTextInputValue>(AddGuestRequest.BusinessJustificationInputActionId).Value,
			ExpirationDate = expirationDate ?? DateTime.Now.AddYears(1),
		};

		await slackApiClient.Chat.PostMessage(
			slackMessageService.CreateAdminRequestMessage(options.Value.AddGuestAdminChannel, formValues));

		return ViewSubmissionResponse.Null;
	}

	public Task HandleClose(ViewClosed viewClosed)
	{
		Console.WriteLine($"{viewClosed.User.Name} cancelled the add guest modal view.");
		return Task.CompletedTask;
	}
}
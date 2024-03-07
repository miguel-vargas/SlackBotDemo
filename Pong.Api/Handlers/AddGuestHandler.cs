using System.Text.Json;
using Microsoft.Extensions.Options;
using Pong.Api.Modals;
using Pong.Api.Models;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;
using SlackNet.WebApi;

namespace Pong.Api.Handlers;

public class AddGuestHandler(ISlackApiClient slackApiClient, IOptions<SlackConfiguration> options)
	: ISlashCommandHandler, IBlockActionHandler<DatePickerAction>, IViewSubmissionHandler
{
	public const string AddGuestCommand = "/add-guest";
	private const string InvalidDateErrorPattern = "Please select a date on or before {0}";

	public async Task<SlashCommandResponse> Handle(SlashCommand command)
	{
		await slackApiClient.Views.Open(command.TriggerId, AddGuestModal.ModalView(command.ChannelId, command.ChannelName));
		return new SlashCommandResponse();
	}

	public async Task Handle(DatePickerAction action, BlockActionRequest request)
	{
		var oneYearFromToday = DateTime.Now.AddYears(1);
		if (action.SelectedDate > oneYearFromToday)
		{
			var error = string.Format(InvalidDateErrorPattern, DateOnly.FromDateTime(oneYearFromToday));
			await slackApiClient.Views.Push(request.TriggerId, ErrorModal.ModalView(error));
		}
	}

	public async Task<ViewSubmissionResponse> Handle(ViewSubmission viewSubmission)
	{
		var metadata = JsonSerializer.Deserialize<ModalMetadata>(viewSubmission.View.PrivateMetadata);
		
		var state = viewSubmission.View.State;
		var expirationDate = state.GetValue<DatePickerValue>(AddGuestModal.ExpirationDatePickerActionId).SelectedDate;
		var requestorId = viewSubmission.User.Id;

		var requestSubmissionMessage = await slackApiClient.Chat.PostMessage(new Message
		{
			Channel = metadata.ChannelId,
			Text = $"<@{requestorId}> your request has been submitted and will be reviewed. This thread will be updated once a decision has been made.",
		});

		var formValues = new AddGuestForm
		{
			RequestorId = requestorId,
			RequestorChannel = requestSubmissionMessage.Channel,
			RequestThreadTimestamp = requestSubmissionMessage.Ts,
			GuestEmail = state.GetValue<PlainTextInputValue>(AddGuestModal.GuestEmailInputActionId).Value,
			ChannelToAddGuest = state.GetValue<ChannelSelectValue>(AddGuestModal.ChannelSelectActionId).SelectedChannel,
			BusinessJustification =
				state.GetValue<PlainTextInputValue>(AddGuestModal.BusinessJustificationInputActionId).Value,
			ExpirationDate = expirationDate ?? DateTime.Now.AddYears(1),
		};

		await slackApiClient.Chat.PostMessage(new Message
		{
			Channel = options.Value.AddGuestAdminChannel,
			Text = $"Submitted {formValues}",
		});
		
		return ViewSubmissionResponse.Null;
	}

	public Task HandleClose(ViewClosed viewClosed)
	{
		Console.WriteLine($"{viewClosed.User.Name} cancelled the add guest modal view.");
		return Task.CompletedTask;
	}
}
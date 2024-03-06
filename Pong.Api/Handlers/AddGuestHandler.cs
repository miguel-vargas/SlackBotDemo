using Pong.Api.Modals;
using SlackNet;
using SlackNet.Blocks;
using SlackNet.Interaction;

namespace Pong.Api.Handlers;

public class AddGuestHandler(ISlackApiClient slackApiClient)
	: ISlashCommandHandler, IBlockActionHandler<DatePickerAction>
{
	public const string AddGuestCommand = "/add-guest";
	private const string InvalidDateErrorPattern = "Please select a date on or before {0}";

	public async Task<SlashCommandResponse> Handle(SlashCommand command)
	{
		await slackApiClient.Views.Open(command.TriggerId, AddGuestModal.ModalView);
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
}
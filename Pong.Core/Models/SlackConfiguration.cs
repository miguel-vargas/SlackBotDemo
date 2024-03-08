namespace Pong.Core.Models;

public record SlackConfiguration
{
	public const string SlackConfigKey = "Slack";
	public string AccessToken { get; init; }
	public string AddGuestAdminChannel { get; init; }
	public string AppToken { get; init; }
	public string SigningSecret { get; init; }
}
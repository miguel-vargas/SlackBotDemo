namespace Pong.Api;

public record SlackConfiguration
{
	internal const string SlackConfigKey = "Slack";
	public required string AccessToken { get; init; }
	public required string AddGuestAdminChannel { get; init; }
	public required string AppToken { get; init; }
	public required string SigningSecret { get; init; }
}
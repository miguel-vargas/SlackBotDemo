namespace Pong.Core.Models;

public record SlackConfiguration
{
	public const string SlackConfigKey = "Slack";
	public string AccessToken { get; init; } = null!;
	public string AddGuestAdminChannel { get; init; } = null!;
	public string AppToken { get; init; } = null!;
	public string SigningSecret { get; init; } = null!;
}
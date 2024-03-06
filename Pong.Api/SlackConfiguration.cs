namespace Pong.Api;

public record SlackConfiguration
{
	public required string AccessToken { get; init; }
	public required string AppToken { get; init; }
	public required string SigningSecret { get; init; }
}
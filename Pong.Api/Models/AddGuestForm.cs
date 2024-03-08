namespace Pong.Api.Models;

public record AddGuestForm
{
	public string RequestorId { get; init; }
	public string RequestorChannel { get; init; }
	public string RequestThreadTimestamp { get; init; }
	public string RequestThreadPermalink { get; init; }
	public string GuestEmail { get; init; }
	public string ChannelIdToAddGuest { get; init; }
	public string BusinessJustification { get; init; }
	public DateTime ExpirationDate { get; init; }
}
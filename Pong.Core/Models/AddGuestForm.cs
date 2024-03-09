using System.Text;

namespace Pong.Core.Models;

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

	public string ToMarkdownString()
	{
		var sb = new StringBuilder();

		sb.Append($"<@{RequestorId}> has requested to add a guest. \n");
		sb.Append($"*Thread:* <{RequestThreadPermalink}| Request Thread> \n");
		sb.Append($"*Guest Email:* {GuestEmail} \n");
		sb.Append($"*Channel Id To Add Guest:* <#{ChannelIdToAddGuest}> \n");
		sb.Append($"*Business Justification:* {BusinessJustification} \n");
		sb.Append($"*Expiration Date:* {DateOnly.FromDateTime(ExpirationDate)} \n");

		return sb.ToString();
	}
}
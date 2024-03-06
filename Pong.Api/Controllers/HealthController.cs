using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Pong.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController(HealthCheckService healthCheckService) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var report = await healthCheckService.CheckHealthAsync();

		return report.Status == HealthStatus.Healthy
			? Ok(report)
			: StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
	}
}
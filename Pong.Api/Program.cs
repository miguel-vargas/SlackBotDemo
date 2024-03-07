using Pong.Api;
using Pong.Api.Handlers;
using Pong.Api.Modals;
using SlackNet.AspNetCore;
using SlackNet.Blocks;

var builder = WebApplication.CreateBuilder(args);

var slackSection = builder.Configuration.GetSection("Slack");
var slackConfiguration = slackSection.Get<SlackConfiguration>() ??
                         throw new NullReferenceException("Slack configuration missing.");

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SlackConfiguration>(slackSection);

builder.Services.AddSlackNet(c =>
{
	c.UseApiToken(slackConfiguration.AccessToken);
	c.UseAppLevelToken(slackConfiguration.AppToken);
	c.RegisterBlockActionHandler<DatePickerAction, AddGuestHandler>(AddGuestModal.ExpirationDatePickerActionId);
	c.RegisterSlashCommandHandler<AddGuestHandler>(AddGuestHandler.AddGuestCommand);
	c.RegisterViewSubmissionHandler<AddGuestHandler>(AddGuestModal.AddGuestModalCallbackId);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseSlackNet(c => c.UseSocketMode(true));

app.Run();
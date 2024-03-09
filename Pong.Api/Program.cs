using Pong.Core.Blocks;
using Pong.Core.Handlers;
using Pong.Core.Models;
using Pong.Core.Services;
using Pong.Core.Services.Interfaces;
using SlackNet.AspNetCore;
using SlackNet.Blocks;

var builder = WebApplication.CreateBuilder(args);

var slackSection = builder.Configuration.GetSection(SlackConfiguration.SlackConfigKey);
var slackConfiguration = slackSection.Get<SlackConfiguration>() ??
                         throw new NullReferenceException("Slack configuration missing.");

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISlackMessageService, SlackMessageService>();
builder.Services.AddScoped<ISlackModalService, SlackModalService>();
builder.Services.Configure<SlackConfiguration>(slackSection);

builder.Services.AddSlackNet(c =>
{
	c.UseApiToken(slackConfiguration.AccessToken);
	c.UseAppLevelToken(slackConfiguration.AppToken);
	c.RegisterBlockActionHandler<DatePickerAction, AddGuestHandler>(AddGuestRequest.ExpirationDatePickerActionId);
	c.RegisterSlashCommandHandler<AddGuestHandler>(AddGuestHandler.AddGuestCommand);
	c.RegisterViewSubmissionHandler<AddGuestHandler>(SlackModalService.AddGuestModalCallbackId);
	c.RegisterBlockActionHandler<ButtonAction, AddGuestAdminHandler>(AddGuestAdminRequest.AdminDenyRequestActionId);
	c.RegisterBlockActionHandler<ButtonAction, AddGuestAdminHandler>(AddGuestAdminRequest.AdminApproveRequestActionId);
	c.RegisterViewSubmissionHandler<AddGuestAdminHandler>(SlackModalService.AdminDenialModalCallbackId);
	c.RegisterViewSubmissionHandler<AddGuestAdminHandler>(SlackModalService.AdminApprovalModalCallbackId);

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
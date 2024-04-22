using Microsoft.Extensions.Hosting;
using Tennis.Interfaces;
using Tennis.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITrainingTeamService, TrainingTeamService>();
builder.Services.AddTransient<ITennisLobbyService, TennisLobbyService>();
builder.Services.AddTransient<ILaneService, LaneService>();
builder.Services.AddTransient<ILaneBookingService, LaneBookingService>();
builder.Services.AddTransient<IEventService, EventService>();
builder.Services.AddTransient<IEventBookingService, EventBookingService>();
builder.Services.AddTransient<IArticleService, ArticleService>();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

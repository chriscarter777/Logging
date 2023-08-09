using Logging.Common.Services;
using Logging.NLog.Services;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITranscriptBuilder, TranscriptBuilder>();

// ---------------- logging ------------------
builder.Host.UseNLog();

string logTemplate = "${longdate}|${event-properties:item=EventId:whenEmpty=0}|${level:uppercase=true}|${callsite}|${message} ${exception:format=tostring}";
NLog.LogManager.Setup().LoadConfiguration(builder => {
     //WriteToDebug() is superfluous if using WriteToConsole();
     builder.ForLogger().FilterMinLevel(NLog.LogLevel.Debug).WriteToConsole(layout: logTemplate);
     builder.ForLogger().FilterMinLevel(NLog.LogLevel.Debug).WriteToFile(fileName: @"C:\temp\nlog-LoggingDemo-all-${shortdate}.log", layout: logTemplate);
     builder.ForLogger().FilterMinLevel(NLog.LogLevel.Warn).WriteToFile(fileName: @"C:\temp\nlog-LoggingDemo-error-${shortdate}.log", layout: logTemplate);
});

// -------------------------------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
     app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

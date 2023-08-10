using Logging.Common.Services;
using Logging.Serilog.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITranscriptBuilder, TranscriptBuilder>();

// ---------------- logging ------------------
builder.Host.UseSerilog();

string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Method} | {Session} | {Message:lj}{NewLine}{Exception}";
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .WriteTo.Debug(outputTemplate: logTemplate)
    .WriteTo.Console(outputTemplate: logTemplate)
    .WriteTo.File(@"C:\temp\serilog-LoggingDemo-.log", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate)
    .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
    .CreateLogger();


// ------------------------------------------

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

using Logging.Common.Models;
using Logging.Common.Services;
using Logging.Serilog.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Diagnostics;
using System.Net;

namespace Logging.Serilog.Controllers;
public class HomeController : Controller
{
     private readonly ILogger<HomeController> _logger;
     private readonly ITranscriptBuilder _transcriptBuilder;

     public HomeController(ILogger<HomeController> logger, ITranscriptBuilder transcriptBuilder)
     {
          _logger = logger;
          _transcriptBuilder = transcriptBuilder;
     }

     public IActionResult Index()
     {
          string method = $"{nameof(HomeController)}.{nameof(Index)}";
          Guid session = Guid.NewGuid();
          IPAddress? userIP = Request.HttpContext.Connection.RemoteIpAddress;
          LogContext.PushProperty("Method", method);
          LogContext.PushProperty("Session", session);
          WriteLogDelineator();

          _logger.LogInformation("Called by UserIP {ip}.", userIP);
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, false);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);
               LogContext.PushProperty("Method", method);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning("Unexpected number of transcripts returned.");
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation("Returning {count}.", transcripts.Count);
               return View(viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, "EXCEPTION");
               TranscriptsViewModel viewModel = new TranscriptsViewModel(ex.Message);
               return View(viewModel);
          }
          finally
          {
               WriteLogDelineator();
               LogContext.Reset();
          }
     }

     public IActionResult ShowMeAnException()
     {
          string method = $"{nameof(HomeController)}.{nameof(ShowMeAnException)}";
          Guid session = Guid.NewGuid();
          IPAddress? userIP = Request.HttpContext.Connection.RemoteIpAddress;
          LogContext.PushProperty("Method", method);
          LogContext.PushProperty("Session", session);
          WriteLogDelineator();

          _logger.LogInformation("Called by UserIP {ip}.", userIP);
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, true);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);
               LogContext.PushProperty("Method", method);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning("Unexpected number of transcripts returned.");
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation("Returning {count}.", transcripts.Count);
               return View("Index", viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(ShowMeAnException)} session {session}");
               TranscriptsViewModel viewModel = new TranscriptsViewModel(ex.Message);
               return View("Index", viewModel);
          }
          finally
          {
               WriteLogDelineator();
               LogContext.Reset();
          }
     }

     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
     public IActionResult Error()
     {
          return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
     }


     //private methods

     private void WriteLogDelineator()
     {
          _logger.LogInformation("\n=====================================================================================");
     }
}

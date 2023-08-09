using Logging.Common.Models;
using Logging.Common.Services;
using Logging.NLog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace Logging.NLog.Controllers;
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
          string method = nameof(Index);
          Guid session = Guid.NewGuid();
          IPAddress? userIP = Request.HttpContext.Connection.RemoteIpAddress;
          WriteLogDelineator();

          _logger.LogInformation("{method} starting: userIP={userIP} session={session}",  method, userIP, session);
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, false);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning($"{nameof(Index)} session {session}: Unexpected number of transcripts returned.");
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation($"{nameof(Index)} returning {transcripts.Count} transcripts: session={session} .");
               return View(viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(Index)} session {session}");
               TranscriptsViewModel viewModel = new TranscriptsViewModel(ex.Message);
               return View(viewModel);
          }
          finally
          {
               WriteLogDelineator();
          }
     }

     public IActionResult ShowMeAnException()
     {
          string method = nameof(ShowMeAnException);
          Guid session = Guid.NewGuid();
          IPAddress? userIP = Request.HttpContext.Connection.RemoteIpAddress;
          WriteLogDelineator();

          _logger.LogInformation("{method} starting: userIP={userIP} session={session}", method, userIP, session);
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, true);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning($"{nameof(Index)} session {session}: Unexpected number of transcripts returned.");
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation($"{nameof(ShowMeAnException)} returning {transcripts.Count} transcripts: session={session} .");
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

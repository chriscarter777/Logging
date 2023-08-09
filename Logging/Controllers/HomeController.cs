using Logging.Common.Models;
using Logging.Common.Services;
using Logging.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace Logging.Controllers;
public class HomeController : Controller
{
     private readonly ILogger<HomeController> _logger;
     private readonly ITranscriptBuilder _transcriptBuilder;
     private readonly string _timestampFormat = "yyyy-MM-dd HH:mm:ss.sss";

     public HomeController(ILogger<HomeController> logger, ITranscriptBuilder transcriptBuilder)
     {
          _logger = logger;
          _transcriptBuilder = transcriptBuilder;
     }

     public IActionResult Index()
     {
          Guid session = Guid.NewGuid();
          WriteLogDelineator();

          //verbatim logging
          _logger.LogInformation($"{DateTime.Now.ToString(_timestampFormat)} Index starting: userIP={Request.HttpContext.Connection.RemoteIpAddress} session {session}");
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, false);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning($"{DateTime.Now.ToString(_timestampFormat)} Index session {session}: Unexpected number of transcripts ({transcripts.Count}) returned.");
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation($"{DateTime.Now.ToString(_timestampFormat)} Index returning {transcripts.Count} transcripts: session={session}");
               return View(viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{DateTime.Now.ToString(_timestampFormat)} Index session {session}");
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

          //logging with variables
          _logger.LogInformation("{timestamp} {method} starting: userIP={userIP} session={session}", DateTime.Now.ToString(_timestampFormat), method, userIP, session);
          try
          {
               BuildTranscriptRequest buildRequest = new BuildTranscriptRequest(session, 3, true);
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(buildRequest);

               if (transcripts.Count != 3)
               {
                    _logger.LogWarning("{timestamp} {method} session {session}: Unexpected number of transcripts ({count}) returned.", DateTime.Now.ToString(_timestampFormat), method, session, transcripts.Count);
               }

               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               _logger.LogInformation("{timestamp} {method} returning {count} transcripts: session={session}", DateTime.Now.ToString(_timestampFormat), method, transcripts.Count, session);
               return View("Index", viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, "{timestamp} {method} session {session}", DateTime.Now.ToString(_timestampFormat), method, session);
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

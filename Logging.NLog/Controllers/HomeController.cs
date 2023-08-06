using Logging.Common.Models;
using Logging.Common.Services;
using Logging.NLog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
          Guid session = Guid.NewGuid();
          _logger.LogInformation($"{nameof(Index)} session {session} starting.");
          try
          {
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(session, 3, false);
               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               return View(viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(Index)} session {session}");
               TranscriptsViewModel viewModel = new TranscriptsViewModel(ex.Message);
               return View(viewModel);
          }
     }
     public IActionResult ErroredIndex()
     {
          Guid session = Guid.NewGuid();
          _logger.LogInformation($"{nameof(ErroredIndex)} session {session} starting.");
          try
          {
               List<Transcript> transcripts = _transcriptBuilder.BuildMany(session, 3, true);
               TranscriptsViewModel viewModel = new TranscriptsViewModel(transcripts);
               return View("Index", viewModel);
          }
          catch (Exception ex)
          {
               _logger.LogError(ex, $"{nameof(ErroredIndex)} session {session}");
               TranscriptsViewModel viewModel = new TranscriptsViewModel(ex.Message);
               return View("Index", viewModel);
          }
     }

     public IActionResult Privacy()
     {
          return View();
     }

     [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
     public IActionResult Error()
     {
          return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
     }
}

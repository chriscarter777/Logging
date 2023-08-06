using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Models;

public class TranscriptsViewModel
{
     public TranscriptsViewModel() { Transcripts = new List<Transcript>(); }

     public TranscriptsViewModel(List<Transcript> transcripts)
     {
          Transcripts = transcripts;
     }

     public TranscriptsViewModel(string message)
     {
          Message = "EXCEPTION: \"" + message + "\"";
          Transcripts = new List<Transcript>();
     }


     public string Message { get; set; }
     public List<Transcript> Transcripts { get; set; }
}

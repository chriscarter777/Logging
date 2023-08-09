using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Models;
public class BuildTranscriptRequest
{
     public BuildTranscriptRequest() { }

     public BuildTranscriptRequest(Guid session, int thisMany, bool throwException)
     {
          Session = session;
          ThisMany = thisMany;
          ThrowException = throwException;
     }


     public Guid Session { get; set; }
     public int ThisMany { get; set; }
     public bool ThrowException { get; set; }
}

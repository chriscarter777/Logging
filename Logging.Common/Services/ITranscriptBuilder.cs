using Logging.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Services
{
    public interface ITranscriptBuilder
     {
          Transcript Build(Guid session, bool throwException);

          List<Transcript> BuildMany(Guid session, int thisMany, bool throwException);
     }
}

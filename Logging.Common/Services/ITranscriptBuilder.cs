﻿using Logging.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Common.Services
{
    public interface ITranscriptBuilder
     {
          List<Transcript> BuildMany(BuildTranscriptRequest buildRequest);
     }
}

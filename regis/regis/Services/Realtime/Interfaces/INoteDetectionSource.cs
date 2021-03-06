﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using Regis.Models;
using Regis.Plugins.Models;

namespace Regis.Services.Realtime
{
    interface INoteDetectionSource
    {
        ConcurrentQueue<Note[]> NoteQueue { get; }
    }
}


﻿using DotNetCoreCryptographyCore.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotNetCoreCryptographyCore.Concrete
{
    /// <summary>
    /// Simple and stupid key value store that encrypt with AES using a folder
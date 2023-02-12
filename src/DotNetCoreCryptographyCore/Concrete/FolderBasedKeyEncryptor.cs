
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
    /// as key material storage. All keys are stored in the given location but
    /// protected with a password.
    /// </summary>
    public class FolderBasedKeyEncryptor : IKeyEncryptor
    {
        private readonly ConcurrentDictionary<int, EncryptionKey> _keys = new ConcurrentDictionary<int, EncryptionKey>();
        private EncryptionKey _currentKey;
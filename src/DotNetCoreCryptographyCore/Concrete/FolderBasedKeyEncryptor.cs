
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
        private readonly string _keyMaterialFolderStore;

        private readonly object _lock = new object();

        /// <summary>
        /// If different from empty, will be used to encrypt keys
        /// that are stored inside the shared folder.
        /// </summary>
        private readonly string _password;
        private readonly KeysDatabase _keyInformation;

        public FolderBasedKeyEncryptor(
            string keyMaterialFolderStore,
            string password)
        {
            _keyMaterialFolderStore = keyMaterialFolderStore;
            _password = password;
            InternalUtils.EnsureDirectory(_keyMaterialFolderStore);

            _keyInformation = LoadInfo();
            if (_keyInformation.ActualKeyNumber == 0)
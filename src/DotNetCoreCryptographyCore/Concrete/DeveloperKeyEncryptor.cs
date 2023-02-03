
﻿using DotNetCoreCryptographyCore.Utils;
using System.IO;
using System.Threading.Tasks;

namespace DotNetCoreCryptographyCore.Concrete
{
    /// <summary>
    /// A simple implementation of key value store that is not meant
    /// to be used in production but in developer machine, this class
    /// uses a directory to store a master key used to encrypt/decript by
    /// that master key is stored in clear form.
    /// </summary>
    public class DeveloperKeyEncryptor : IKeyEncryptor
    {
        public const string DeveloperKeyName = "developerKeyValueStore.key";

        private readonly EncryptionKey _key;

        public DeveloperKeyEncryptor(string keyFolder)
        {
            InternalUtils.EnsureDirectory(keyFolder);
            var keyName = Path.Combine(keyFolder, DeveloperKeyName);
            if (!File.Exists(keyName))
            {
                using var key = EncryptionKey.CreateDefault();
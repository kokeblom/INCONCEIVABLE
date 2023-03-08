
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
            {
                GenerateNewKey();
            }
            else
            {
                _currentKey = GetKey(_keyInformation.ActualKeyNumber);
            }
        }

        private bool KeysAreEncrpted => !string.IsNullOrEmpty(_password);

        private byte[] Encrypt(byte[] key)
        {
            if (!String.IsNullOrEmpty(_password))
            {
                //use a static shared password to encrypt the data
                return StaticEncryptor.AesEncryptWithPassword(key, _password);
            }

            //Key is unencrypted
            return key;
        }

        private byte[] Decrypt(byte[] key)
        {
            if (!String.IsNullOrEmpty(_password))
            {
                //use a static shared password to decrypt the data
                return StaticEncryptor.AesDecryptWithPassword(key, _password);
            }

            //Key is unencrypted
            return key;
        }

        private EncryptionKey GetKey(int keyNumber)
        {
            if (!_keys.TryGetValue(keyNumber, out var key))
            {
                var keyName = Path.Combine(_keyMaterialFolderStore, $"{keyNumber}.key");
                var encryptedSerializedKey = File.ReadAllBytes(keyName);
                var serializedKey = Decrypt(encryptedSerializedKey);
                key = EncryptionKey.CreateFromSerializedVersion(serializedKey);
                _keys[keyNumber] = key;
            }
            return key;
        }

        public async Task<EncryptionKey> DecryptAsync(byte[] encryptedKey)
        {
            using var sourceMs = new MemoryStream(encryptedKey);
            var buffer = new byte[4];
            sourceMs.Read(buffer, 0, 4);
            var decryptionKey = GetKey(BitConverter.ToInt32(buffer));
            using var destinationMs = new MemoryStream();
            await StaticEncryptor.DecryptAsync(sourceMs, destinationMs, decryptionKey).ConfigureAwait(false);
            return EncryptionKey.CreateFromSerializedVersion(destinationMs.ToArray());
        }

        public async Task<byte[]> EncryptAsync(EncryptionKey key)
        {
            using var destinationMs = new MemoryStream();
            destinationMs.Write(BitConverter.GetBytes(_keyInformation.ActualKeyNumber));

﻿using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using DotNetCoreCryptographyCore;
using System;
using System.Threading.Tasks;

namespace DotNetCoreCryptography.Azure
{
    public class AzureKeyVaultStoreKeyEncryptor : IKeyEncryptor
    {
        private readonly string _actualKeyName;
        private readonly KeyClient _keyClient;

        public AzureKeyVaultStoreKeyEncryptor(
            string keyValueStoreAddress,
            string actualKeyName)
        {
            _keyClient = new KeyClient(new Uri(keyValueStoreAddress), new DefaultAzureCredential());
            _actualKeyName = actualKeyName;
        }

        public async Task<EncryptionKey> DecryptAsync(byte[] encryptedKey)
        {
            var key = await _keyClient.GetKeyAsync(_actualKeyName);
            var cryptoClient = new CryptographyClient(keyId: key.Value.Id, credential: new DefaultAzureCredential());

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
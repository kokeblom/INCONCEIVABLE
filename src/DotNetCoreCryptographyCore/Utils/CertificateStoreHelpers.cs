
﻿using System.Security.Cryptography.X509Certificates;

namespace DotNetCoreCryptographyCore.Utils
{
    internal class CertificateStoreHelpers
    {
        public X509Certificate2? GetCertificateFromThumbprint(string thumbprint)
        {
            return GetCertificateFromStore(thumbprint, StoreLocation.LocalMachine) ??
                GetCertificateFromStore(thumbprint, StoreLocation.CurrentUser);
        }

        private static X509Certificate2? GetCertificateFromStore(string thumbprint, StoreLocation storeLocation)
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
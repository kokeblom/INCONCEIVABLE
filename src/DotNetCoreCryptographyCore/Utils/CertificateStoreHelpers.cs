
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

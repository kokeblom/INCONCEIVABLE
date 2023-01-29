
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
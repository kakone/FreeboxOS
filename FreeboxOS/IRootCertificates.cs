using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace FreeboxOS
{
    /// <summary>
    /// Freebox root certificates interface
    /// </summary>
    public interface IRootCertificates : IEnumerable<X509Certificate2>, IDisposable
    {
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Motivo.Certificates
{
    public class MotivoCertificateValidationService
    {
        public bool ValidateCertificate(X509Certificate clientCertificate)
        {
            return true;
        }
    }
}

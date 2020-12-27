﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ServiceCertValidator : X509CertificateValidator
    {
        /// <summary>
        /// Implementation of a custom certificate validation on the service side.
        /// Service should consider certificate valid if its issuer is the same as the issuer of the service.
        /// If validation fails, throw an exception with an adequate message.
        /// </summary>
        /// <param name="certificate"> certificate to be validate </param>
        public override void Validate(X509Certificate2 certificate)
        {
            string service = (Formatter.ParseName(WindowsIdentity.GetCurrent().Name)).ToLower(); //servis

            X509Certificate2 certificateOfService = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, service);

            if (!certificate.Issuer.Equals(certificateOfService.Subject))
            {
                throw new Exception("Cert is not from the valid issuer");
            }           
        }
    }
}

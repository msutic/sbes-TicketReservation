using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using Contracts;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
			string srvCertCN = "sbesserver";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
           
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                Console.WriteLine("Connection established.");
                Performance p = new Performance(0, "test", DateTime.Now, 4, 450);
                if (!proxy.AddPerformance(0, p))
                {
                    Console.WriteLine("Nisam dodao");
                    Console.ReadLine();
                }
                proxy.Dispose();
                proxy.Close();
            }

            Console.ReadKey();
        }
    }
}

using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name); //sbesserver

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/Receiver";
            ServiceHost host = new ServiceHost(typeof(WCFService));
            host.AddServiceEndpoint(typeof(IWCFService), binding, address);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            

            try
            {
                
                try
                {
                    Database.performances = Database.ReadPerformances();
                }
                catch 
                {
                    Console.WriteLine("ERROR - reading performances.");
                }

                try
                {
                    Database.reservations = Database.ReadReservations();
                }
                catch 
                {
                    Console.WriteLine("ERROR - reading reservations.");
                }

                try
                {
                    Database.users = Database.ReadUsers();
                }
                catch
                {
                    Console.WriteLine("ERROR - reading users.");
                }      

                try
                {
                    Database.Discount = Database.ReadDiscount();
                }
                catch
                {
                    Console.WriteLine("ERROR - reading discount.");
                }
                
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                Console.ReadLine();
            }
            finally
            {
                host.Close();
                try
                {
                    Database.WriteAllPerformances();
                }
                catch 
                {
                    Console.WriteLine("ERROR - writing performances.");
                }

                try
                {
                    Database.WriteAllUsers();
                }
                catch 
                {
                    Console.WriteLine("ERROR - writing users.");
                }

                try
                {
                    Database.WriteAllReservations();
                }
                catch 
                {
                    Console.WriteLine("ERROR - writing reservations.");
                }

                try
                {
                    Database.WriteDiscount();
                }
                catch
                {
                    Console.WriteLine("ERROR - writing discount.");
                }
            }

        }
    }
}

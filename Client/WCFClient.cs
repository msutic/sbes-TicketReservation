using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address):base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = (Formatter.ParseName(WindowsIdentity.GetCurrent().Name)).ToLower();

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator =  new ClientCertValidator();

            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public bool AddPerformance(Performance performance)
        {
            if(!factory.AddPerformance(performance))
            {
                Console.WriteLine("Performance with given key already exists.");
                return false;
            }
            
            Console.WriteLine($"Successfully added performance with id {performance.Id}");
            return true;
        }

        public void MakeReservation()
        {
            factory.MakeReservation();
            
        }

        public void ModifyDiscount(int discount)
        {
            factory.ModifyDiscount(discount);
        }

        public bool ModifyPerformance(Performance performance)
        {
            if(!factory.ModifyPerformance(performance))
            {
                Console.WriteLine($"Performance with id {performance.Id} doesn't exist.");
                return false;
            }
            
            Console.WriteLine($"Successfully modified performance with id {performance.Id}");
            return true;
        }

        public void PayReservation()
        {
            factory.PayReservation();
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool CheckIfExists(int key)
        {
            if (!factory.CheckIfExists(key))
            {
                Console.WriteLine($"Performance with id = {key} doesn't exist.");
                return false;
            }
            return true;
        }

        public void ListAllPerformances()
        {
            factory.ListAllPerformances();
        }

        //public List<Performance> ReadPerformances()
        //{
        //    return factory.ReadPerformances();
        //}
    }
}

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

        public bool AddPerformance(string name, DateTime date, int room, double price, out int idPerformance)
        {
            idPerformance = -1;
            if(factory.AddPerformance(name,date,room,price, out int id))
            {
                idPerformance = id;
                Console.WriteLine($"Successfully added new performance with id {id}.");
                return true;
            }         
            return false;
        }
        

        public void ModifyDiscount(int discount)
        {
            factory.ModifyDiscount(discount);
            Console.WriteLine($"Successfully modified discount.");
        }

        public bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            if(factory.ModifyPerformance(id,name,date,room,ticketPrice))
            {        
               Console.WriteLine($"Successfully modified performance with id {id}.");
               return true;
            }

            return false;
        }

        public bool PayReservationWithoutDiscount(int reservationsId, string clientUsername)
        {
            if(factory.PayReservationWithoutDiscount(reservationsId,clientUsername))
            {
                Console.WriteLine($"Successfully paied reservation with id {reservationsId}.");
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }

        public bool CheckIfPerformanceExists(int id)
        {
            if (!factory.CheckIfPerformanceExists(id))
            {
                Console.WriteLine($"Performance with id {id} doesn't exist.");
                return false;
            }
            return true;
        }

        public void ListAllPerformances()
        {
            factory.ListAllPerformances();
        }

        public void ListAllUsers()
        {
            factory.ListAllUsers();
        }

        public void ListAllReservations()
        {
            factory.ListAllReservations();
        }

        public bool MakeReservation(int performanceId, DateTime date, int ticketQuantity, string clientUsername, out int reservationId)
        {
            reservationId = -1;
            if(factory.MakeReservation(performanceId, date, ticketQuantity, clientUsername, out int id))
            {
                reservationId = id;
                Console.WriteLine($"Successfully made new reservation, for performance with id {performanceId}. New reservations id is {reservationId}.");
                return true;
            }
            return false;
        }

        public bool CheckIfReservationCanBePaied(int reservationsId, string clientUsername)
        {
            if(!factory.CheckIfReservationCanBePaied(reservationsId,clientUsername))
            {
                Console.WriteLine($"Reservation with id {reservationsId} can't be paied.");
                return false;
            }
            return true;
        }
    }
}

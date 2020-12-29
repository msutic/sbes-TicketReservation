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
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator =  new ClientCertValidator();

            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            try
            {
                if (factory.ModifyPerformance(id, name, date, room, ticketPrice))
                {
                    Console.WriteLine($"Successfully modified performance with id {id}.");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to modify performance: {e.Message}.");
            }
            return false;
        }

        public bool CheckIfPerformanceExists(int id)
        {
            try
            {
                if (!factory.CheckIfPerformanceExists(id))
                {
                    Console.WriteLine($"Performance with id {id} doesn't exist.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to check if performance exists {e.Message}.");
            }

            return true;
        }

        public bool AddPerformance(string name, DateTime date, int room, double price, out int idPerformance)
        {
            idPerformance = -1;
            try
            {
                if (factory.AddPerformance(name, date, room, price, out int id))
                {
                    idPerformance = id;
                    Console.WriteLine($"Successfully added new performance with id {id}.");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to add performance: {e.Message}.");
            }
            return false;
        }

        public void ModifyDiscount(int discount)
        {
            try
            {
                factory.ModifyDiscount(discount);
                Console.WriteLine($"Successfully modified discount.");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error while trying to modify discount {e.Message}.");
            }
        }

        public bool PayReservation(int reservationsId)
        {
            try
            {
                if (factory.PayReservation(reservationsId))
                {
                    Console.WriteLine($"Successfully paied reservation with id {reservationsId}.");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to pay reservation: {e.Message}.");
            }
            return false;
        }       

        public void ListAllPerformances()
        {
            try
            {
                factory.ListAllPerformances();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to list all performances: {e.Message}.");
            }
        }

        public void ListAllUsers()
        {
            try
            { 
                factory.ListAllUsers();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to list all users: {e.Message}.");
            }
        }

        public void ListAllReservations()
        {
            try
            { 
                factory.ListAllReservations();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to list all reservations: {e.Message}.");
            }
        }

        public bool MakeReservation(int performanceId, DateTime date, int ticketQuantity, out int reservationId)
        {
            reservationId = -1;
            try
            {
                if (factory.MakeReservation(performanceId, date, ticketQuantity, out int id))
                {
                    reservationId = id;
                    Console.WriteLine($"Successfully made new reservation, for performance with id {performanceId}. New reservations id is {reservationId}.");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to make reservation: ${e.Message}.");
            }
            return false;
        }

        public bool CheckIfReservationCanBePaied(int reservationsId)
        {
            try
            {
                if (!factory.CheckIfReservationCanBePaied(reservationsId))
                {
                    Console.WriteLine($"Reservation with id {reservationsId} can't be paied.");
                    return false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error while trying to check if reservation can be paied: {e.Message}.");
            }
            return true;
        }

        public void ListDiscount()
        {
            try
            { 
                factory.ListDiscount();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to list the discount: {e.Message}.");
            }
        }

        public void ListUser()
        {
            factory.ListUser();
        }
        
        public bool Validation(string methodName)
        {
            bool retValue = false;
            try
            {
                factory.Validation(methodName);
                retValue = true;
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Message);
            }
            return retValue;
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}

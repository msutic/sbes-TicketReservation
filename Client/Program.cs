using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using Contracts;
using System.Security.Principal;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
			string srvCertCN = "sbesserver";
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);

            //X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            //string[] parts = clientCert.SubjectName.Name.Split(',');
            //string clientGroup = parts[1].Split('=')[1];
            //Console.WriteLine(clientGroup);

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));

            int id;
            string name = "";
            string dateInput = "";
            DateTime date;
            int room;
            double price;
            int discount;
            int ticketQuantity;
            string[] tokens;
            int input = 0;

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                Console.WriteLine("Connection established.");
                
                do
                {
                    Console.WriteLine("\nMenu:\n\t1. Add Performance\n\t2. Modify Performance\n\t3. Modify Discount\n\t" +
                        "4. Make Reservation\n\t5. Pay Reservation\n\t6. Show all performances\n\t" +
                        "7. Show all users\n\t8. Show all reservations\n\t9. Exit\n======================\n");

                    input = int.Parse(Console.ReadLine());

                    switch (input)
                    {
                        case 1:
                            Console.Write("\nNew Performance\n\tname: ");
                            name = Console.ReadLine();
                            Console.Write("\tdate (format dd/mm/yyyy): ");
                            dateInput = Console.ReadLine();
                            tokens = dateInput.Split('/');
                            date = new DateTime(int.Parse(tokens[2]), int.Parse(tokens[1]), int.Parse(tokens[0]));
                            Console.Write("\troom: ");
                            room = int.Parse(Console.ReadLine());
                            Console.Write("\tticket price: ");
                            price = double.Parse(Console.ReadLine());
                            proxy.AddPerformance(name, date, room, price, out id);
                            break;

                        case 2:
                            Console.WriteLine("\nEnter id of the performance you want to modify: ");
                            id = int.Parse(Console.ReadLine());
                            if (!proxy.CheckIfPerformanceExists(id))
                                break;
                            Console.Write("\tname: ");
                            name = Console.ReadLine();
                            Console.Write("\tdate (format dd/mm/yyyy): ");
                            dateInput = Console.ReadLine();
                            tokens = dateInput.Split('/');
                            date = new DateTime(int.Parse(tokens[2]), int.Parse(tokens[1]), int.Parse(tokens[0]));
                            Console.Write("\troom: ");
                            room = int.Parse(Console.ReadLine());
                            Console.Write("\tticket price: ");
                            price = double.Parse(Console.ReadLine());
                            proxy.ModifyPerformance(id, name, date, room, price);
                            break;

                        case 3:
                            Console.Write("\nEnter new discount: ");
                            discount = int.Parse(Console.ReadLine());
                            proxy.ModifyDiscount(discount);
                            break;

                        case 4:
                            Console.WriteLine("\nEnter id of the performance you want to reserve: ");
                            id = int.Parse(Console.ReadLine());
                            if (!proxy.CheckIfPerformanceExists(id))
                                break;
                            Console.Write("\n\tticketQuantity: ");
                            ticketQuantity = int.Parse(Console.ReadLine());
                            proxy.MakeReservation(id, DateTime.Now, ticketQuantity, cltCertCN, out int reservationId);
                            break;
                        case 5:
                            Console.WriteLine("\nEnter id of the reservation you want to pay: ");
                            id = int.Parse(Console.ReadLine());
                            if (!proxy.CheckIfReservationCanBePaied(id,cltCertCN))
                                break;
                            proxy.PayReservationWithoutDiscount(id,cltCertCN);
                            break;
                        case 6:
                            proxy.ListAllPerformances();
                            break;
                        case 7:
                            proxy.ListAllUsers();
                            break;
                        case 8:
                            proxy.ListAllReservations();
                            break;
                        case 9:
                            break;
                    }
                }
                while (input != 9);

                proxy.Dispose();
                proxy.Close();

            }
        }
    }
}

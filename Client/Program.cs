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
                Performance p;
                int id;
                string name = "";
                string dateInput = "";
                DateTime date;
                int room;
                double price;
                int discount;
                string[] tokens;
                int input = 0;

                do
                {
                    Console.WriteLine("\nMenu:\n\t1. Add Performance\n\t2. Modify Performance\n\t3. Modify Discount\n\t" +
                        "4. Make Reservation\n\t5. Pay Reservation\n\t6. Show all performances\n======================\n");

                    input = int.Parse(Console.ReadLine());

                    switch (input)
                    {
                        case 1:
                            Console.Write("\nNew Performance\n\tid: ");
                            id = int.Parse(Console.ReadLine());
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
                            proxy.AddPerformance(new Performance(id, name, date, room, price));
                            break;

                        case 2:
                            Console.WriteLine("\nEnter id of the performance you want to modify: ");
                            id = int.Parse(Console.ReadLine());
                            if (!proxy.CheckIfExists(id))
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
                            proxy.ModifyPerformance(new Performance(id, name, date, room, price));
                            break;

                        case 3:
                            Console.Write("\nEnter new discount: ");
                            discount = int.Parse(Console.ReadLine());
                            proxy.ModifyDiscount(discount);
                            break;

                        case 4:
                            proxy.MakeReservation();
                            break;
                        case 5:
                            proxy.PayReservation();
                            break;
                        case 6:
                            proxy.ListAllPerformances();
                            break;
                        case 7:
                            break;
                    }
                }
                while (input != 7);

                proxy.Dispose();
                proxy.Close();

            }

            Console.ReadKey();
        }
    }
}

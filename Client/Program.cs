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

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);            

            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));

            int id;
            string name = "";
            string dateInput = "";
            DateTime date = DateTime.Now;
            int room = -1;
            double price = -1;
            int discount = -1;
            int ticketQuantity = -1;
            string[] tokens;
            int input = 0;

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                Console.WriteLine("Connection established.");
                do
                {
                    Console.WriteLine("\nMenu:\n\t1. Add Performance\n\t2. Modify Performance\n\t3. Modify Discount\n\t" +
                        "4. Make Reservation\n\t5. Pay Reservation\n\t6. Show all performances\n\t" +
                        "7. Show all users\n\t8. Show all reservations\n\t9. Show discount\n\t10. Show my informations\n\t" +
                        "11. Exit\n\t\n======================\n");
                    try
                    {
                        input = int.Parse(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Invalid input: {e.Message}.");
                    }

                    switch (input)
                    {
                        case 1:                            

                            Console.Write("\nNew Performance\n\tname: ");
                            try
                            {
                                name = Console.ReadLine();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\tdate (format dd/mm/yyyy): ");
                            try
                            {
                                dateInput = Console.ReadLine();
                                tokens = dateInput.Split('/');
                                date = new DateTime(int.Parse(tokens[2]), int.Parse(tokens[1]), int.Parse(tokens[0]));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\troom: ");
                            try
                            {
                                room = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\tticket price: ");
                            try
                            {
                                price = double.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            proxy.AddPerformance(name, date, room, price, out id);
                            break;

                        case 2:

                            Console.WriteLine("\nEnter id of the performance you want to modify: ");
                            try
                            {
                                id = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            if (!proxy.CheckIfPerformanceExists(id,2))
                                break;

                            Console.Write("\tname: ");
                            try
                            {
                                name = Console.ReadLine();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\tdate (format dd/mm/yyyy): ");
                            try
                            {
                                dateInput = Console.ReadLine();
                                tokens = dateInput.Split('/');
                                date = new DateTime(int.Parse(tokens[2]), int.Parse(tokens[1]), int.Parse(tokens[0]));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\troom: ");
                            try
                            {
                                room = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            Console.Write("\tticket price: ");
                            try
                            {
                                price = double.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            proxy.ModifyPerformance(id, name, date, room, price);
                            break;

                        case 3:

                            Console.Write("\nEnter new discount: ");
                            try
                            {
                                discount = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            proxy.ModifyDiscount(discount);
                            break;

                        case 4:

                            Console.WriteLine("\nEnter id of the performance you want to reserve: ");
                            try
                            {
                                id = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            if (!proxy.CheckIfPerformanceExists(id,4))
                                break;

                            Console.Write("\n\tticketQuantity: ");
                            try
                            {
                                ticketQuantity = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            proxy.MakeReservation(id, DateTime.Now, ticketQuantity, out int reservationId);
                            break;

                        case 5:

                            Console.WriteLine("\nEnter id of the reservation you want to pay: ");
                            try
                            {
                                id = int.Parse(Console.ReadLine());
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Input not valid {e.Message}. Please try again.");
                                break;
                            }

                            if (!proxy.CheckIfReservationCanBePaied(id))
                                break;

                            proxy.PayReservation(id);
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
                            proxy.ListDiscount();
                            break;

                        case 10:
                            proxy.ListUser();
                            break;

                        case 11:
                            break;

                        default:
                            Console.WriteLine("Entered invalid number. Valid 1-11. Please try again.");
                            break;
                    }
                }
                while (input != 11);

                proxy.Dispose();
                proxy.Close();
            }
        }
    }
}

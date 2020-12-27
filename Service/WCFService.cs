using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public static string clientCertSubjectName { get; set; }

        public string GetClientRole()
        {            
            string[] parts = clientCertSubjectName.Split(',');
            return parts[1].Split('=')[1];
        }

        public void ErrorMessage(string group, string method)
        {
            string[] tokens = clientCertSubjectName.Split(',');
            string name = tokens[0].Split('=')[1];
            DateTime time = DateTime.Now;
            string message = String.Format($"Access is denied. User {name} try to call {method} method (time : {time}). " +
                $"For this method user needs to be member of group {group}.");
            throw new FaultException<SecurityException>(new SecurityException(message));
        }

        public bool Validation(string methodName)
        {          
            if (GetClientRole().Equals("Admin"))
            {
                if (methodName.Equals("Add Performance") || methodName.Equals("Modify Performance") || methodName.Equals("Modify Discount"))
                { 
                   return true;
                }
                else
                {
                   ErrorMessage("SuperKorisnik/Korisnik", methodName);
                   return false;
                }
            }
           else
           {
                if (methodName.Equals("Make Reservation") || methodName.Equals("Pay Reservation"))
                {
                    return true;
                }
                else
                {
                    ErrorMessage("Admin", methodName);
                    return false;
                }
           }
        }

        public bool AddPerformance(string name, DateTime date, int room, double price, out int idPerformance)
        {
            idPerformance = -1;
            
            Console.WriteLine("Adding performance...");
            Performance performance = null;
            if (Database.performances.Count()>0)
            {
                performance = new Performance(Database.performances.Count(), name, date, room, price);
                idPerformance = Database.performances.Count();
            }
            else
            {
                performance = new Performance(0, name, date, room, price);
                idPerformance = 0;
            }
            Database.performances.Add(performance);
            return true;
        }

        public bool CheckIfPerformanceExists(int id)
        {
            foreach(Performance p in Database.performances)
            {
                if(p.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        public void ListAllPerformances()
        {
            foreach(Performance p in Database.performances)
            {
                Console.WriteLine(p.ToString());
            }
        }

        public void ListAllUsers()
        {
            foreach (User u in Database.users)
            {
                Console.WriteLine(u.ToString());
            }
        }

        public void ListAllReservations()
        {
            foreach (Reservation r in Database.reservations)
            {
                Console.WriteLine(r.ToString());
            }
        }

        public bool MakeReservation(int performanceId, DateTime date, int ticketQuantity, out int reservationId)
        {
            reservationId = -1;
            Console.WriteLine("Making reservation...");
            Reservation reservation = null;
            string[] tokens = clientCertSubjectName.Split(',');
            string clientUsername = tokens[0].Split('=')[1];

            for (int i=0;i <Database.users.Count(); ++i)
            {
                if (Database.users[i].Username.Equals(clientUsername))
                {
                    if (Database.reservations.Count()>0)
                    {
                        reservation = new Reservation(Database.reservations.Count(), performanceId, date, ticketQuantity);
                        reservationId = Database.reservations.Count();
                    }
                    else
                    {
                        reservation = new Reservation(0, performanceId, date, ticketQuantity);
                        reservationId = 0;
                    }
                    Database.users[i].Reservations.Add(reservation);
                }
            }

            Database.reservations.Add(reservation);
            return true;
        }

        public void ModifyDiscount(int discount)
        {
            Console.WriteLine("Modifying discount...");
            Database.Discount = discount;
        }

        public bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            Console.WriteLine("Modifying performance...");

            for(int i=0; i<Database.performances.Count(); ++i)
            {
                if (id == Database.performances[i].Id)
                {
                    Database.performances[i].Name = name;
                    Database.performances[i].Date = date;
                    Database.performances[i].Room = room;
                    Database.performances[i].TicketPrice = ticketPrice;
                }
            }
            
            return true;
        }

        public bool PayReservationWithoutDiscount(int reservationsId)
        {
            string[] tokens = clientCertSubjectName.Split(',');
            string clientUsername = tokens[0].Split('=')[1];
            Console.WriteLine("Paying reservation...");
            foreach (User u in Database.users)
            {
                if (u.Username.Equals(clientUsername))
                {
                    foreach (Reservation r in u.Reservations)
                    {
                        foreach (Performance p in Database.performances)
                        {
                            if (p.Id.Equals(r.PerformanceId))
                            {
                                if (r.Id.Equals(reservationsId))
                                {
                                    u.Balance -= r.TicketQuantity * p.TicketPrice;
                                    for (int i = 0; i < u.Reservations.Count(); i++)
                                    {
                                        if (u.Reservations[i].Id.Equals(reservationsId))
                                        {
                                            u.Reservations[i].State = ReservationState.PAID;
                                            return true;
                                        }
                                    }
                                }
                            }
                                
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckIfReservationCanBePaied(int reservationsId)
        {
            string[] tokens = clientCertSubjectName.Split(',');
            string clientUsername = tokens[0].Split('=')[1];
            foreach (User u in Database.users)
            {
                if (u.Username.Equals(clientUsername))
                {
                    foreach (Reservation r in u.Reservations)
                    {
                        if (r.Id.Equals(reservationsId))
                        {
                           foreach (Performance p in Database.performances)
                           {
                                if (p.Id.Equals(r.PerformanceId))
                                {
                                    if (r.State.Equals(ReservationState.UNPAID))
                                    {
                                        if (u.Balance >= r.TicketQuantity * p.TicketPrice)
                                        {
                                            return true;
                                        }
                                    }
                                }                                
                            }                                                   
                        }
                    }
                }
            }
            return false;
        }

        public void SendMySubjectName(string subjectName)
        {
            clientCertSubjectName = subjectName;
        }
    }
}

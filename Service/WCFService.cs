using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public bool AddPerformance(Performance performance)
        {
            Console.WriteLine("Adding performance...");
            if (CheckIfPerformanceExists(performance.Id))
            {
                return false;
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

        public bool MakeReservation(Reservation reservation, string clientUsername)
        {
            Console.WriteLine("Making reservation...");
            if (!CheckIfPerformanceExists(reservation.PerformanceId)) {
                Console.WriteLine($"Performance with {reservation.PerformanceId} doesn't exist.");
                return false;
            }

            for(int i=0;i <Database.users.Count(); ++i)
            {
                if (Database.users[i].Username.Equals(clientUsername))
                {
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

        public void PayReservation()
        {
            Console.WriteLine("Paying reservation...");
            
        }
    }
}

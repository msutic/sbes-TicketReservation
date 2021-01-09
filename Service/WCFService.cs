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
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public string GetClientRole()
        {
            string retValue = "";
            try
            {
                string name = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
                string[] clientName = name.Split(';');
                string[] parts = clientName[0].Split(',');
                string[] roleName = parts[1].Split('=');
                retValue = roleName[1];
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nError while trying to get the client role: {e.Message}.");
            }
            return retValue;
        }

        public string GetClientUserName()
        {
            string retValue = "";
            try
            {
                string nameClient = Formatter.ParseName(ServiceSecurityContext.Current.PrimaryIdentity.Name);
                string[] clientName = nameClient.Split(';');
                string[] tokens = clientName[0].Split(',');
                retValue = tokens[0].Split('=')[1];
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nError while trying to get the client name: {e.Message}.");
            }
            return retValue;
        }

        public void ErrorMessage(string group, string method)
        {
            string userName = GetClientUserName();
            DateTime time = DateTime.Now;
            string message = String.Format($"Access is denied. User {userName} try to call {method} method (time : {time}). " +
                     $"For this method user needs to be member of group {group}");
            SecurityException exception = new SecurityException(message);
            throw new FaultException<SecurityException>(exception, new FaultReason(exception.Message));
        }

        public void AddPerformance(string name, DateTime date, int room, double price, out int idPerformance)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            idPerformance = -1;

            if (principal.IsInRole("Admin"))
            {
                string userName = GetClientUserName();

                Console.WriteLine("\nAdding performance...");
                Performance performance = null;
                if (Database.performances.Count() > 0)
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

                try
                {
                    Audit.AddToBaseSuccess(userName, "performance.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Database.WritePerformances();
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Add Performance",
                        $"Add Performance can be used only by user in the Admin group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Admin", "Add Performance");
            }
        }

        public bool CheckIfPerformanceExists(int id, int methodID)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (methodID == 2)
            {
                if (principal.IsInRole("Admin"))
                {
                    foreach (Performance p in Database.performances)
                    {
                        if (p.Id.Equals(id))
                        {
                            return true;
                        }
                    }

                    try
                    {
                        Audit.MethodCallFailed(GetClientUserName(), "Modify Performance",
                            $"User did not enter a valid id of perfrormance.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    return false;
                }
                else
                {
                    try
                    {
                        Audit.AuthorizationFailed(GetClientUserName(), "Modify Performance",
                            $"Modify performance can be used only by user in the Admin group.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    ErrorMessage("Admin", "Modify Performance");
                    return false;
                }
            }
            else
            {
                if (principal.IsInRole("Korisnik") || principal.IsInRole("SuperKorisnik"))
                {
                    foreach (Performance p in Database.performances)
                    {
                        if (p.Id.Equals(id))
                        {
                            return true;
                        }
                    }

                    try
                    {
                        Audit.MethodCallFailed(GetClientUserName(), "Make Reservation",
                            $"User did not enter a valid id of perfrormance.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    return false;
                }
                else
                {
                    try
                    {
                        Audit.AuthorizationFailed(GetClientUserName(), "Make Reservation",
                            $"Make Reservation can be used only by user in the Korisnik or SuperKorisnik group.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    ErrorMessage("Korisnik or SuperKorisnik", "Make Reservation");
                    return false;
                }

            }
        }

        public void ListAllPerformances()
        {
            Console.WriteLine();
            Console.WriteLine("List of performances in the system:");

            foreach (Performance p in Database.performances)
            {
                Console.WriteLine(p.ToString());
            }

            try
            {
                Audit.AuthorizationSuccess(GetClientUserName(),
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void ListAllUsers()
        {
            Console.WriteLine();
            Console.WriteLine("List of all users in the system:");
            foreach (User u in Database.users)
            {
                Console.WriteLine(u.ToString());
            }

            try
            {
                Audit.AuthorizationSuccess(GetClientUserName(),
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ListAllReservations()
        {
            Console.WriteLine();
            Console.WriteLine("List of reservations in the system:");
            foreach (Reservation r in Database.reservations)
            {
                Console.WriteLine(r.ToString());
            }

            try
            {
                Audit.AuthorizationSuccess(GetClientUserName(),
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void MakeReservation(int performanceId, DateTime date, int ticketQuantity, out int reservationId)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            reservationId = -1;

            if (principal.IsInRole("Korisnik") || principal.IsInRole("SuperKorisnik"))
            {
                Console.WriteLine("\nMaking reservation...");
                Reservation reservation = null;

                string clientUsername = GetClientUserName();

                for (int i = 0; i < Database.users.Count(); ++i)
                {
                    if (Database.users[i].Username.Equals(clientUsername))
                    {
                        if (Database.reservations.Count() > 0)
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

                        try
                        {
                            Audit.AddToBaseSuccess(clientUsername, "reservation.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                }
                Database.reservations.Add(reservation);
                Database.WriteReservations();
                Database.WriteUsers();
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Make Reservation",
                        $"Make Reservation can be used only by user in the Korisnik or SuperKorisnik group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Korisnik or SuperKorisnik", "Make Reservation");
            }
        }

        public void ModifyDiscount(int discount)
        {
            string clientUsername = GetClientUserName();
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole("Admin"))
            {
                Console.WriteLine("\nModifying discount...");

                Database.Discount = discount;
                Database.WriteDiscount();

                try
                {
                    Audit.ChangeSuccess(clientUsername, "discount.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Modify Discount",
                        $"Modify Discount can be used only by user in the Admin group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Admin", "Modify Discount");
            }
        }

        public void ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole("Admin"))
            {

                Console.WriteLine("\nModifying performance...");

                for (int i = 0; i < Database.performances.Count(); ++i)
                {
                    if (id.Equals(Database.performances[i].Id))
                    {
                        Database.performances[i].Name = name;
                        Database.performances[i].Date = date;
                        Database.performances[i].Room = room;
                        Database.performances[i].TicketPrice = ticketPrice;
                    }
                }

                try
                {
                    Audit.ChangeSuccess(GetClientUserName(), "performance.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Database.WritePerformances();
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Modify Performance",
                        $"Modify Performance can be used only by user in the Admin group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Admin", "Modify Performance");
            }
        }

        public void PayReservation(int reservationsId)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole("Korisnik") || principal.IsInRole("SuperKorisnik"))
            {
                string clientUsername = GetClientUserName();
                string clientRole = GetClientRole();
                Console.WriteLine("\nPaying reservation...");
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
                                        if (clientRole.Equals("Korisnik"))
                                        {
                                            u.Balance -= r.TicketQuantity * p.TicketPrice;
                                            try
                                            {
                                                Audit.ChangeSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "balance.");
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                            }
                                        }
                                        else
                                        {
                                            u.Balance -= r.TicketQuantity * p.TicketPrice - (r.TicketQuantity * p.TicketPrice) * (Database.Discount / 100);
                                            try
                                            {
                                                Audit.ChangeSuccess(Formatter.ParseName(WindowsIdentity.GetCurrent().Name), "balance.");
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.Message);
                                            }
                                        }

                                        for (int i = 0; i < u.Reservations.Count(); i++)
                                        {
                                            if (u.Reservations[i].Id.Equals(reservationsId))
                                            {
                                                u.Reservations[i].State = ReservationState.PAID;

                                                try
                                                {
                                                    Audit.PayReservationSuccess(clientUsername, ReservationState.UNPAID.ToString(), ReservationState.PAID.ToString());
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.Message);
                                                }

                                                Database.WriteReservations();
                                                Database.WriteUsers();
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Pay Reservation",
                        $"Pay Reservation can be used only by user in the Korisnik or SuperKorisnik group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Korisnik or SuperKorisnik", "Pay Reservation");
            }
        }

        public bool CheckIfReservationCanBePaied(int reservationsId)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

            if (principal.IsInRole("Korisnik") || principal.IsInRole("SuperKorisnik"))
            {
                string clientUsername = GetClientUserName();
                string clientRole = GetClientRole();
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
                                            if (clientRole.Equals("Korisnik"))
                                            {
                                                if (u.Balance >= r.TicketQuantity * p.TicketPrice)
                                                {
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                if (u.Balance >= r.TicketQuantity * p.TicketPrice - (r.TicketQuantity * p.TicketPrice) * (Database.Discount / 100))
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
                }

                try
                {
                    Audit.MethodCallFailed(GetClientUserName(), "Pay Reservation",
                        $"User did not enter a valid id of reservation.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return false;
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(GetClientUserName(), "Pay Reservation",
                        $"Pay Reservation can be used only by user in the Korisnik or SuperKorisnik group.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                ErrorMessage("Korisnik or SuperKorisnik", "Pay Reservation");
                return false;
            }
        }       

        public void ListDiscount()
        {
            Console.WriteLine($"\nCurrent discount is {Database.Discount}.");

            try
            {
                Audit.AuthorizationSuccess(GetClientUserName(),
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ListUser()
        {
            string clientUsername = GetClientUserName();

            Console.WriteLine($"\nMy informations: {Database.users.First(item => item.Username == clientUsername).ToString()}");

            try
            {
                Audit.AuthorizationSuccess(clientUsername,
                    OperationContext.Current.IncomingMessageHeaders.Action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

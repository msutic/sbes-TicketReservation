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
            if (CheckIfExists(performance.Id))
            {
                return false;
            }

            Database.performances.Add(performance);
            return true;
        }

        public bool CheckIfExists(int id)
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
            Console.WriteLine("ulazim");
            foreach(Performance p in Database.performances)
            {
                Console.WriteLine(p.ToString());
            }
        }

        public void MakeReservation()
        {
            Console.WriteLine("Making reservation...");
        }

        public void ModifyDiscount(int discount)
        {
            Console.WriteLine("Modifying discount...");
            Database.Discount = discount;
        }

        public bool ModifyPerformance(Performance performance)
        {
            Console.WriteLine("Modifying performance...");
            if (CheckIfExists(performance.Id))
            {
                return false;
            } 

            for(int i =0;i<Database.performances.Count(); ++i)
            {
                if (performance.Id == Database.performances[i].Id)
                    Database.performances[i] = performance;
            }
            
            return true;
        }

        public void PayReservation()
        {
            Console.WriteLine("Paying reservation...");
            
        }

        //public List<Performance> ReadPerformances()
        //{
        //    string path = @"D:\novo_git\sbes-TicketReservation\performances.txt";

        //    List<Performance> performances = new List<Performance>();
        //    FileStream stream = new FileStream(path, FileMode.Open);
        //    StreamReader reader = new StreamReader(stream);

        //    string line = "";
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        string[] tokens = line.Split(';');
        //        Console.WriteLine("evo usao u while");
        //        string[] dateTokens = tokens[2].Split('/');
        //        DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));

        //        Performance p = new Performance(int.Parse(tokens[0]), tokens[1], date, int.Parse(tokens[3]), double.Parse(tokens[4]));

        //        performances.Add(p);
        //    }

        //    reader.Close();
        //    stream.Close();

        //    return performances;
        //}
    }
}

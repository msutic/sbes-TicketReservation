using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Database
    {
        private static string path1 = @"..\..\items\performances.txt";
        private static string fullPath = Path.GetFullPath(path1);

        public static List<Performance> performances = ReadPerformances();
        //public static List<User> users = FileOperations.ReadUsers();
        //public static List<Reservation> reservations = FileOperations.ReadReservations();
        public static int Discount { get; set; }



        public static List<Performance> ReadPerformances()
        {
            List<Performance> performances = new List<Performance>();
            FileStream stream = new FileStream(fullPath, FileMode.Open);
            StreamReader reader = new StreamReader(stream);

            string line = "";
            while ((line = reader.ReadLine()) != null)
            {
                string[] tokens = line.Split(';');
                Console.WriteLine("evo usao u while");
                string[] dateTokens = tokens[2].Split('/');
                DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));

                Performance p = new Performance(int.Parse(tokens[0]), tokens[1], date, int.Parse(tokens[3]), double.Parse(tokens[4]));

                performances.Add(p);
            }

            reader.Close();
            stream.Close();

            return performances;
        }
    }
}

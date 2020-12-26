//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

//namespace Contracts
//{
    
//    public class FileOperations
//    {
        
//        public static List<User> ReadUsers()
//        {
//            string path = @"~/items/UserBase.txt";
//            List<User> users = new List<User>();
//            FileStream stream = new FileStream(path, FileMode.Open);
//            StreamReader reader = new StreamReader(stream);
//            string line = "";
//            while((line = reader.ReadLine()) != null)
//            {
//                string[] tokens = line.Split(';');

//                List<Reservation> reservations = ReadReservations();
//                List<Reservation> userReservations = new List<Reservation>();
//                var count = tokens[3].Count(x => x == ',');
//                if(count != 0)
//                {
//                    string[] idRes = tokens[3].Split(',');
//                    for(int i = 0; i < count; i++)
//                    {
//                        foreach (Reservation res in reservations)
//                        {
//                            if (int.Parse(idRes[i]) == res.Id)
//                            {
//                                userReservations.Add(res);
//                            }
//                        }
//                    }
//                }
//                User user = new User(tokens[0], tokens[1], int.Parse(tokens[2]), userReservations);
//                users.Add(user);
//            }

//            reader.Close();
//            stream.Close();

//            return users;
//        }
        
//        public static List<Reservation> ReadReservations()
//        {
//            string path = @"~/items/reservations";
//            List<Reservation> reservations = new List<Reservation>();
//            FileStream stream = new FileStream(path, FileMode.Open);
//            StreamReader reader = new StreamReader(stream);
//            string line = "";
//            while ((line = reader.ReadLine()) != null)
//            {
//                string[] tokens = line.Split(';');

//                string[] dateTokens = tokens[2].Split('/');
//                DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));
                
//                Reservation r = new Reservation(int.Parse(tokens[0]), int.Parse(tokens[1]), date, int.Parse(tokens[3]));
//                r.State = (ReservationState)Enum.Parse(typeof(ReservationState), tokens[4]);
//                reservations.Add(r);
//            }

//            reader.Close();
//            stream.Close();

//            return reservations;
//        }
        
//        public static List<Performance> ReadPerformances()
//        {
//            string path = @"D:\novo_git\sbes-TicketReservation\performances.txt";
            
//            List<Performance> performances = new List<Performance>();
//            FileStream stream = new FileStream(path, FileMode.Open);
//            StreamReader reader = new StreamReader(stream);

//            string line = "";
//            while ((line = reader.ReadLine()) != null)
//            {
//                string[] tokens = line.Split(';');
//                Console.WriteLine("evo usao u while");
//                string[] dateTokens = tokens[2].Split('/');
//                DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));

//                Performance p = new Performance(int.Parse(tokens[0]), tokens[1], date, int.Parse(tokens[3]), double.Parse(tokens[4]));

//                performances.Add(p);
//            }

//            reader.Close();
//            stream.Close();

//            return performances;
//        }
//    }
//}

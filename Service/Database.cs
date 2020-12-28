using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Database
    {
        private static string path = @"..\..\items\";

        public static List<Performance> performances;
        public static List<User> users;
        public static List<Reservation> reservations;
        public static double Discount { get; set; }

        public static void ReadPerformances()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "performances.txt");

                FileStream stream = new FileStream(fullPath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(';');
                    string[] dateTokens = tokens[2].Split('/');
                    DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));

                    Performance p = new Performance(int.Parse(tokens[0]), tokens[1], date, int.Parse(tokens[3]), double.Parse(tokens[4]));
                    performances.Add(p);
                }

                reader.Close();
                stream.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error while trying to read performances : {e.Message}");
            }
        }

        public static void ReadDiscount()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "discount.txt");

                FileStream stream = new FileStream(fullPath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);

                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    Discount = int.Parse(line);
                }

                reader.Close();
                stream.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to read discount : {e.Message}");
            }
        }

        public static void ReadUsers()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "users.txt");

                FileStream stream = new FileStream(fullPath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);

                string line = "";

                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(';');

                    List<Reservation> userReservations = new List<Reservation>();
                    User user = null;
                    int count = tokens[3].Count(x => x == ',');
                    if (count != 0)
                    {
                        string[] idRes = tokens[3].Split(',');
                        for (int i = 0; i < count; i++)
                        {
                            foreach (Reservation res in reservations)
                            {
                                if (int.Parse(idRes[i]) == res.Id)
                                {
                                    userReservations.Add(res);
                                }
                            }
                        }
                        user = new User(tokens[0], tokens[1], double.Parse(tokens[2]), userReservations);
                    }
                    else
                    {
                        user = new User(tokens[0], tokens[1]);
                    }
                    users.Add(user);
                }

                reader.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to read users : {e.Message}");
            }
        }

        public static void ReadReservations()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "reservations.txt");

                FileStream stream = new FileStream(fullPath, FileMode.Open);
                StreamReader reader = new StreamReader(stream);

                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] tokens = line.Split(';');

                    string[] dateTokens = tokens[2].Split('/');
                    DateTime date = new DateTime(int.Parse(dateTokens[2]), int.Parse(dateTokens[1]), int.Parse(dateTokens[0]));

                    Reservation r = new Reservation(int.Parse(tokens[0]), int.Parse(tokens[1]), date, int.Parse(tokens[3]));
                    r.State = (ReservationState)Enum.Parse(typeof(ReservationState), tokens[4]);
                    reservations.Add(r);
                }

                reader.Close();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to read performances : {e.Message}");
            }
        }

        public static void WritePerformance(Performance p)
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "performances.txt");
                File.AppendAllText(fullPath, p.Write());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write performance with id {p.Id}, error: : {e.Message}");
            }
        }

        public static void WriteAllPerformances()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "performances.txt");
                File.WriteAllText(fullPath, String.Empty);

                foreach (Performance p in performances)
                {
                    WritePerformance(p);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write all performances : {e.Message}");
            }
        }

        public static void WriteUser(User u)
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "users.txt");
                File.AppendAllText(fullPath, u.Write());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write user with username {u.Username}, error: : {e.Message}");
            }
        }

        public static void WriteAllUsers()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "users.txt");
                File.WriteAllText(fullPath, String.Empty);

                foreach (User u in users)
                {
                    WriteUser(u);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write all users : {e.Message}");
            }
        }

        public static void WriteDiscount()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "discount.txt");
                File.WriteAllText(fullPath, String.Empty);
                File.AppendAllText(fullPath, Discount.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write discount : {e.Message}");
            }
        }

        public static void WriteReservation(Reservation r)
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "reservations.txt");
                File.AppendAllText(fullPath, r.Write());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write reservation with id {r.Id}, error: : {e.Message}");
            }
        }

        public static void WriteAllReservations()
        {
            try
            {
                string fullPath = Path.GetFullPath(path + "reservations.txt");
                File.WriteAllText(fullPath, String.Empty);

                foreach (Reservation r in reservations)
                {
                    WriteReservation(r);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while trying to write all performances : {e.Message}");
            }
        }
       
    }
}

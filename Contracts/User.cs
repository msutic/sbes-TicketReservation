using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Balance { get; set; }
        public List<Reservation> Reservations { get; set; }

        public User()
        {

        }

        public User(string username, string password, int balance, List<Reservation> reservations)
        {
            Username = username;
            Password = password;
            Balance = balance;
            Reservations = reservations;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int Balance { get; set; }
        [DataMember]
        public List<Reservation> Reservations { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Balance = -1;
        }

        public User(string username, string password, int balance, List<Reservation> reservations)
        {
            Username = username;
            Password = password;
            Balance = balance;
            Reservations = reservations;
        }

        public string Write()
        {
            if (Balance!=-1)
            {
                if (Reservations.Count()>0)
                {
                    string s = $"{Username};{Password};{Balance.ToString()};";
                    foreach (Reservation reservation in Reservations)
                    {
                        s += $"{reservation.Id},";
                    }
                    s += $";{Environment.NewLine}";
                    return s;
                }
                else
                {
                    return $"{Username};{Password};{Balance.ToString()};;{Environment.NewLine}";

                }
            }
            else
            {
                return $"{Username};{Password};{Balance.ToString()};;{Environment.NewLine}";
            }
        }

        public override string ToString()
        {
            if (Balance != -1)
            {
                if (Reservations.Count() > 0)
                {
                    string s = $"{Username};{Password};{Balance};";
                    foreach (Reservation reservation in Reservations)
                    {
                        s += $"{reservation.Id},;";
                    }
                    s += $"{Environment.NewLine}";
                    return s;
                }
                else
                {
                    return $"{Username};{Password};{Balance};;{Environment.NewLine}";
                }
            }
            else
            {
                return $"{Username};{Password};{Balance};;{Environment.NewLine}";
            }
        }
    }
}

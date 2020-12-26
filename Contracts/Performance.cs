using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class Performance
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Room { get; set; }
        [DataMember]
        public double TicketPrice { get; set; }

        public Performance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            Id = id;
            Name = name;
            Date = date;
            Room = room;
            TicketPrice = ticketPrice;
        }

        public override string ToString()
        {
            return $"Performance:\n\tid: {Id},\n\tname: {Name},\n\tdate: {Date.ToString("dd/MM/yyyy")},\n\troom: {Room},\n\tprice: {TicketPrice}\n\n";
        }
    }
}

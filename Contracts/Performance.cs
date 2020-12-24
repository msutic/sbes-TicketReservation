using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Performance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Room { get; set; }
        public double TicketPrice { get; set; }

        public Performance()
        {

        }

        public Performance(int id, string name, DateTime date, int room, double ticketPrice)
        {
            Id = id;
            Name = name;
            Date = date;
            Room = room;
            TicketPrice = ticketPrice;
        }
    }
}

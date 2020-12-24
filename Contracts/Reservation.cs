using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Reservation
    {
        public int Id { get; set; }
        public int PerformanceId { get; set; }
        public DateTime Date { get; set; }
        public int TicketQuantity { get; set; }
        public ReservationState State { get; set; }

        public Reservation()
        {
            Id = 0;
            PerformanceId = 0;
            Date = DateTime.Now;
            TicketQuantity = 0;
            State = ReservationState.UNPAID;
        }

        public Reservation(int id, int performanceId, DateTime date, int ticketQuantity)
        {
            Id = id;
            PerformanceId = performanceId;
            Date = date;
            TicketQuantity = ticketQuantity;
            State = ReservationState.UNPAID;
        }

    }
}

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

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [DataContract]
    public class Reservation
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int PerformanceId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int TicketQuantity { get; set; }
        [DataMember]
        public ReservationState State { get; set; }

        public Reservation()
        {
            State = ReservationState.UNPAID;
        }

        public Reservation(int id, int perfId, DateTime date, int ticketQuantity)
        {
            Id = id;
            PerformanceId = perfId;
            Date = date;
            TicketQuantity = ticketQuantity;
            State = ReservationState.UNPAID;
        }

        public string Write()
        {
            return $"{Id.ToString()};{PerformanceId.ToString()};{Date.Day.ToString()}/{Date.Month.ToString()}/{Date.Year.ToString()};{TicketQuantity.ToString()};{State.ToString()};{Environment.NewLine}";
        }

        public override string ToString()
        {
                return $"{Id.ToString()};{PerformanceId.ToString()};{Date.Day.ToString()}/{Date.Month.ToString()}/{Date.Year.ToString()};{TicketQuantity.ToString()};{State.ToString()};{Environment.NewLine}";
            
        }
    }
}

using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class WCFService : IWCFService
    {
        public void AddPerformance(int key, Performance performance)
        {
            if (!Database.performances.ContainsKey(key))
            {
                Database.performances.Add(key, performance);
            }
            else
            {
                //throw exception
            }
        }

        public void MakeReservation()
        {
            Reservation reservation = new Reservation();
        }

        public void ModifyDiscount()
        {
            throw new NotImplementedException();
        }

        public void ModifyPerformance(int key, Performance performance)
        {
            if (Database.performances.ContainsKey(key))
            {
                Database.performances[key] = performance;
            }
            else
            {
                //throw exception
            }
        }

        public void PayReservation(Reservation reservation)
        {
            if (reservation.State.Equals(ReservationState.UNPAID))
            {

            }
        }
    }
}

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
        public bool AddPerformance(int key, Performance performance)
        {
            try
            {
                Console.WriteLine("Adding Performance...");
            }
            catch (Exception e)
            {
                Console.WriteLine("servis: " + e.Message);
                Console.ReadLine();
            }
            
            return true;
        }

        public void MakeReservation()
        {
            throw new NotImplementedException();
        }

        public void ModifyDiscount()
        {
            throw new NotImplementedException();
        }

        public void ModifyPerformance()
        {
            throw new NotImplementedException();
        }

        public void PayReservation()
        {
            throw new NotImplementedException();
        }
    }
}

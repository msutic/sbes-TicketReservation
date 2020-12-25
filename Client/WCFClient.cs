using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class WCFClient : ChannelFactory<IWCFService>, IWCFService, IDisposable
    {
        IWCFService factory;
        public WCFClient(NetTcpBinding binding, EndpointAddress address):base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public bool AddPerformance(int key, Performance performance)
        {
            bool retVal = false;

            if(retVal = factory.AddPerformance(0, performance))
                Console.WriteLine("Performance Added");
            

            return retVal;
        }

        public void MakeReservation()
        {
            throw new NotImplementedException();
        }

        public void ModifyDiscount()
        {
            throw new NotImplementedException();
        }

        public bool ModifyPerformance(int key, Performance performance)
        {
            bool retVal = false;

            if(retVal = factory.ModifyPerformance(key, performance))
                Console.WriteLine($"Performance with key {key} modified.");


            return retVal;
        }

        public void PayReservation(Reservation reservation)
        {
            throw new NotImplementedException();
        }
    }
}

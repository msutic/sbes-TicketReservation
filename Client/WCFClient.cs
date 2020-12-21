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

        public void AddPerformance()
        {
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

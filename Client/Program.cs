using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/WCFService";

            using(WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
            {
                Console.WriteLine("Connection established.");
                Performance p = new Performance(0, "test", DateTime.Now, 4, 450);
                proxy.AddPerformance(0, p);
            }

            Console.ReadKey();
        }
    }
}

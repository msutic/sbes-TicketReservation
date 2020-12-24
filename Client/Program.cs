using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

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
            }

            Console.ReadKey();
        }
    }
}

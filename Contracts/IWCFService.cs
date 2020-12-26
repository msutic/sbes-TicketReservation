using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        bool AddPerformance(Performance performance);
        [OperationContract]
        bool ModifyPerformance(Performance performance);
        [OperationContract]
        void ModifyDiscount(int discount);
        [OperationContract]
        void MakeReservation();
        [OperationContract]
        void PayReservation();
        [OperationContract]
        bool CheckIfExists(int key);
        [OperationContract]
        void ListAllPerformances();
        //[OperationContract]
        //List<Performance> ReadPerformances();
    }
}

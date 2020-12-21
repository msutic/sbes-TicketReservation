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
        void AddPerformance();
        [OperationContract]
        void ModifyPerformance();
        [OperationContract]
        void ModifyDiscount();
        [OperationContract]
        void MakeReservation();
        [OperationContract]
        void PayReservation();
    }
}

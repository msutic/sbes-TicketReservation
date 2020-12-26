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
        bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice);
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
        [OperationContract]
        void ListAllUsers();
        [OperationContract]
        void ListAllReservations();
    }
}

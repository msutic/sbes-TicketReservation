using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract]
    public interface IWCFService
    {
        [OperationContract]
        bool AddPerformance(string name,DateTime date,int room,double price, out int id);
        [OperationContract]
        bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice);
        [OperationContract]
        void ModifyDiscount(int discount);
        [OperationContract]
        bool MakeReservation(int performanceId, DateTime date, int ticketQuantity, out int reservationId);
        [OperationContract]
        bool CheckIfReservationCanBePaied(int reservationsId);
        [OperationContract]
        bool PayReservation(int reservationsId);
        [OperationContract]
        bool CheckIfPerformanceExists(int key);
        [OperationContract]
        void ListAllPerformances();
        [OperationContract]
        void ListAllUsers();
        [OperationContract]
        void ListAllReservations();
        [OperationContract]
        void ListDiscount();
        [OperationContract]
        void ListUser();
        [FaultContract(typeof(SecurityException))]
        [OperationContract]
        bool Validation(string methodName);
    }
}

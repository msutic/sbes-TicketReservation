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
        bool AddPerformance(string name,DateTime date,int room,double price, out int id);
        [OperationContract]
        bool ModifyPerformance(int id, string name, DateTime date, int room, double ticketPrice);
        [OperationContract]
        void ModifyDiscount(int discount);
        [OperationContract]
        bool MakeReservation(int performanceId, DateTime date, int ticketQuantity, string clientUsername, out int reservationId);
        [OperationContract]
        bool PayReservationWithoutDiscount(int reservationsId, string clientUsername);
        [OperationContract]
        bool CheckIfPerformanceExists(int key);
        [OperationContract]
        void ListAllPerformances();
        [OperationContract]
        void ListAllUsers();
        [OperationContract]
        void ListAllReservations();
        [OperationContract]
        bool CheckIfReservationCanBePaied(int reservationsId,string clientUsername);
    }
}

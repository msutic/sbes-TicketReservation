using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}.", e.Message);
            }
        }


        public static void AuthenticationSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserAuthenticationSuccess = AuditEvents.AuthenticationSuccess;
                string message = String.Format(UserAuthenticationSuccess,userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AuthenticationSuccess));
            }
        }

        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            if (customLog != null)
            {
                string AuthorizationSuccess =
                    AuditEvents.AuthorizationSuccess;
                string message = String.Format(AuthorizationSuccess,
                    userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationSuccess));
            }
        }

        public static void WriteInFileSuccess(string userName, string fileName)
        {
            if (customLog != null)
            {
                string WriteInFileSuccess =
                    AuditEvents.WriteInFileSuccess;
                string message = String.Format(WriteInFileSuccess,
                    userName, fileName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WriteInFileSuccess));
            }
        }

        public static void ReadFromFileSuccess(string userName, string fileName)
        {
            if (customLog != null)
            {
                string ReadFromFileSuccess =
                    AuditEvents.ReadFromFileSuccess;
                string message = String.Format(ReadFromFileSuccess,
                    userName, fileName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ReadFromFileSuccess));
            }
        }

        public static void AddToBaseSuccess(string userName, string typeOfEntity)
        {
            if (customLog != null)
            {
                string AddToBaseSuccess =
                    AuditEvents.AddToBaseSuccess;
                string message = String.Format(AddToBaseSuccess,
                    userName, typeOfEntity);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AddToBaseSuccess));
            }
        }

        public static void ChangeSuccess(string userName, string typeOfEntity)
        {
            if (customLog != null)
            {
                string ChangeSuccess =
                    AuditEvents.ChangeSuccess;
                string message = String.Format(ChangeSuccess,
                    userName, typeOfEntity);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ChangeSuccess));
            }
        }

        public static void PayReservationSuccess(string userName, string oldState, string newState)
        {
            if (customLog != null)
            {
                string PayReservationSuccess =
                    AuditEvents.PayReservationSuccess;
                string message = String.Format(PayReservationSuccess,
                    userName, oldState, newState);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PayReservationSuccess));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
        /// <param name="reason">permission name</param>
        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string AuthorizationFailed =
                    AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed,
                    userName, serviceName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailed));
            }
        }

        public static void WriteInFileFailed(string userName, string fileName, string reason)
        {
            if (customLog != null)
            {
                string WriteInFileFailed =
                    AuditEvents.WriteInFileFailed;
                string message = String.Format(WriteInFileFailed,
                    userName, fileName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.WriteInFileFailed));
            }
        }

        public static void ReadFromFileFailed(string userName, string fileName, string reason)
        {
            if (customLog != null)
            {
                string ReadFromFileFailed =
                    AuditEvents.ReadFromFileFailed;
                string message = String.Format(ReadFromFileFailed,
                    userName, fileName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ReadFromFileFailed));
            }
        }

        public static void MethodCallFailed(string userName, string methodName, string reason)
        {
            if (customLog != null)
            {
                string MethodCallFailed =
                    AuditEvents.MethodCallFailed;
                string message = String.Format(MethodCallFailed,
                    userName, methodName, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.MethodCallFailed));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}

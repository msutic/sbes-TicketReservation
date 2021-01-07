using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public enum AuditEventTypes
    {
        AuthenticationSuccess = 0,
        AuthorizationSuccess = 1,
        AuthorizationFailed = 2,
        WriteInFileSuccess = 3,
        ReadFromFileSuccess = 4,
        AddToBaseSuccess = 5,
        WriteInFileFailed = 6,
        ReadFromFileFailed = 7,
        ChangeSuccess = 8,
        PayReservationSuccess = 9,
        MethodCallFailed = 10
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
            }
        }

        public static string AuthorizationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
            }
        }

        public static string WriteInFileSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WriteInFileSuccess.ToString());
            }
        }

        public static string ReadFromFileSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReadFromFileSuccess.ToString());
            }
        }

        public static string AddToBaseSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddToBaseSuccess.ToString());
            }
        }

        public static string WriteInFileFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.WriteInFileFailed.ToString());
            }
        }

        public static string ReadFromFileFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReadFromFileFailed.ToString());
            }
        }

        public static string ChangeSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ChangeSuccess.ToString());
            }
        }

        public static string PayReservationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PayReservationSuccess.ToString());
            }
        }

        public static string MethodCallFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.MethodCallFailed.ToString());
            }
        }
    }
}

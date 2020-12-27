using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CertManager
    {
        /// <summary>
        /// Get a certificate with the specified subject name from the predefined certificate storage
        /// Only valid certificates should be considered
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="storeLocation"></param>
        /// <param name="subjectName"></param>
        /// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);
            foreach(X509Certificate2 item in collection)
            {
                if (item.SubjectName.Name.Contains(','))
                {
                    string[] parts = item.SubjectName.Name.Split(',');
                    if (parts[0].Equals(string.Format("CN={0}", subjectName)))
                    {
                        return item;
                    }
                }

                if(item.SubjectName.Name.Equals(string.Format("CN={0}",subjectName)))
                {
                    return item;
                }
            }

            return null;
        }
    }
}

using System;
using System.Linq;
using System.Data;
using System.Data.Sql;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System.ServiceProcess;

namespace NashConfigurator.Helper
{
    public class HostnameResolver
    {
        /// <summary>
        /// Finds all availible SQL servers in local network via UDP broadcast, and 
        /// searching registered servers.  This takes ~10s to complete so take care.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHostnameList()
        {
            List<string> hostnames = new List<string>();

            // Try grab registered SQL instances
            try {
                ManagedComputer mc = new ManagedComputer();

                foreach (ServerInstance instance in mc.ServerInstances) {
                    hostnames.Add(instance.Name);
                }
            } catch {}

            // Try grab SQL instances with UDP broadcast
            try {
                DataTable dt = SmoApplication.EnumAvailableSqlServers(false);

                foreach (DataRow dr in dt.Rows) {
                    if (string.IsNullOrEmpty(dr["Instance"].ToString()))
                        hostnames.Add(dr["Name"].ToString());
                    else
                        hostnames.Add($"{dr["Name"]}\\{dr["Instance"]}");
                }
            } catch { }

            // Remove dupes
            return hostnames.Distinct().ToList();
        }
    }
}

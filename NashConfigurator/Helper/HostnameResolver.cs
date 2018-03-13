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
        private static void TryStartService()
        {
            ServiceController controller = new ServiceController("");
        }

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
                    hostnames.Add($"{dr["Name"]}\\{dr["Instance"]}");
                }
            } catch { }

            // Remove dupes
            return hostnames.Distinct().ToList();
        }
    }
}

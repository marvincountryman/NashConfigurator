using System;
using System.Linq;
using System.Data;
using System.Data.Sql;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo.Wmi;

namespace NashConfigurator.Helper
{
    public class HostnameResolver
    {
        public static List<string> GetHostnameList()
        {
            List<string> hostnames = new List<string>();

            try {
                ManagedComputer mc = new ManagedComputer();

                foreach (ServerInstance instance in mc.ServerInstances) {
                    hostnames.Add(instance.Name);
                }
            } catch {}

            return hostnames.Distinct().ToList();
        }
        public static async Task<List<string>> GetHostnameListAsync()
        {
            return await new Task<List<string>>(() => GetHostnameList());
        }
    }
}

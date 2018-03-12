using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashConfigurator.Model
{
    public class WindowsAuthenticator : ISqlAuthenticator
    {
        public string Name { get; } = "Windows Authentication";

        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsReadonly { get; } = true;

        public void Test() { }
        public void Connect() { }
        public void LoadDefaults() {
            Username = Environment.UserDomainName + "\\" + Environment.UserName;
        }
    }
}

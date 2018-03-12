using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashConfigurator.Model
{
    public interface ISqlAuthenticator
    {
        string Name { get; }

        string Username { get; set; }
        string Password { get; set; }
        bool IsReadonly { get; }

        void Test();
        void Connect();
        void LoadDefaults();
    }
}

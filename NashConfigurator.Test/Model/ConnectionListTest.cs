using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashConfigurator.Model.Test
{
    [TestFixture]
    public class ConnectionListTest
    {
        [Test]
        public void TestRefreshHostnames() {
            ConnectionList.Instance.RefreshHostnames();

            TestContext.Out.WriteLine("Hostnames:");
            foreach (var pair in ConnectionList.Instance.Connections) {
                TestContext.Out.WriteLine(pair.Key);
            }
        }

        [Test]
        public void TestRefreshDatabases()
        {
            ConnectionList.Instance.RefreshHostnames();
            ConnectionList.Instance.RefreshDatabases();
        }
    }
}

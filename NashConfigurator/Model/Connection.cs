using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Data.SqlClient;

using NashConfigurator.Helper;

namespace NashConfigurator.Model
{
    public class Connection
    {
        [XmlIgnore]
        public static string Filename { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "NashConfigurator.xml");

        public string Hostname { get; set; }
        public string Database { get; set; }

        [XmlIgnore]
        public bool IsEmpty {
            get => String.IsNullOrEmpty(Hostname) || String.IsNullOrEmpty(Database);
        }

        [XmlIgnore]
        public SqlConnection SqlConnection; 
        
        /// <summary>
        /// Connect to Sql database and update CompanyViewModel's Company list
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync() {
            SqlConnection = new SqlConnection($@"
                Data Source={Hostname};
                Initial Catalog={Database};
                Integrated Security=SSPI;");

            await SqlConnection.OpenAsync();
            AppController.Instance.CompanyViewModel.Companies = await Company.FindAll(SqlConnection);
        }

        /// <summary>
        /// Save to NashConfigurator.xaml on disk
        /// </summary>
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Filename));

            using (FileStream stream = File.OpenWrite(Filename))
                new XmlSerializer(typeof(Connection)).Serialize(stream, this);
        }

        /// <summary>
        /// Load new instance from NashConfigurator.xaml
        /// </summary>
        /// <returns></returns>
        public static Connection Load()
        {
            try {
                using (FileStream stream = File.OpenRead(Filename))
                    return new XmlSerializer(typeof(Connection)).Deserialize(stream) as Connection;
            } catch {
                return new Connection();
            }
        }
    }
}

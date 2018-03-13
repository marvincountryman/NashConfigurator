using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using Dapper;

namespace NashConfigurator.Model
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }
        public DateTime CloseDate { get; set; }
        public bool IgnoreLastPeriod { get; set; }
        public DateTime FiscalDate { get; set; }

        /// <summary>
        /// Save Company object in database with supplied SqlConnection.
        /// </summary>
        /// <param name="connection">SqlConnection</param>
        /// <returns></returns>
        public async Task Update(SqlConnection connection)
        {
            int count = await connection.ExecuteAsync(@"
                UPDATE dbo.Company SET 
                    Name = @Name,
                    Closed = @Closed,
                    CloseDate = @CloseDate,
                    FiscalDate = @FiscalDate,
                    IgnoreLastPeriod = @IgnoreLastPeriod
                WHERE
                    Id = @Id
            ", new {
                 Id = Id,
                 Name = Name,
                 Closed = Closed,
                 CloseDate = CloseDate,
                 FiscalDate = FiscalDate,
                 IgnoreLastPeriod = IgnoreLastPeriod
            });
        }

        /// <summary>
        /// Finds all Company defininitions with supplied SqlConnection
        /// </summary>
        /// <param name="connection"></param>
        /// <returns>List of Companies order by FiscalDate descending</returns>
        public static async Task<List<Company>> FindAll(SqlConnection connection) {
            return (await connection.QueryAsync<Company>(@"SELECT * FROM dbo.Company"))
                .OrderByDescending(x => x.FiscalDate)
                .ToList();
        }
    }
}

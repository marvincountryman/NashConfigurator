using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NashConfigurator.Model
{
    public class Logger
    {
        public static string Filename { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "NashConfigurator.log");

        public void Log(string type, string message) {
            StringBuilder builder = new StringBuilder();

            builder.Append($"[{type}]");
            builder.Append($"[{DateTime.Now}] ");
            builder.Append(message);
            builder.Append(Environment.NewLine);

            File.AppendAllText(Filename, builder.ToString());
        }
        
        public void Warn(Exception ex) {
            Log("Warn", ex.ToString());
        }
        public void Error(Exception ex) {
            Log("Error", ex.ToString());
        }
        public void Fatal(Exception ex) {
            Log("Fatal", ex.ToString());
        }
    }
}

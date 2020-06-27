using Microsoft.Extensions.Configuration;
using Store.BuisnessLogicLayer.Common.Interfaces;
using System.IO;
using System.Text;

namespace Store.BuisnessLogicLayer.Common
{
    public class Logger : ILogger
    {
        private int count = 0;
        private readonly string _writePath;
        public Logger(IConfiguration configuration)
        {
            _writePath = configuration.GetSection("LoggerFilePath").Value;
        }

        public void Log(string message)
        {
            using (var fileStream = new FileStream(_writePath, FileMode.Append))
            {
                message = $"\n Error {++count} \n {message}";
                var input = Encoding.Default.GetBytes(message);
                fileStream.Write(input, 0, input.Length);
            }
        }
    }
}

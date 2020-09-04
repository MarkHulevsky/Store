using Microsoft.Extensions.Configuration;
using Store.BuisnessLogic.Common.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Common
{
    public class Logger : ILogger
    {
        private static int _count = 0;
        private readonly string _writePath;
        public Logger(IConfiguration configuration)
        {
            _writePath = configuration.GetSection("LoggerFilePath").Value;
        }

        public async Task LogAsync(string message)
        {
            if (!File.Exists(_writePath))
            {
                using var fileCreationStream = File.Create(_writePath);
            }
            using var fileStream = new FileStream(_writePath, FileMode.Append);
            message = $"\n Error #{++_count} \n {message}";
            var input = Encoding.Default.GetBytes(message);
            await fileStream.WriteAsync(input, 0, input.Length);
        }
    }
}

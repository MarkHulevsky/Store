using Store.Presentation.Helpers.Interfaces;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation.Helpers
{
    public class HttpHelper : IHttpHelper
    {
        private const string Method = "GET";
        private const string Accept = "application/json";
        private const string UserAgent = "Mozilla/5.0";
        public async Task<string> GetHttpContent(string url)
        {
            var request =
            (HttpWebRequest)WebRequest.Create(url);
            request.Method = Method;
            request.Accept = Accept;
            request.UserAgent = UserAgent;
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var output = new StringBuilder();
            output.Append(await reader.ReadToEndAsync());
            response.Close();
            return output.ToString();
        }
    }
}

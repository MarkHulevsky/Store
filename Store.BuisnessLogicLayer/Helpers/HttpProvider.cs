using Store.BuisnessLogic.Helpers.Interfaces;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers
{
    public class HttpProvider : IHttpProvider
    {
        private const string METHOD = "GET";
        private const string ACCEPT = "application/json";
        private const string USER_AGENT = "Mozilla/5.0";
        public async Task<string> GetHttpContentAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = METHOD;
            request.Accept = ACCEPT;
            request.UserAgent = USER_AGENT;
            var response = (HttpWebResponse)request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var output = new StringBuilder();
            output.Append(await reader.ReadToEndAsync());
            response.Close();
            return output.ToString();
        }
    }
}

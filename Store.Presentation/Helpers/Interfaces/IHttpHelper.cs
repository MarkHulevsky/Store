using System.Threading.Tasks;

namespace Store.Presentation.Helpers.Interfaces
{
    public interface IHttpHelper
    {
        Task<string> GetHttpContent(string url);
    }
}

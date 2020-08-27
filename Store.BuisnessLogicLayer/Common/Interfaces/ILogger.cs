using System.Threading.Tasks;

namespace Store.BuisnessLogic.Common.Interfaces
{
    public interface ILogger
    {
        Task LogAsync(string message);
    }
}

using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IEmailProvider
    {
        Task SendAsync(string toAddress, string subject, string body);
    }
}

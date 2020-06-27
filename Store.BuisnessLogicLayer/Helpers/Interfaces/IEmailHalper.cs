using System.Threading.Tasks;

namespace Store.BuisnessLogic.Helpers.Interfaces
{
    public interface IEmailHalper
    {
        Task SendAsync(string toAddress, string subject, string body);
    }
}

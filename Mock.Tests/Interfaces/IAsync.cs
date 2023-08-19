using System.Threading.Tasks;

namespace Toubiana.Mock.Tests.Interfaces
{
    public interface IAsync
    {
        Task<string> SimpleMethodAsync();

        Task<string> SimpleMethodAsync(string arg1);

        Task<string> SimpleMethodAsync(string arg1, string arg2);
    }
}

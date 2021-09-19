using SQLite;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IDatabaseService
    {
        public Task<SQLiteAsyncConnection> GetConnectionAsync();

        public Task<bool> LoginAsync(string password, string address);

        public Task<string> GetBaseAddress();
    }
}

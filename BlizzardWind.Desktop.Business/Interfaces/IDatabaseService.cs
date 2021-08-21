using SQLite;

namespace BlizzardWind.Desktop.Business.Interfaces
{
    public interface IDatabaseService
    {
        public Task<SQLiteAsyncConnection> GetConnectionAsync();
    }
}

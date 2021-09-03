using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using SQLite;

namespace BlizzardWind.Desktop.Business.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection _encryptedDb;
        private readonly string DBNAME = "blizzard.db3";
        private string _key = "123qwe";

        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_encryptedDb != null)
                return _encryptedDb;

            string dbFilePath = Path.Combine(Environment.CurrentDirectory, DBNAME);
            //SQLiteConnectionString options = new(dbFilePath, true, key: _key);
            SQLiteConnectionString options = new(dbFilePath, true);
            _encryptedDb = new SQLiteAsyncConnection(options);

            await TablesInitialAsync();
            return _encryptedDb;
        }

        private async Task TablesInitialAsync()
        {
            await _encryptedDb.CreateTableAsync<Information>();
            await _encryptedDb.CreateTableAsync<MarkResource>();
            await _encryptedDb.CreateTableAsync<Article>();
            await _encryptedDb.CreateTableAsync<ArticleFamily>();
            await _encryptedDb.CreateTableAsync<ArticleFolder>();
        }
    }
}

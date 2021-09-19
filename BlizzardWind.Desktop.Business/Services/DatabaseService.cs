using BlizzardWind.Desktop.Business.Entities;
using BlizzardWind.Desktop.Business.Interfaces;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlizzardWind.Desktop.Business.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string DB_DIRECTORY = "Database";
        private readonly string DB_NAME = "blizzard.db3";
        private readonly string SETTING_DB_NAME = "blizzard_setting.db3";
        private readonly string BASE_ADDRESS_KEY = "base_address";
        private SQLiteAsyncConnection _settingsDb;
        private SQLiteAsyncConnection _encryptedDb;
        private string _password;
        private string _baseAddress;

        public DatabaseService()
        {
            _settingsDb = new SQLiteAsyncConnection(Path.Combine(Environment.CurrentDirectory, SETTING_DB_NAME));
        }

        public async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            if (_encryptedDb != null)
                return _encryptedDb;

            string directory = Path.Combine(_baseAddress, DB_DIRECTORY);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string dbFilePath = Path.Combine(directory, DB_NAME);
            SQLiteConnectionString options = new(dbFilePath, true, key: _password);
            _encryptedDb = new SQLiteAsyncConnection(options);

            await BusinessTablesInitialAsync();
            return _encryptedDb;
        }

        public async Task<bool> LoginAsync(string password, string address)
        {
            if (!Directory.Exists(address))
                throw new Exception("目录不存在");
            string dbPath = Path.Combine(address, DB_DIRECTORY, DB_NAME);
            if (File.Exists(dbPath))
            {
                try
                {
                    SQLiteConnectionString options = new(dbPath, true, key: password);
                    var db = new SQLiteAsyncConnection(options);
                    int count = await db.Table<Note>().CountAsync();
                    _encryptedDb = db;
                }
                catch (Exception ex)
                {
                    throw new Exception($"密码错误或数据库文件受损：{ex.Message}");
                }
            }
            else
            {
                _password = password;
                _baseAddress = address;
                var db = await GetConnectionAsync();
            }
            _password = password;
            _baseAddress = address;
            var info = await _settingsDb.Table<Information>()
                .FirstOrDefaultAsync(x => x.Key == _baseAddress);
            if (info != null)
            {
                info.Value = address;
                info.CreatedAt = DateTime.Now;
                await _settingsDb.UpdateAsync(info);
            }
            else
            {
                info = new Information()
                {
                    Key = BASE_ADDRESS_KEY,
                    Value = address,
                    CreatedAt = DateTime.Now
                };
                await _settingsDb.InsertAsync(info);
            }
            return true;
        }

        public async Task<string> GetBaseAddress()
        {
            await SettingTableInitialAsync();
            if (!string.IsNullOrEmpty(_baseAddress))
                return _baseAddress;
            var info = await _settingsDb.Table<Information>()
                .FirstOrDefaultAsync(x => x.Key == BASE_ADDRESS_KEY);
            if (info == null || !Directory.Exists(info.Value))
                return string.Empty;
            else
                return info.Value;
        }



        private async Task SettingTableInitialAsync()
        {
            await _settingsDb.CreateTableAsync<Information>();
        }

        private async Task BusinessTablesInitialAsync()
        {
            await _encryptedDb.CreateTableAsync<FileResource>();
            await _encryptedDb.CreateTableAsync<Note>();
            await _encryptedDb.CreateTableAsync<NoteFamily>();
            await _encryptedDb.CreateTableAsync<NoteFolder>();
        }
    }
}

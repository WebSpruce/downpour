using downpour.Models;
using SQLite;
using System.Diagnostics;

namespace downpour.Data
{
    public class database
    {
        private readonly SQLiteAsyncConnection _database;
        public database(string dbPath)
        {
            try
            {
                _database = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                _database.CreateTableAsync<favouriteCities>().Wait();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"database connection error: {ex}");
                MainPage.instance.DisplayAlert($"Database Connection Error", "We couldn't get your data from database. Try later.", "Close Information");
            }
        }
        public Task<string> CheckIfTableIsCreated()
        {
            return _database.ExecuteScalarAsync<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='weatherFavouriteCities';");
        }
        public Task<CreateTableResult> CreateTable()
        {
            return _database.CreateTableAsync<favouriteCities>();
        }
        public async Task<List<favouriteCities>> GetAllFavouriteCities()
        {
            return await _database.Table<favouriteCities>().ToListAsync();
        }
        public async Task<int> SaveCityAsync(favouriteCities fav)
        {
            return await _database.InsertAsync(fav);
        }
        public async Task<int> DeleteCityAsync(favouriteCities fav)
        {
            return await _database.DeleteAsync(fav);
        }
    }
}

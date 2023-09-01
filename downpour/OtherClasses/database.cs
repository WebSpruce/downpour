using SQLite;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace downpour.OtherClasses
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
            }catch(Exception ex)
            {
                Trace.WriteLine($"database connection error: {ex}");
                MainPage.instance.DisplayAlert($"Database Connection Error", "We couldn't get your data from database. Try later.","Close Information");
            }
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

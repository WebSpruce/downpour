using SQLite;

namespace downpour.Models
{
    public class favouriteCities
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CityOrLocation { get; set; }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace downpour.OtherClasses
{
    public class favouriteCities
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CityOrLocation { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using WebExcelReader.Data;
using WebExcelReader.Models;

namespace WebExcelReader.Repository
{
    public class ReadDatabase:IReadDatabase
    {
        private readonly ApplicationDbContext _db;
        public ReadDatabase(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<List<ExcelDb>> GetRecordsAsync()
        {
            var records= await _db.ExcelReaderTable.Select(x=>new ExcelDb()
            {
                first_name=x.first_name,
                last_name=x.last_name,
                highest_probability_country_code=x.highest_probability_country_code,
                lowest_probability_country_code= x.lowest_probability_country_code,
                highest_probability_value=x.highest_probability_value,
                lowest_probability_value= x.lowest_probability_value
            }).ToListAsync();

            return records;
        }
    }
}

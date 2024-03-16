using WebExcelReader.Models;

namespace WebExcelReader.Repository
{
    public interface IReadDatabase
    {
        Task<List<ExcelDb>> GetRecordsAsync();
    }
}

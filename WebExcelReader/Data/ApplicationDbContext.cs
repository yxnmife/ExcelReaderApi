using Microsoft.EntityFrameworkCore;
using WebExcelReader.Models;

namespace WebExcelReader.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<ExcelDb> ExcelReaderTable { get; set; }
    }
}

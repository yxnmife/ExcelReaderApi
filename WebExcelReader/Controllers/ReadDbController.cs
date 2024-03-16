using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebExcelReader.Repository;

namespace WebExcelReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadDbController : ControllerBase
    {
        private readonly IReadDatabase _readDatabase;
        public ReadDbController(IReadDatabase readDatabase)
        {
            _readDatabase= readDatabase;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetDatabaseRecords()
        {
            var records=await _readDatabase.GetRecordsAsync();
            return Ok(records);
        }
    }
}

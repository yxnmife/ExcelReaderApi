using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Text.Json;
using WebExcelReader.Data;
using WebExcelReader.Models;
using System;
using System.Threading.Tasks;

namespace WebExcelReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelReaderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ExcelReaderController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpPost("ReadExcel")]
        public async Task<IActionResult> ReadExcel(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file received");
                }
                // Check if the file is an Excel file
                if (Path.GetExtension(file.FileName).ToLower() != ".xlsx")
                {
                    return BadRequest("Invalid file format. Please upload an Excel file (.xlsx).");
                }
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Assuming data is in the first worksheet
                        var rowCount = worksheet.Dimension.Rows;
                        var names = new List<string>(); //list containing the names in excel sheet
                       
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var fullname = worksheet.Cells[row, 2].Value.ToString(); //assuming the first row is for Headers, and the name column is column 2

                            if (!string.IsNullOrEmpty(fullname) && !fullname.Any(char.IsDigit)) //checks is name is empty and contains number
                            {
                                var FullnameSplit = fullname.Split(" "); //splits all the names 
                                if (FullnameSplit.Length <= 3) //checks if the names are less than or equal to three
                                {
                                    names.Add(fullname);
                                }
                            }
                        }
                        using (var httpClient = new HttpClient())
                        {
                            foreach (var name in names)
                            {
                                var SeparatedNames = name.Split(" "); //splits the names
                                var Lastname = SeparatedNames[0].Trim(); //selects the first item in the array for the Lastname
                                var Firstname = SeparatedNames[SeparatedNames.Length - 1].Trim();//selects the second item in the array for the Firstname
                                {
                                    var response = await httpClient.GetAsync($"https://api.nationalize.io?name={Firstname}");

                                    if (response.IsSuccessStatusCode)
                                    {
                                        var success = await response.Content.ReadAsStringAsync();
                                        if (!string.IsNullOrEmpty(success))
                                        {
                                            var ExcelDbItem = JsonSerializer.Deserialize<CountryData>(success);

                                            if (ExcelDbItem != null && ExcelDbItem.country != null)
                                            {
                                                var ExcelNames = new ExcelDb
                                                {
                                                    first_name = Firstname,
                                                    last_name = Lastname,
                                                    highest_probability_value = GetHighestProbabilty(ExcelDbItem.country),
                                                    lowest_probability_value = GetLowestProbabilty(ExcelDbItem.country),
                                                    highest_probability_country_code = HighestProbabilityCountryCode(ExcelDbItem.country),
                                                    lowest_probability_country_code = LowestProbabilityCountryCode(ExcelDbItem.country)
                                                };
                                                await _db.ExcelReaderTable.AddAsync(ExcelNames);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        await _db.SaveChangesAsync();
                    }
                }
                return Ok("Data added successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Data failed to add to database: {ex.Message}");
            }
        }
        private double GetHighestProbabilty(List<Country> countries)
        {
            if(countries==null|| countries.Count == 0)
            {
                return 0;
            }
            return countries.Max(c => c.probability);
        }
        private double GetLowestProbabilty(List<Country> countries)
        {
            if(countries==null|| countries.Count == 0)
            {
                return 0;
            }
            return countries.Min(c => c.probability);
        }
        private string HighestProbabilityCountryCode(List<Country> countries)
        {
            if(countries==null || countries.Count == 0)
            {
                return default;
            }
            var maxprobability = countries[0].probability;
            var countrycode = countries[0].country_id;
            foreach(var c in countries)
            {
                if(c.probability > maxprobability)
                {
                    maxprobability = c.probability;
                    countrycode = c.country_id;
                }
                  
            }
            return countrycode;
        }
        private string LowestProbabilityCountryCode(List<Country> countries)
        {
            if (countries == null || countries.Count == 0)
            {
                return default;
            }
            var minprobability = countries[0].probability;
            var countrycode = countries[0].country_id;
            foreach (var c in countries)
            {
                if (c.probability < minprobability)
                {
                    minprobability = c.probability;
                    countrycode = c.country_id;
                }

            }
            return countrycode;
        }
    }

}

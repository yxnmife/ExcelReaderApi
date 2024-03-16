using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebExcelReader.Models
{
    public class ExcelDb
    {
      [Key]
      public int Id { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public double? lowest_probability_value { get; set; }
        public double? highest_probability_value { get; set; }
        public string? lowest_probability_country_code { get; set; }
        public string? highest_probability_country_code { get; set; }

    }
}

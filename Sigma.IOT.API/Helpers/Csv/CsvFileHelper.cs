using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace ViaVarejo.Integracao.CrossCutting.Helpers.CsvHelper
{
    public class CsvFileHelper : ICsvFileHelper
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<T> ReadCsv<T>(IFormFile csvFile)
        {
            TextReader reader = new StreamReader(csvFile.OpenReadStream(), Encoding.UTF8);
            CsvConfiguration configuration = new CsvConfiguration(CultureInfo.GetCultureInfo("en-US"));
            var csvReader = new CsvReader(reader, configuration);
            return csvReader.GetRecords<T>().ToList();
        }
    }
}

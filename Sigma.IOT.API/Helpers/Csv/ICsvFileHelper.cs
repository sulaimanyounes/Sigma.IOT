namespace ViaVarejo.Integracao.CrossCutting.Helpers.CsvHelper
{
    public interface ICsvFileHelper : IDisposable
    {
        List<T> ReadCsv<T>(IFormFile csvFile);
    }
}
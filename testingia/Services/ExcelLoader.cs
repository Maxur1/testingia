using System.Data;
using System.IO;
using ExcelDataReader;

namespace testingia.Services
{
    public static class ExcelLoader
    {
        public static DataTable LoadSheet(string excelPath)
        {
            // Register the code page provider (needed for ExcelDataReader)
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            return result.Tables[0]; // Return the first sheet
        }
    }
}

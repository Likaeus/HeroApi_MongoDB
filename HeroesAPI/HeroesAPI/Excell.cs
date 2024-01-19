


using HeroesAPI.Models;
using OfficeOpenXml;

namespace HeroesAPI
{
    public class Excell
    {
        public static async Task Excel(IEnumerable<Hero> heroes)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var file = new FileInfo(@"C:\Users\rodri\Desktop\Demos\Heroes.xlsx");

            await SaveExcelfile(heroes, file);
        }

        private static async Task SaveExcelfile(IEnumerable<Hero> heroes, FileInfo file)
        {
            DeleteIfExists(file);

            using var package = new ExcelPackage(file);

            var ws = package.Workbook.Worksheets.Add("List of Heroes");
            var range = ws.Cells["A1"].LoadFromCollection(heroes, true);
            range.AutoFitColumns();


            await package.SaveAsync();
        }

        private static void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
    }
}

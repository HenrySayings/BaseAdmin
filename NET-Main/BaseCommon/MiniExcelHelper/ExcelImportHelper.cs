using MiniExcelLibs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCommon.MiniExcelHelper
{
    public static class ExcelImportHelper<TEntity> where TEntity : class, new()
    {
        public static async Task<List<TEntity>> ReadExcel(string filePath)
        {
            using (var reader = File.OpenRead(filePath))
            {
                var rows = await reader.QueryAsync<TEntity>();
                return rows.ToList();
            }
        }
    }
}

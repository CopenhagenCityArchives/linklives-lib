using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace Linklives.Serialization {
public abstract class Encoder {
    public static Encoder ForFormat(string format) {
        if(format == "xlsx") {
            return XlsxEncoder.Instance;
        }
        if(format == "csv") {
            return CsvEncoder.Instance;
        }
        if(format == "xls") {
            return XlsEncoder.Instance;
        }
        return null;
    }

    public abstract string ContentType {
        get;
    }

    public abstract MemoryStream Encode(Dictionary<string, (string, Exportable)>[] rows);

    protected Dictionary<string, int> GetColumnKeys(Dictionary<string, (string, Exportable)>[] rows) {
        // TODO: maybe a default ordering we strip things from that are not in any rows?
        var keyToLowestWeightMap = new Dictionary<string, int>();
        foreach(var row in rows) {
            foreach(var key in row.Keys) {
                var existingWeight = keyToLowestWeightMap.ContainsKey(key) ? keyToLowestWeightMap[key] : int.MaxValue;
                var lowestWeight = (new int[] { existingWeight, row[key].Item2.Weight }).Min();
                keyToLowestWeightMap[key] = lowestWeight;
            }
        }

        var keysInOrder = keyToLowestWeightMap.Keys.OrderBy((key) => keyToLowestWeightMap[key]);

        // create map from key to column index
        var result = new Dictionary<string, int>();
        foreach(var key in keysInOrder) {
            result[key] = result.Count;
        }

        return result;
    }

    protected string[] GetOrderedColumns(Dictionary<string, int> columnMap) {
        // Use the columnMap values to order the results
        return columnMap.Keys
            .OrderBy((key) => columnMap[key])
            .ToArray();
    }

    protected string[] GetOrderedValues(Dictionary<string, int> columnMap, Dictionary<string, (string, Exportable)> row) {
        // The result will be the length of the columnMap
        var result = new string[columnMap.Count];

        // Map each key to its position in the result using the columnMap as lookup
        foreach(var key in row.Keys) {
            var i = columnMap[key];
            result[i] = row[key].Item1;
        }
        return result;
    }
}

public class XlsxEncoder: Encoder {
    private static XlsxEncoder instance = new XlsxEncoder();
    public static XlsxEncoder Instance {
        get {
            return instance;
        }
    }

    public override string ContentType {
        get {
            return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
    }

    public override MemoryStream Encode(Dictionary<string, (string, Exportable)>[] rows)
    {
        // Write to Workbook
        var workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
        var sheet = workbook.CreateSheet("Link-lives Download");

        var columnMap = GetColumnKeys(rows);
        var orderedColumns = GetOrderedColumns(columnMap);

        var headerRow = sheet.CreateRow(0);
        for (var i = 0; i < orderedColumns.Length; i++) {
            var cell = headerRow.CreateCell(i);
            cell.SetCellValue(orderedColumns[i]);
        }

        for (var i = 0; i < rows.Length; i++) {
            var row = rows[i];
            var orderedValues = GetOrderedValues(columnMap, row);
            var sheetRow = sheet.CreateRow(i + 1);

            for (var j = 0; j < orderedValues.Length; j++) {
                var cell = sheetRow.CreateCell(j);
                cell.SetCellValue(orderedValues[j]);
            }
        }

        // Output to stream - NPOI closes a memory stream on write, so we need to discard one (so weird tbh)
        var intermediate = new MemoryStream();
        workbook.Write(intermediate);
        var bytes = intermediate.ToArray();

        return new MemoryStream(bytes);
    }
}

public class XlsEncoder: Encoder {
    private static XlsEncoder instance = new XlsEncoder();
    public static XlsEncoder Instance {
        get {
            return instance;
        }
    }

    public override string ContentType {
        get {
            return "application/vnd.ms-excel";
        }
    }

    public override MemoryStream Encode(Dictionary<string, (string, Exportable)>[] rows)
    {
        // Write to Workbook
        var workbook = new CarlosAg.ExcelXmlWriter.Workbook();
        var sheet = workbook.Worksheets.Add("Link-lives Download");

        var columnMap = GetColumnKeys(rows);
        var orderedColumns = GetOrderedColumns(columnMap);

        var headingRow = sheet.Table.Rows.Add();
        foreach(var col in orderedColumns) {
            headingRow.Cells.Add(col);
        }

        foreach(var row in rows) {
            var orderedValues = GetOrderedValues(columnMap, row);
            var sheetRow = sheet.Table.Rows.Add();

            foreach(var val in orderedValues) {
                sheetRow.Cells.Add(val ?? "");
            }
        }

        // Output to stream via temp file
        var result = new MemoryStream();
        var tempFilePath = Path.GetTempFileName();
        try {
            workbook.Save(tempFilePath);

            using (var readingFileStream = File.OpenRead(tempFilePath)) {
                readingFileStream.CopyTo(result);
            }
        }
        finally {
            File.Delete(tempFilePath);
        }

        //result.Seek(0, SeekOrigin.Begin); // Needed?

        return result;
    }
}

public class CsvEncoder: Encoder {
    private static CsvEncoder instance = new CsvEncoder();
    public static CsvEncoder Instance {
        get {
            return instance;
        }
    }

    public override string ContentType {
        get {
            return "text/csv";
        }
    }

    public override MemoryStream Encode(Dictionary<string, (string, Exportable)>[] rows) {
        var result = new StringBuilder();

        var columnMap = GetColumnKeys(rows);
        var orderedColumns = GetOrderedColumns(columnMap);
        WriteRow(result, orderedColumns);

        foreach(var row in rows) {
            var orderedValues = GetOrderedValues(columnMap, row);
            WriteRow(result, orderedValues);
        }

        var bytes = Encoding.UTF8.GetBytes(result.ToString());
        return new MemoryStream(bytes);
    }

    private Regex quotesRegex = new Regex("\"");

    private void WriteRow(StringBuilder result, IEnumerable<string> items) {
        items = items.Select((item) => {
            if(item == null) {
                return "";
            }
            if(!item.Contains(",") && !item.Contains("\"") && !item.Contains("\n")) {
                return item;
            }
            return $"\"{quotesRegex.Replace(item, "\"\"")}\"";
        });

        result.Append(string.Join(",", items));
        result.Append("\n");
    }
}
}

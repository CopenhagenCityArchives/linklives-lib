using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linklives.Serialization {
public static class SpreadsheetSerializer {
    public static Dictionary<string, string>[] Serialize(object[] items) {
        Dictionary<string, string> result = new Dictionary<string, string>();
        return items.SelectMany((item) => Serialize(item)).ToArray();
    }

    public static Dictionary<string, string>[] Serialize(object item) {
        var result = new Dictionary<string,string>{};

        var serializableProperties = item.GetType().GetProperties()
            .Where((prop) => prop.CanRead)
            .Where((prop) => {
                var attrs = prop.GetCustomAttributes(true);
                return attrs.Any((attr) => attr is Exportable);
            });

        foreach(var prop in serializableProperties) {
            var value = prop.GetValue(item, null);
            result[prop.Name.ToLower()] = value?.ToString();
        }

        return new [] { result };
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class Exportable : Attribute {
    private FieldCategory _cat;
    private string _prefix;

    public Exportable(FieldCategory cat = FieldCategory.Other, string prefix = "") {
        this._cat = cat;
        this._prefix = prefix;
    }

    public string BuildName(string name) {
        var sb = new StringBuilder();

        if(_cat is FieldCategory.Standard) {
            sb.Append("st_");
        }
        else if(_cat is FieldCategory.Transcribed) {
            sb.Append("tr_");
        }

        sb.Append(_prefix);
        sb.Append(name);

        return sb.ToString();
    }
}

public enum FieldCategory {
    Identification = 100,
    Standard = 300,
    Transcribed = 500,
    Other = 999,
}

public interface RowSerializable {
    Dictionary<string, string>[] Serialize();
}
}

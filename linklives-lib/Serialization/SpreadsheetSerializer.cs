using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Linklives.Serialization {
public static class SpreadsheetSerializer {
    public static Dictionary<string, (string, Exportable)>[] Serialize(object[] items) {
        Dictionary<string, string> result = new Dictionary<string, string>();
        return items.SelectMany((item) => Serialize(item)).ToArray();
    }

    public static Dictionary<string, (string, Exportable)>[] Serialize(object item, NestedExportable parent = null) {
        var serializableProperties = item.GetType().GetProperties()
            .Where((prop) => prop.CanRead)
            .Select((prop) => {
                var attrs = prop.GetCustomAttributes(true);
                var exportableAttr = attrs.FirstOrDefault((attr) => attr is Exportable || attr is NestedExportable);

                if(parent != null) {
                    if(parent.IncludeAllProperties && exportableAttr == null) {
                        exportableAttr = new Exportable(FieldCategory.Other, parent.Prefix, parent.ExtraWeight);
                    }
                    else if(exportableAttr is Exportable) {
                        exportableAttr = ((Exportable)exportableAttr).Expand(parent.Prefix, parent.ExtraWeight);
                    }
                }

                return (prop, exportableAttr);
            })
            .Where((propAttrPair) => propAttrPair.Item2 != null);

        var flatFields = serializableProperties
            .Where((propAttrPair) => {
                var (prop, attr) = propAttrPair;
                return attr is Exportable;
            })
            .Select((propAttrPair) => {
                var (prop, attr) = propAttrPair;
                return (prop, (Exportable)attr);
            });

        var flatFieldsRow = new Dictionary<string,(string, Exportable)>{};
        foreach(var (prop, attr) in flatFields) {
            var value = prop.GetValue(item, null);
            flatFieldsRow[attr.BuildName(prop.Name)] = (value?.ToString(), attr);
            continue;
        }

        var nestedListFieldRows = serializableProperties
            .Where((propAttrPair) => {
                var (prop, attr) = propAttrPair;
                return attr is NestedExportable;
            })
            .Select((propAttrPair) => {
                var (prop, attr) = propAttrPair;
                var nestedExportable = (NestedExportable)attr;
                var value = prop.GetValue(item, null);
                if(value == null) {
                    return new Dictionary<string,(string, Exportable)>[] {};
                }

                if(typeof(IEnumerable<object>).IsAssignableFrom(prop.PropertyType)) {
                    var enumerable = (IEnumerable<object>)value;
                    return enumerable.SelectMany((item, i) => Serialize(item, nestedExportable.Expand(extraWeight: i)));
                }

                return Serialize(value, nestedExportable);
            });
        
        var nestedListFieldRowsArray = nestedListFieldRows.ToArray();
        var flatFieldsRows = new [] { flatFieldsRow };
        if(nestedListFieldRowsArray.Length == 0) {
            return flatFieldsRows;
        }

        var listOfRowsOfRows = new IEnumerable<Dictionary<string, (string, Exportable)>>[nestedListFieldRowsArray.Length + 1];
        listOfRowsOfRows[0] = flatFieldsRows;
        for(var i = 0; i < nestedListFieldRowsArray.Length; i++) {
            listOfRowsOfRows[i + 1] = nestedListFieldRowsArray[i];
        }
        return BraidRows(listOfRowsOfRows);
    }

    private static Dictionary<string, (string, Exportable)>[] BraidRows(IEnumerable<Dictionary<string, (string, Exportable)>>[] listOfRowsOfRows) {
        var firstColSet = listOfRowsOfRows[0];
        var secondColSet = listOfRowsOfRows[1];

        var resultRows = new List<Dictionary<string, (string, Exportable)>>();
        foreach(var firstColSetRow in firstColSet) {
            foreach(var secondColSetRow in secondColSet) {
                var resultRow = new Dictionary<string, (string, Exportable)>();
                foreach(var (k, v) in firstColSetRow) {
                    resultRow[k] = v;
                }
                foreach(var (k, v) in secondColSetRow) {
                    resultRow[k] = v;
                }
                resultRows.Add(resultRow);
            }
        }

        if(listOfRowsOfRows.Length == 2) {
            return resultRows.ToArray();
        }

        var result = listOfRowsOfRows.Skip(2).Prepend(resultRows);
        return BraidRows(result.ToArray());
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class Exportable : Attribute {
    private FieldCategory _cat;
    private string _prefix;
    private int _extraWeight;

    public Exportable(FieldCategory cat = FieldCategory.Other, string prefix = "", int extraWeight = 0) {
        this._cat = cat;
        this._prefix = prefix;
        this._extraWeight = extraWeight;
    }

    public Exportable Expand(string prefix = "", int extraWeight = 0) {
        return new Exportable(
            _cat,
            prefix + _prefix,
            extraWeight + _extraWeight
        );
    }

    public string BuildName(string name) {
        var sb = new StringBuilder();

        sb.Append(_prefix);
        sb.Append(name);

        return sb.ToString().ToLower();
    }

    public int Weight {
        get {
            return _extraWeight + (int)this._cat;
        }
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class NestedExportable : Attribute {
    string _prefix;
    public string Prefix { get { return _prefix; } }

    int _extraWeight;
    public int ExtraWeight { get { return _extraWeight; } }

    bool _includeAllProperties;
    public bool IncludeAllProperties { get { return _includeAllProperties; } }

    public NestedExportable(string prefix = "", int extraWeight = 1000, bool includeAllProperties = false) {
        _prefix = prefix;
        _extraWeight = extraWeight;
        _includeAllProperties = includeAllProperties;
    }

    public NestedExportable Expand(int extraWeight = 0) {
        return new NestedExportable(
            _prefix,
            _extraWeight + extraWeight,
            _includeAllProperties
        );
    }
}

public enum FieldCategory {
    Identification = 100,
    Other = 900,
}
}

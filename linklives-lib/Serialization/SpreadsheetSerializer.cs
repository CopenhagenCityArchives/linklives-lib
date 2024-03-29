using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Linklives.Serialization {
public static class SpreadsheetSerializer {
    public static Dictionary<string, (string, Exportable)>[] SerializeAll(object[] items, NestedExportable parent = null) {
        Dictionary<string, string> result = new Dictionary<string, string>();
        return items.SelectMany((item) => Serialize(item, parent)).ToArray();
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
                    else if(exportableAttr is NestedExportable) {
                        exportableAttr = ((NestedExportable)exportableAttr).Expand(parent.Prefix, parent.ExtraWeight);
                    }
                }

                return (prop, exportableAttr);
            })
            .Where((propAttrPair) => propAttrPair.Item2 != null)
            .Where((propAttrPair) => {
                if(propAttrPair.Item2 is Exportable) {
                    return ((Exportable)propAttrPair.Item2).ShouldExport(item);
                }
                return true;
            });

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

            // If value is an array, stringify nicely with commas per default
            if(value != null && typeof(object[]).IsAssignableFrom(value.GetType())) {
                var prettyValue = String.Join(",", (value as object[]).Select((entry) => entry.ToString()));
                flatFieldsRow[attr.BuildName(prop.Name)] = (prettyValue, attr);
                continue;
            }

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

                // Key-value string dictionary (or dictionary-like) can be inlined
                if(typeof(IDictionary<string, object>).IsAssignableFrom(value.GetType())) {
                    var dict = (IDictionary<string, object>)value;
                    var result = dict.SelectDict(keyValue => {
                        var (key, value) = keyValue;
                        var attr = new Exportable(prefix: nestedExportable.Prefix, extraWeight: nestedExportable.ExtraWeight);
                        return (attr.BuildName(key), (value.ToString(), attr));
                    });
                    return new Dictionary<string, (string, Exportable)>[] { result };
                }

                // Key-value dynamic object should be inlined if nestedExpandable.ForcedStrategy == NestingStrategy.Inline
                if(nestedExportable.ForcedStrategy == NestingStrategy.Inline) {
                    var props = value.GetType().GetProperties().Where((nestedProp) => nestedProp.CanRead && nestedProp.GetIndexParameters().Length == 0);
                    var result = new Dictionary<string, (string, Exportable)> {};
                    foreach(var nestedProp in props) {
                        var propAttr = new Exportable(prefix: nestedExportable.Prefix, extraWeight: nestedExportable.ExtraWeight);
                        var propValue = nestedProp.GetValue(value, null);
                        result[propAttr.BuildName(nestedProp.Name)] = (propValue.ToString(), propAttr);
                    }
                    return new Dictionary<string, (string, Exportable)>[] { result };
                }

                if(typeof(IEnumerable<object>).IsAssignableFrom(value.GetType())) {
                    var enumerable = (IEnumerable<object>)value;
                    return enumerable.SelectMany((item) => Serialize(item, nestedExportable));
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
        if(listOfRowsOfRows.Length == 1) {
            return listOfRowsOfRows[0].ToArray();
        }

        var firstColSet = listOfRowsOfRows[0];
        if(firstColSet.Count() == 0) {
            return BraidRows(listOfRowsOfRows.Skip(1).ToArray());
        }
        var secondColSet = listOfRowsOfRows[1];
        if(secondColSet.Count() == 0) {
            return BraidRows(listOfRowsOfRows.Skip(2).Prepend(firstColSet).ToArray());
        }

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

    private static Dictionary<string, U> SelectDict<T, U>(this IDictionary<string, T> dict, Func<(string, T), (string, U)> map) {
        var result = new Dictionary<string, U>();
        foreach(var (key, val) in dict) {
            var (newKey, newValue) = map((key, val));
            result[newKey] = newValue;
        }
        return result;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class Exportable : Attribute {
    private FieldCategory _cat;
    private string _prefix;
    private int _extraWeight;

    private Type _exportIfType;

    public Exportable(
        FieldCategory cat = FieldCategory.Other,
        string prefix = "",
        int extraWeight = 0,
        Type exportIfType = null
    ) {
        this._cat = cat;
        this._prefix = prefix;
        this._extraWeight = extraWeight;
        this._exportIfType = exportIfType;
    }

    public Exportable Expand(string prefix = "", int extraWeight = 0) {
        return new Exportable(
            _cat,
            prefix + _prefix,
            extraWeight + _extraWeight,
            _exportIfType
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

    public bool ShouldExport(object subject) {
        if(_exportIfType == null) {
            return true;
        }
        return subject.GetType() == _exportIfType;
    }
}

public enum NestingStrategy {
    Auto,
    Inline,
}

[AttributeUsage(AttributeTargets.Property)]
public class NestedExportable : Attribute {
    string _prefix;
    public string Prefix { get { return _prefix; } }

    int _extraWeight;
    public int ExtraWeight { get { return _extraWeight; } }

    bool _includeAllProperties;
    public bool IncludeAllProperties { get { return _includeAllProperties; } }

    NestingStrategy _forcedStrategy;
    public NestingStrategy ForcedStrategy { get { return _forcedStrategy; } }

    public NestedExportable(string prefix = "", int extraWeight = 1000, bool includeAllProperties = false, NestingStrategy forcedStrategy = NestingStrategy.Auto) {
        _prefix = prefix;
        _extraWeight = extraWeight;
        _includeAllProperties = includeAllProperties;
        _forcedStrategy = forcedStrategy;
    }

    public NestedExportable Expand(string prefix = "", int extraWeight = 0) {
        return new NestedExportable(
            prefix + _prefix,
            _extraWeight + extraWeight,
            _includeAllProperties,
            _forcedStrategy
        );
    }
}

public enum FieldCategory {
    Identification = 100,
    Other = 900,
}
}

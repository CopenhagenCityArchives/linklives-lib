using CsvHelper.Configuration.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    public enum SourceType
    {
        none = 0,
        parish_register = 1,
        census = 2,
        burial_protocol = 3
    }
    [ElasticsearchType(IdProperty = nameof(Source_id))]
    public class Source
    {
        [Name("source_id")]
        public int Source_id { get; set; }
        [Name("source_name")]
        public string Source_name { get; set; }
        [Name("file_reference")]
        public string File_reference { get; set; }
        [Name("original_data_reference")]
        public string Original_data_reference { get; set; }
        [Name("institution_origin")]
        public string Institution_origin { get; set; }
        [Name("link")]
        public string Link { get; set; }
        [Nest.Ignore] //Tells nest to ignore the property when indexing but still lets us include it when serializing to json
        public SourceType Type { get { return GetType(Source_id); } }
        public static SourceType GetType(int source_Id)
        {
            switch (source_Id)
            {
                case var n when (n <= 9):
                    return SourceType.census;
                case 10:
                    return SourceType.burial_protocol;
                case var n when (n >= 11 && n <= 16):
                    return SourceType.parish_register;
                default:
                    return SourceType.none;
            }
        }
    }
}

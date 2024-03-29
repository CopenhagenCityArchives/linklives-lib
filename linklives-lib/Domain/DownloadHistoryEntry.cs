﻿using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Linklives.Domain
{

    public enum DownloadType {
        PersonAppearance = 0,
        Lifecourse = 1,
        SearchResult = 2,
    }

    public static class DownloadTypeExt {
        public static string Stringify(this DownloadType type) {
            switch(type) {
                case DownloadType.PersonAppearance:
                    return "PersonAppearance";
                case DownloadType.Lifecourse:
                    return "Lifecourse";
                case DownloadType.SearchResult:
                    return "SearchResult";
                default:
                    return "Download";
            }
        }

        public static DownloadType FromString(string type) {
            switch(type) {
                case "PersonAppearance":
                    return DownloadType.PersonAppearance;
                case "Lifecourse":
                    return DownloadType.Lifecourse;
                case "SearchResult":
                    return DownloadType.SearchResult;
                default:
                    throw new InvalidOperationException($"Trying to create a DownloadType from an unrecognized string key '{type}'");
            }
        }
    }

    public class DownloadHistoryEntry {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }

        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have a valid download type")]
        public string DownloadType { get; set; }

        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have valid query")]
        public string Query { get; set; }

        [DataMember, Required(AllowEmptyStrings = false, ErrorMessage = "Must have valid downloadedBy ID")]
        public string DownloadedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }

        public DownloadHistoryEntry(DownloadType downloadType, string query, string downloadedBy) {
            DownloadType = downloadType.Stringify();
            Query = query;
            DownloadedBy = downloadedBy;
        }

        public DownloadHistoryEntry(string downloadType, string query, string downloadedBy) {
            //Validate downloadType input string is valid
            DownloadTypeExt.FromString(downloadType);

            DownloadType = downloadType;
            Query = query;
            DownloadedBy = downloadedBy;
        }
    }
}

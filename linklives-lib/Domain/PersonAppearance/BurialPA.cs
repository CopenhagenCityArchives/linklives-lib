using Linklives.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from burial records
    /// </summary>
    public class BurialPA : BasePA
    {
        public override string Sourceplace_searchable
        {
            get
            {
                return "København";
            }
        }
        public override string Deathplace_searchable
        {
            get
            {
                return Sourceplace_searchable;
            }
        }
        public override string First_names_sortable
        {
            get
            {
                return Standard.First_names;
            }
        }
        public override string Pa_entry_permalink_wp4
        {
            get
            {
                return Transcribed.GetTranscriptionPropertyValue("id") == null ? null : $"https://kbharkiv.dk/permalink/post/1-{Transcribed.GetTranscriptionPropertyValue("id") }";
            }
        }
        public override string Source_type_wp4 
        {
            get
            {
                return "burial_protocol";
            } 
        }
        public override int? Deathyear_searchable
        {
            get
            {
                return Int32.TryParse(Standard.Event_year, out var tempInt) ? tempInt : (int?)null;
            }
        }
        public override string Deathyear_searchable_fz
        {
            get
            {
                return IntToRangeAsStringHelper.GetRangePlusMinus3(Deathyear_searchable);
            }
        }
        public override int? Deathyear_display
        {
            get
            {
                return Deathyear_searchable;
            }
        }
        public override string Source_type_display
        {
            get
            {
                return "Begravelsesprotokol";
            }
        }
        public override string Source_archive_display
        {
            get
            {
                return "Københavns Stadsarkiv";
            }
        }
        public override string Birthplace_searchable
        {
            get
            {
                return null;
            }
        }

        public override string Sourceplace_display
        {
            get
            {
                return "København";
            }
        }
        public override string Occupation_display
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Transcribed.GetTranscriptionPropertyValue("positions")))
                    {
                        return "";
                    }
                    string[] relationstypes = Transcribed.GetTranscriptionPropertyValue("relationtypes").Split(",");
                    string[] positions = Transcribed.GetTranscriptionPropertyValue("positions").Split(",");

                    int i = 0;
                    string relationTypesAndPositions = "";
                    foreach (var oc in relationstypes)
                    {
                        if (i >= positions.Length) { i--; }
                        relationTypesAndPositions += $"{positions[i]} ({oc}), ";
                        i++;
                    }

                    return relationTypesAndPositions.Substring(0, relationTypesAndPositions.Length - 2);
                }
                catch(Exception e)
                {
                    return "";
                }
            }
        }

        public override string Occupation_searchable
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Transcribed.GetTranscriptionPropertyValue("positions")))
                    {
                        return "";
                    }

                    string[] occupations = Transcribed.GetTranscriptionPropertyValue("relationtypes").Split(",");
                    string[] positions = Transcribed.GetTranscriptionPropertyValue("positions").Split(",");

                    int i = 0;
                    string relationTypesAndPositions = "";
                    foreach (var oc in occupations)
                    {
                        if (oc.Contains("eget erhverv", StringComparison.InvariantCultureIgnoreCase))
                        {
                            relationTypesAndPositions += $"{positions[i]}, ";
                            i++;
                        }
                    }
                    if (relationTypesAndPositions.Length < 2) { return ""; }
                    return relationTypesAndPositions.Substring(0, relationTypesAndPositions.Length - 2);
                }
                catch(Exception e)
                {
                    return "";
                }
            }
        }

        public BurialPA()
        {
            
        }
        public BurialPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.burial_protocol;
        }
    }
}

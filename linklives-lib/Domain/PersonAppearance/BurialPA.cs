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
        public BurialPA()
        {
            
        }
        public BurialPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.burial_protocol;
        }
        protected override void InitSourceSpecificFields()
        {
            //TODO: Implement burial mapping
            if (string.IsNullOrEmpty(Sourceplace_searchable))
            {
                Sourceplace_searchable = "København";
            }

            int deathYear;
            if (Int32.TryParse(Standard.Event_year, out deathYear)) { 
                Deathyear_searchable = deathYear; 
            
                Deathyear_searchable_fz = string.Join(' ', new int[]
                {
                        Deathyear_searchable.Value -3,
                        Deathyear_searchable.Value -2,
                        Deathyear_searchable.Value -1,
                        Deathyear_searchable.Value,
                        Deathyear_searchable.Value +1,
                        Deathyear_searchable.Value +2,
                        Deathyear_searchable.Value +3
                });
            }

            Occupation_searchable = null; //TODO: figure out what this is supposed to map to (mapping sheet doesnt give field name)

            var occupations = new List<string>();
            occupations.AddRange(Transcribed.Transcription.positions.Replace("\"", "").Split(","));
            var relationtypes = new List<string>();
            relationtypes.AddRange(Transcribed.Transcription.relationtypes.Replace("\"", "").Split(","));

            for (int i = 0; i < occupations.Count; i++)
            {
                Occupation_display = Occupation_display + relationtypes[i].Trim() + " " + occupations[i].Trim() + ", ";
            }
            Occupation_display = Occupation_display.Substring(0, Occupation_display.Length - 2);
            //Occupation_display = Transcribed.Transcription.positions; //TODO: figure out what this is supposed to map to (mapping sheet doesnt give field name)
            Sourceplace_display = "København";
            //Sourceyear_display = Transcribed.Transcription.Source; //TODO: figure out what this is supposed to map to (mapping sheet says source_year but that field doesnt exist anywhere)
            //Event_year_display = null; //TODO: figure out what this is supposed to map to (mapping sheet says source_year but that field doesnt exist anywhere)
            Deathyear_display = Deathyear_searchable;
            Source_type_display = "Begravelsesprotokol";
            Source_archive_display = "Københavns Stadsarkiv";

        }
    }
}

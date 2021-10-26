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
            Deathyear_searchable = Convert.ToInt32(Standard.Event_year);
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
            Occupation_searchable = null; //TODO: figure out what this is supposed to map to (mapping sheet doesnt give field name)
            Occupation_display = null; //TODO: figure out what this is supposed to map to (mapping sheet doesnt give field name)
            Sourceplace_display = "københavn";
            Sourceyear_display = null; //TODO: figure out what this is supposed to map to (mapping sheet says source_year but that field doesnt exist anywhere)
            Event_year_display = null; //TODO: figure out what this is supposed to map to (mapping sheet says source_year but that field doesnt exist anywhere)
            Deathyear_display = Deathyear_searchable;
            Source_type_display = "Begravelsesprotokol";
            Source_archive_display = "Københavns Stadsarkiv";

        }
    }
}

using System;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from census records
    /// </summary>
    public class CensusPA : BasePA
    {
        public CensusPA()
        {

        }
        public CensusPA(StandardPA standardPA, TranscribedPA transcribedPA, int sourceId) : base(standardPA, transcribedPA, sourceId)
        {
            Type = SourceType.census;
        }
        protected override void InitSourceSpecificFields()
        {
            if (string.IsNullOrEmpty(Sourceplace_searchable))
            {
                Sourceplace_searchable = string.Join(' ', new string[] { Transcribed.Transcription.Sogn, Transcribed.Transcription.Herred, Transcribed.Transcription.Amt });
            }
            Occupation_searchable = Transcribed.Transcription.Stilling_i_husstanden;
            Role_display = Standard.Household_position;
            Occupation_display = Occupation_searchable;
            Sourceplace_display = Sourceplace_searchable.Replace(' ', ',');
            Sourceyear_display = Convert.ToInt32(Standard.Event_year);
            Event_year_display = Standard.Event_year;
            Source_type_display = "Folketælling";
            Source_archive_display = "Rigsarkivet";

        }
    }
}

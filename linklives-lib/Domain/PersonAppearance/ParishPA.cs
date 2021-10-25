using System;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance fromed from parish register records
    /// </summary>
    public class ParishPA : BasePA
    {
        public ParishPA()
        {

        }
        public ParishPA(StandardPA standardPA, TranscribedPA transcribedPA, int sourceId) : base(standardPA, transcribedPA, sourceId)
        {
            Type = SourceType.parish_register;
        }
        protected override void InitSourceSpecificFields()
        {
            if (string.IsNullOrEmpty(Birthplace_searchable))
            {
                Birthplace_searchable = string.Join(' ', new string[] { Transcribed.BirthPlace, Transcribed.BirthParish, Transcribed.BirthMunicilality, Transcribed.BirthState });
            }
            if (string.IsNullOrEmpty(Sourceplace_searchable))
            {
                Sourceplace_searchable = string.Join(' ', new string[] { Transcribed.Browselevel, Transcribed.Browselevel1 });
            }
            if (Standard.Event_type == "burial")
            {
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

            }
            if (string.IsNullOrEmpty(Birthplace_display))
            {
                Birthplace_searchable = string.Join(',', new string[] { Transcribed.BirthPlace, string.IsNullOrEmpty(Transcribed.BirthParish) ? null : Transcribed.BirthParish + " sogn", Transcribed.BirthMunicipality, Transcribed.BirthState });
            }
            Sourceplace_display = string.Join(',', new string[] { Transcribed.Browselevel1, Transcribed.Browselevel });
            Sourceyear_display = Transcribed.Browselevel2;
            Event_year_display = Standard.Event_year;
            Deathyear_display = Deathyear_searchable;
            Source_type_display = "Kirkebog";
            Source_archive_display = "Rigsarkivet";
        }
    }
}

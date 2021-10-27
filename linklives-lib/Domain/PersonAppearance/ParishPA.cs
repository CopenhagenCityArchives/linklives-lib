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
        public ParishPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.parish_register;
        }
        protected override void InitSourceSpecificFields()
        {
            if (string.IsNullOrEmpty(Birthplace_searchable))
            {
                Birthplace_searchable = string.Join(' ', new string[] { Transcribed.Transcription.BirthPlace, Transcribed.Transcription.BirthParish, Transcribed.Transcription.BirthMunicilality, Transcribed.Transcription.BirthState });
            }
            if (string.IsNullOrEmpty(Sourceplace_searchable))
            {
                Sourceplace_searchable = string.Join(' ', new string[] { Transcribed.Transcription.Browselevel, Transcribed.Transcription.Browselevel1 }).Trim();
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
                Birthplace_searchable = string.Join(' ', new string[] { Transcribed.Transcription.BirthPlace, string.IsNullOrEmpty(Transcribed.Transcription.BirthParish) ? null : Transcribed.Transcription.BirthParish + " sogn", Transcribed.Transcription.BirthMunicipality, Transcribed.Transcription.BirthState }).Trim().Replace(' ', ','); //trim and replace so we dont end up with strings of just commas
            }
            Sourceplace_display = string.Join(' ', new string[] { Transcribed.Transcription.Browselevel1, Transcribed.Transcription.Browselevel }).Trim().Replace(' ', ',');  //trim and replace so we dont end up with strings of just commas
            Sourceyear_display = Transcribed.Transcription.Browselevel2;
            Event_year_display = Standard.Event_year;
            Deathyear_display = Deathyear_searchable;
            Source_type_display = "Kirkebog";
            Source_archive_display = "Rigsarkivet";
        }
    }
}

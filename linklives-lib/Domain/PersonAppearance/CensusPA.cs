using System;
using System.Linq;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a Person Appearance formed from census records
    /// </summary>
    public class CensusPA : BasePA
    {
        public override string Source_type_wp4
        {
            get
            {
                return "census";
            }
        }
        public override string Pa_grouping_id_wp4
        {
            get
            {
                return Standard.Household_id;
            }
        }
        public override string Role_display
        {
            get
            {
                return Standard.Household_position;
            }
        }
        public override string Source_type_display
        {
            get
            {
                return "Folketælling";
            }
        }
        public override string Source_archive_display
        {
            get
            {
                return "Rigsarkivet";
            }
        }
        public override string Role_searchable
        {
            get
            {
                return Standard.Household_position;
            }
        }
        public override string Occupation_display
        {
            get
            {
                try
                {
                    var erhverv = Transcribed.GetTranscriptionPropertyValue("Erhverv");
                    if (erhverv == null)
                    {
                        return Transcribed.GetTranscriptionPropertyValue("Stilling_i_husstanden");
                    }

                    return erhverv;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        public override string Occupation_searchable
        {
            get
            {
                try
                {
                    var erhverv = Transcribed.GetTranscriptionPropertyValue("Erhverv");
                    var stilling =  Transcribed.GetTranscriptionPropertyValue("Stilling_i_husstanden");

                    var str = erhverv == null && stilling == null ? null : (erhverv + " " + stilling).Trim();
                    if (string.IsNullOrEmpty(str)) return null;
                    return str;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        //TODO: If BasePA also adds "sogn" to paris, this override is not necessary
        public override string Sourceplace_display
        {
            get
            {
                var sogn = string.IsNullOrEmpty(Standard.Event_parish) ? null : Standard.Event_parish;
                var locations = string.Join(' ', new string[] { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }.Where(s => !string.IsNullOrEmpty(s)).Distinct()).Trim();  //trim and replace so we dont end up with strings of just commas
                var str = sogn == null ? locations:  locations.Replace(sogn, sogn + " sogn");
                if (string.IsNullOrEmpty(str)) return null;
                return str;
            }
        }
        public CensusPA()
        {

        }
        public CensusPA(StandardPA standardPA, TranscribedPA transcribedPA, Source source) : base(standardPA, transcribedPA, source)
        {
            Type = SourceType.census;
        }
    }
}

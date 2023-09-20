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
                    System.Console.WriteLine($"CensusPA with no valid occupation_display: {e}");
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
                    var stilling = Transcribed.GetTranscriptionPropertyValue("Stilling_i_husstanden");

                    var str = erhverv == null && stilling == null ? null : (erhverv + " " + stilling).Trim();
                    if (string.IsNullOrEmpty(str)) return null;
                    return str;
                }
                catch (Exception e)
                {
                    System.Console.WriteLine($"CensusPA with no valid occupation_searchable: {e}");
                    return null;
                }
            }
        }
        private string _sourceplace_display;
        //TODO: If BasePA also adds "sogn" to paris, this override is not necessary
        public override string Sourceplace_display
        {
            get
            {
                if (_sourceplace_display != null) return _sourceplace_display;

                // Add "sogn", "herred" and "amt" if the respective fields has values
                var sogn = string.IsNullOrEmpty(Standard.Event_parish) || (Standard.Event_parish != null && Standard.Event_parish.Trim().Length == 0) ? null : Standard.Event_parish.Trim() + " sogn";
                var herred = string.IsNullOrEmpty(Standard.Event_district) || (Standard.Event_district != null && Standard.Event_district.Trim().Length == 0) ? null : Standard.Event_district.Trim() + " herred";
                var amt = string.IsNullOrEmpty(Standard.Event_county) || (Standard.Event_county != null && Standard.Event_county.Trim().Length == 0) ? null : Standard.Event_county.Trim() + " amt";

                // Get trimmed, distinct places that are not null or empty
                var places = new string[] { Standard.Event_location, sogn, herred, Standard.Event_town, amt, Standard.Event_country }.Where(l => l != null).Where(s => !string.IsNullOrEmpty(s)).Select(p => p.Trim()).Distinct();
                
                // Return Event_location or null if no places are given
                if(places.Count() == 0) { return string.IsNullOrEmpty(Standard.Event_location) ? null : Standard.Event_location; }
                
                // If all given locations matches, return only sogn, amt and herred
                var uniqueLocations = new string[] { Standard.Event_location, Standard.Event_town, Standard.Event_country, Standard.Event_parish, Standard.Event_district, Standard.Event_county }.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).Distinct();

                var specialNotEmptyLocations = new string[] { sogn, herred, amt }.Where(s => !string.IsNullOrEmpty(s));

                if (uniqueLocations.Count() == 1 && specialNotEmptyLocations.Count() > 0)
                {
                    return string.Join(", ", specialNotEmptyLocations);
                }

                // Join places with ,
                _sourceplace_display = string.Join(", ", places);

                return _sourceplace_display;
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

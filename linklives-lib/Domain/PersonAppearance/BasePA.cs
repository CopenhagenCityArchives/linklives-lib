using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a generic Person Appearance
    /// </summary>
    public abstract class BasePA : KeyedItem
    {
        private object source_type_wp4;

        public int Source_id { get; set; }
        public int Pa_id { get; set; }
        public SourceType Type { get; protected set; }

        //Searchables
        public string Name_searchable { get; set; }
        public string Name_searchable_fz { get; set; }
        public string Lastname_searchable { get; set; }
        public string Lastname_searchable_fz { get; set; }
        public string Firstnames_searchable { get; set; }
        public string Firstnames_searchable_fz { get; set; }
        public int? Birthyear_searchable { get; set; }
        public string Birthyear_searchable_fz { get; set; }
        public string Birthplace_searchable { get; set; }
        public int? Sourceyear_searchable { get; set; }
        public string Sourceyear_searchable_fz { get; set; }
        public string Sourceplace_searchable { get; set; }
        public int? Deathyear_searchable { get; set; }
        public string Deathyear_searchable_fz { get; set; }
        public string Gender_searchable { get; set; }
        public string Birthname_searchable { get; set; }
        public string Occupation_searchable { get; set; }

        //Sortables
        public string First_names_sortable { get; set; }
        public string Family_names_sortable { get; set; }
        public int? Birthyear_sortable { get; set; }

        //Metadata
        public string Pa_entry_permalink_wp4 { get; set; }
        public DateTime Last_updated_wp4 { get; set; }
        public string Source_type_wp4 { get => Type.ToString(); }

        //Display
        public string Name_display { get; set; }
        public int? Birthyear_display { get; set; }
        public string Role_display { get; set; }
        public string Birthplace_display { get; set; }
        public string Occupation_display { get; set; }
        public string Sourceplace_display { get; set; }
        public string Event_type_display { get; set; }
        public int? Sourceyear_display { get; set; }
        public string Event_year_display { get; set; }
        public int? Deathyear_display { get; set; }
        public string Source_type_display { get; set; }
        public string Source_archive_display { get; set; }

        /// <summary>
        /// The original standardised Person Appearance data
        /// </summary>
        public StandardPA Standard { get; set; }
        /// <summary>
        /// The raw transcribed Person Appearance data
        /// </summary>
        [Nest.Ignore] //Tells nest to ignore the property when indexing but still lets us include it when serializing to json
        public TranscribedPA Transcribed { get; set; }
        [Nest.Ignore]
        public Source Source { get; set; }
        public BasePA()
        {

        }
        public BasePA(StandardPA standardPA, TranscribedPA transcribedPA, Source source)
        {
            Standard = standardPA;
            Transcribed = transcribedPA;
            Pa_id = standardPA.Pa_id;
            Source_id = source.Source_id;
            InitKey();
            InitStandardFields();
            InitSourceSpecificFields();
        }
        public static BasePA Create(Source source, StandardPA standardPA, TranscribedPA transcribedPA)
        {
            switch (source.Type)
            {
                case SourceType.parish_register:
                    return new ParishPA(standardPA, transcribedPA, source);
                case SourceType.census:
                    return new CensusPA(standardPA, transcribedPA, source);
                case SourceType.burial_protocol:
                    return new BurialPA(standardPA, transcribedPA, source);
                default:
                    throw new ArgumentOutOfRangeException($"{source.Type.ToString()} is not a suported source type");
            }
        }
        private void InitStandardFields()
        {
            Name_searchable = Standard.Name_cl;
            Name_searchable_fz = $"{Standard.Name_cl} {Standard.Name}";
            Lastname_searchable = Standard.Name_cl.Trim().Split(' ').Last();
            Lastname_searchable_fz = string.Join(' ', new string[]
                { Lastname_searchable }
                .Concat(Standard.Patronyms.Split(' '))
                .Concat(Standard.Family_names.Split(' '))
                .Concat(Standard.Maiden_names.Split(' '))
                .Concat(Standard.All_patronyms.Split(' '))
                .Concat(Standard.All_family_names.Split(' '))
                .Distinct()).Trim();
            Firstnames_searchable = (string.IsNullOrEmpty(Standard.Name_cl) || Standard.Name_cl.Length < 2) ? Standard.Name_cl : Standard.Name_cl.Substring(0, Standard.Name_cl.Length - Lastname_searchable.Length + 1); //+1 to also get the preceding space
            Firstnames_searchable_fz = string.Join(' ', new string[]
                { Firstnames_searchable }
                .Concat(Standard.First_names.Split(' '))
                .Distinct()).Trim();
            Birthyear_searchable = Int32.TryParse(Standard.Birth_year, out var tempBirthyear) ? tempBirthyear : (int?)null;
            Birthyear_searchable_fz = Birthyear_searchable == null ? null : string.Join(' ', new int[]
                {
                    Birthyear_searchable.Value -3,
                    Birthyear_searchable.Value -2,
                    Birthyear_searchable.Value -1,
                    Birthyear_searchable.Value,
                    Birthyear_searchable.Value +1,
                    Birthyear_searchable.Value +2,
                    Birthyear_searchable.Value +3
                });
            Birthplace_searchable = string.Join(' ', new string[] { Standard.Birth_place, Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }).Trim();
            Sourceyear_searchable = Int32.TryParse(Standard.Birth_year, out var tempSourceYear) ? tempSourceYear : (int?)null;
            Sourceyear_searchable_fz = Sourceyear_searchable == null ? null :  string.Join(' ', new int[]
                {
                    Sourceyear_searchable.Value -3,
                    Sourceyear_searchable.Value -2,
                    Sourceyear_searchable.Value -1,
                    Sourceyear_searchable.Value,
                    Sourceyear_searchable.Value +1,
                    Sourceyear_searchable.Value +2,
                    Sourceyear_searchable.Value +3
                });
            Sourceplace_searchable = string.Join(' ', new string[]
                { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }
                .Distinct()).Trim();
            Deathyear_searchable = null; //To be filled by derived class
            Deathyear_searchable_fz = null; //To be filled by derived class
            Gender_searchable = string.IsNullOrEmpty(Standard.Sex) ? "u" : Standard.Sex;
            Birthname_searchable = Standard.Maiden_names;
            Occupation_searchable = null; //To be filled by derived class

            First_names_sortable = Firstnames_searchable;
            Family_names_sortable = Lastname_searchable;
            Birthyear_sortable = Birthyear_searchable;

            Last_updated_wp4 = DateTime.Now;
            Pa_entry_permalink_wp4 = ""; //TODO: figure out whats supposed to go in this field

            Name_display = (string.IsNullOrEmpty(Standard.Name_cl) || Standard.Name_cl.Length < 2) ? Standard.Name_cl : string.Join(' ', Standard.Name_cl.Split(' ').Select(s => s.Length > 1 ? s[0].ToString().ToUpper() + s.Substring(1) : s.ToUpper() )); //Make first letter of each word uppercase
            Birthyear_display = Birthyear_searchable;
            Role_display = PAStrings.ResourceManager.GetString(Standard.Role.ToLower()) ?? Standard.Role;
            Birthplace_display =  string.Join(' ', new string[] { Standard.Birth_place, Standard.Birth_location, string.IsNullOrEmpty(Standard.Birth_parish) ? null : Standard.Birth_parish + " sogn", Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }).Trim().Replace(' ', ',');  //trim and replace so we dont end up with strings of just commas
            Occupation_display = null; //To be filled by derived class
            Sourceplace_display = null; //To be filled by derived class
            Event_type_display = PAStrings.ResourceManager.GetString(Standard.Event_type.ToLower()) ?? Standard.Event_type;
            Sourceyear_display = Sourceyear_searchable;
            Deathyear_display = Deathyear_searchable;
            Source_type_display = null; //To be filled by derived class
            Source_archive_display = null; //To be filled by derived class

        }
        protected abstract void InitSourceSpecificFields();
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }    
}

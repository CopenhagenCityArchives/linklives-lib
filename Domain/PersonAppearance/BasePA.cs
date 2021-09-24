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
        public int Birthyear_searchable { get; set; }
        public string Birthyear_searchable_fz { get; set; }
        public string Birthplace_searchable { get; set; }
        public int Sourceyear_searchable { get; set; }
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
        public int Birthyear_sortable { get; set; }

        //Metadata
        public string Pa_entry_permalink_wp4 { get; set; }
        public DateTime Last_updated_wp4 { get; set; }
        public string Source_type_wp4 { get => Type.ToString(); }

        //Display
        public string Name_display { get; set; }
        public int Birthyear_display { get; set; }
        public string Role_display { get; set; }
        public string Birthplace_display { get; set; }
        public string Occupation_display { get; set; }
        public string Sourceplace_display { get; set; }
        public string Event_type_display { get; set; }
        public int Sourceyear_display { get; set; }
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
        public dynamic Transcribed { get; set; }
        public BasePA()
        {

        }
        public BasePA(StandardPA standardPA, int sourceId)
        {
            Pa_id = standardPA.Pa_id;
            Source_id = sourceId;
            InitKey();
            InitStandardFields();
            InitSourceSpecificFields();
        }
        public static BasePA Create(int sourceId, StandardPA standardPA)
        {
            var type = Source.GetType(sourceId);
            switch (type)
            {
                case SourceType.parish_register:
                    return new ParishPA(standardPA, sourceId);
                case SourceType.census:
                    return new CensusPA(standardPA, sourceId);
                case SourceType.burial_protocol:
                    return new BurialPA(standardPA, sourceId);
                default:
                    throw new ArgumentOutOfRangeException($"{type.ToString()} is not a suported source type");
            }
        }
        private void InitStandardFields()
        {
            Name_searchable = Standard.Name_cl;
            Name_searchable_fz = $"{Standard.Name_cl} {Standard.Name}";
            Lastname_searchable = Standard.Name_cl.Split(' ').Last();
            Lastname_searchable_fz = string.Join(' ', new string[]
                { Lastname_searchable }
                .Concat(Standard.Patronyms.Split(' '))
                .Concat(Standard.Family_names.Split(' '))
                .Concat(Standard.Maiden_names.Split(' '))
                .Concat(Standard.All_patronyms.Split(' '))
                .Concat(Standard.All_family_names.Split(' '))
                .Distinct());
            Firstnames_searchable = Standard.Name_cl.Substring(0, Standard.Name_cl.Length - Lastname_searchable.Length + 1); //+1 to also get the preceding space
            Firstnames_searchable_fz = string.Join(' ', new string[]
                { Firstnames_searchable }
                .Concat(Standard.First_names.Split(' '))
                .Distinct());
            Birthyear_searchable = Convert.ToInt32(Standard.Birth_year);
            Birthyear_searchable_fz = string.Join(' ', new int[]
                {
                    Birthyear_searchable -3,
                    Birthyear_searchable -2,
                    Birthyear_searchable -1,
                    Birthyear_searchable,
                    Birthyear_searchable +1,
                    Birthyear_searchable +2,
                    Birthyear_searchable +3
                });
            Birthplace_searchable = string.Join(' ', new string[] { Standard.Birth_place, Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place });
            Sourceyear_searchable = Convert.ToInt32(Standard.Event_year);
            Sourceyear_searchable_fz = string.Join(' ', new int[]
                {
                    Sourceyear_searchable -3,
                    Sourceyear_searchable -2,
                    Sourceyear_searchable -1,
                    Sourceyear_searchable,
                    Sourceyear_searchable +1,
                    Sourceyear_searchable +2,
                    Sourceyear_searchable +3
                });
            Sourceplace_searchable = null; //To be filled by derived class
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

            Name_display = string.Join(' ', Standard.Name_cl.Split(' ').Select(s => s[0].ToString().ToUpper() + s.Substring(1))); //Make first letter of each word uppercase
            Birthyear_display = Birthyear_searchable;
            Role_display = PAStrings.ResourceManager.GetString(Standard.Role.ToLower()) ?? Standard.Role;
            Birthplace_display = Birthplace_searchable;
            Occupation_display = null; //To be filled by derived class
            Sourceplace_display = null; //To be filled by derived class
            Event_type_display = PAStrings.ResourceManager.GetString(Standard.Event_type.ToLower()) ?? Standard.Event_type;
            Sourceyear_display = Convert.ToInt32(Standard.Event_year);
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

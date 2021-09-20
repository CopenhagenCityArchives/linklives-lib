using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a generic Person Appearance
    /// </summary>
    public abstract class BasePA : KeyedItem
    {
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
        public string sourceyear_searchable_fz { get; set; }
        public string Sourceplace_searchable { get; set; }
        public int Deathyear_searchable { get; set; }
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
        public string Birthyear_display { get; set; }
        public string Role_display { get; set; }
        public string Birthplace_display { get; set; }
        public string Occupation_display { get; set; }
        public string Sourceplace_display { get; set; }
        public string Event_type_display { get; set; }
        public string Sourceyear_display { get; set; }
        public string Event_year_display { get; set; }
        public string Deathyear_display { get; set; }
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
            InitFields();
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
        private void InitFields()
        {
            //TODO: Implement general mapping
        }
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }    
}

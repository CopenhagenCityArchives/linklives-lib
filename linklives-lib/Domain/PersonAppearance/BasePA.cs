using Linklives.Domain.Utilities;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace Linklives.Domain
{
    /// <summary>
    /// Represents a generic Person Appearance
    /// </summary>
    [ElasticsearchType(IdProperty = nameof(Key))]
    public class BasePA : SortableItem
    {
        public int Source_id { get; set; }
        public int Pa_id { get; set; }
        public SourceType Type { get; protected set; }

        //Searchables
        private string _name_searchable;
        public string Name_searchable
        {
            set
            {
                _name_searchable = value;
            }
            get
            {
                if (_name_searchable != null) return _name_searchable;
                
                _name_searchable = Standard.Name_cl.Trim();
                
                return _name_searchable;
            }
        }
        private string _name_searchable_fz;
        public string Name_searchable_fz
        {
            set
            {
                _name_searchable_fz = value;
            }
            get
            {
                if (_name_searchable_fz != null) return _name_searchable_fz;
                _name_searchable_fz = $"{Standard.Name_cl} {Standard.Name}";

                return _name_searchable_fz;
            }
        }
        private string _lastname_searchable;
        public string Lastname_searchable
        {
            set
            {
                _lastname_searchable = value;
            }
            get
            {
                if (_lastname_searchable != null) return _lastname_searchable;
                _lastname_searchable = Standard.Name_cl.Trim().Split(' ').Last();

                return _lastname_searchable;
            }
        }
        private string _lastname_searchable_fz;
        public string Lastname_searchable_fz
        {
            set
            {
                _lastname_searchable_fz = value;
            }
            get
            {
                if (_lastname_searchable_fz != null) return _lastname_searchable_fz;

                _lastname_searchable_fz =  string.Join(' ', new string[]
                    { Standard.Name_cl.Trim().Split(' ').Last() }
                    .Concat(Standard.Patronyms.Split(' '))
                    .Concat(Standard.Family_names.Split(' '))
                    .Concat(Standard.Maiden_names.Split(' '))
                    .Concat(Standard.All_patronyms.Split(' '))
                    .Concat(Standard.All_family_names.Split(' '))
                    .Distinct()).Replace("  ", " ").Trim();

                return _lastname_searchable_fz;
            }
        }
        private string _firstnames_searchable;
        public string Firstnames_searchable
        {
            set
            {
                _firstnames_searchable = value;
            }
            get
            {
                if (_firstnames_searchable != null) return _firstnames_searchable;
                
                _firstnames_searchable = string.Join(" ", Standard.Name_cl.Split(" ").SkipLast(1));

                return _firstnames_searchable;
            }
        }
        private string _firstnames_searchable_fz;
        public string Firstnames_searchable_fz
        {
            set
            {
                _firstnames_searchable_fz = value;
            }
            get
            {
                if (_firstnames_searchable_fz != null) return _firstnames_searchable_fz;

                _firstnames_searchable_fz =  string.Join(' ', new string[] { }
                .Concat(Standard.Name_cl.Split(" ").SkipLast(1))
                .Concat(Standard.First_names.Split(' '))
                .Distinct()).Trim();

                return _firstnames_searchable_fz;
            }
        }
        private int? _birthyear_searchable;
        [Nest.Keyword]
        public int? Birthyear_searchable
        {
            set
            {
                _birthyear_searchable = value;
            }
            get
            {
                if (_birthyear_searchable != null) return _birthyear_searchable;

                _birthyear_searchable = Int32.TryParse(Standard.Birth_year, out var tempBirthyear) ? tempBirthyear : (int?)null;

                return _birthyear_searchable;
            }
        }
        private string _birthyear_searchable_fz;
        public string Birthyear_searchable_fz
        {
            set
            {
                _birthyear_searchable_fz = value;
            }
            get
            {
                if (_birthyear_searchable_fz != null) return _birthyear_searchable_fz;

                _birthyear_searchable_fz = IntToRangeAsStringHelper.GetRangePlusMinus3(Birthyear_searchable);

                return _birthyear_searchable_fz;
            }
        }
        private string _birthplace_searchable;
        public virtual string Birthplace_searchable
        {
            set
            {
                _birthplace_searchable = value;
            }
            get
            {
                if (_birthplace_searchable != null) return _birthplace_searchable;
                _birthplace_searchable = string.Join(' ', new string[] { Standard.Birth_place, Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }.Distinct()).Trim();

                return _birthplace_searchable;
            }
        }
        private int? _sourceyear_display;
        [Nest.Keyword]
        public int? Sourceyear_searchable
        {
            set
            {
                _sourceyear_display = value;
            }
            get
            {
                if (_sourceyear_display != null) return _sourceyear_display;
                _sourceyear_display =  Int32.TryParse(Standard.Event_year, out var tempSourceYear) ? tempSourceYear : (int?)null;

                return _sourceyear_display;
            }
        }
        private string _sourceyear_searchable_fz;
        public string Sourceyear_searchable_fz
        {
            set
            {
                _sourceyear_searchable_fz = value;
            }
            get
            {
                if (_sourceyear_searchable_fz != null) return _sourceyear_searchable_fz;
                _sourceyear_searchable_fz = IntToRangeAsStringHelper.GetRangePlusMinus3(Sourceyear_searchable);

                return _sourceyear_searchable_fz;
            }
        }
        public override int? Sourceyear_sortable
        {
            get
            {
                return Sourceyear_searchable;
            }
        }
        private string _sourceplace_searchable;
        public virtual string Sourceplace_searchable
        {
            set
            {
                _sourceplace_searchable = value;
            }
            get
            {
                if (_sourceplace_searchable != null) return _sourceplace_searchable;

                _sourceplace_searchable = string.Join(' ', new string[]
                { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }
                .Distinct()).Trim();

                return _sourceplace_searchable;
            }
        }
        private string _sourceplace_display;
        public virtual string Sourceplace_display
        {
            set
            {
                _sourceplace_display = value;
            }
            get
            {
                if (_sourceplace_display != null) return _sourceplace_display;
                _sourceplace_display = string.Join(' ', new string[]
                { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }
                .Distinct().Where(s => !string.IsNullOrEmpty(s))).Trim();
                
                if (string.IsNullOrEmpty(_sourceplace_display)) return null;
                
                return _sourceplace_display;
            }
        }
        [Nest.Keyword]
        public virtual int? Deathyear_searchable { get; }

        public virtual string Deathyear_searchable_fz { get; }

        public string Gender_searchable
        {
            get
            {
                return PAStrings.ResourceManager.GetString(Standard.Sex.ToLower()) ?? "not given";
            }
        }

        private string _birthname_searchable;
        public string Birthname_searchable
        {
            set
            {
                _birthname_searchable = value;
            }
            get
            {
                if (_birthname_searchable != null) return _birthname_searchable;

                _birthname_searchable = Standard.Maiden_names != "" ? Standard.Maiden_names : null;

                return _birthname_searchable;
            }
        }
        public virtual string Occupation_searchable { get; set; }

        //Sortables
        public override string First_names_sortable
        {
            get
            {
                return string.Join(" ", Standard.Name_cl.Split(" ").SkipLast(1));
            }
        }
        public override string Family_names_sortable
        {
            get
            {
                return Lastname_searchable;
            }
        }
        public override int? Birthyear_sortable
        {
            get
            {
                return Birthyear_searchable;
            }
        }
        //Metadata
        public virtual string Pa_entry_permalink_wp4 { get; }
        public DateTime Last_updated_wp4
        {
            get
            {
                return DateTime.Now;
            }
        }
        [Nest.Keyword]
        public virtual string Source_type_wp4 { get; }
        [Nest.Keyword]
        public virtual string Pa_grouping_id_wp4 { get; }
        public virtual string Pa_grouping_id_wp4_sortable
        {
            get
            {
                return Standard.Image_appearance;
            }
        }

        //Display
        private string _name_display;
        public string Name_display
        {
            set
            {
                _name_display = value;
            }
            get
            {
                if (_name_display != null) return _name_display;
                string uppercaseWords = string.IsNullOrEmpty(Standard.Name_cl) ? Standard.Name_cl : string.Join(' ', Standard.Name_cl.Split(' ').Select(s => s.Length > 1 ? (s[0].ToString().ToUpper() + s.Substring(1)) : s.ToUpper())); //Make first letter of each word uppercase
                
                _name_display = Regex.Replace(uppercaseWords, @"\s+", " ");
                
                return _name_display;
            }
        }
        [Nest.Keyword]
        public int? Birthyear_display
        {
            get
            {
                return Birthyear_searchable;
            }
        }
        public virtual string Role_display
        {
            get
            {
                return PAStrings.ResourceManager.GetString(Standard.Role.ToLower()) ?? null;
            }
        }
        private string _birthplace_display;
        public string Birthplace_display
        {
            set
            {
                _birthplace_display = value;
            }
            get
            {
                if (_birthplace_display != null) return _birthplace_display;

                // Add "sogn" and "amt" if the respective fields has values
                var sogn = string.IsNullOrEmpty(Standard.Birth_parish) || (Standard.Birth_parish != null && Standard.Birth_parish.Trim().Length == 0) ? null : Standard.Birth_parish.Trim() + " sogn";
                var amt = string.IsNullOrEmpty(Standard.Birth_county) || (Standard.Birth_county != null && Standard.Birth_county.Trim().Length == 0) ? null : Standard.Birth_county.Trim() + " amt";

                // Get trimmed, distinct places that are not null or empty
                var places = new string[] { Standard.Birth_location, sogn, Standard.Birth_town, amt, Standard.Birth_country, Standard.Birth_foreign_place }.Where(l => l != null).Where(s => !string.IsNullOrEmpty(s)).Select(p => p.Trim()).Distinct();

                // Return Birth_place or null if no places are given
                if (places.Count() == 0) { return string.IsNullOrEmpty(Standard.Birth_place) ? null : Standard.Birth_place; }

                // If all given locations matches, return only sogn, amt and herred
                var uniqueLocations = new string[] { Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.Trim()).Distinct();

                var specialNotEmptyLocations = new string[] { sogn, amt }.Where(s => !string.IsNullOrEmpty(s));

                if (uniqueLocations.Count() == 1 && specialNotEmptyLocations.Count() > 0)
                {
                    return string.Join(", ", specialNotEmptyLocations);
                }

                _birthplace_display = string.Join(", ", places);

                return _birthplace_display;
            }
        }
        public virtual string Occupation_display { get; }

        [Nest.Keyword]
        public string Event_type_display
        {
            get
            {
                return PAStrings.ResourceManager.GetString(Standard.Event_type.ToLower()) ?? Standard.Event_type;
            }
        }
        [Nest.Keyword]
        public virtual string Sourceyear_display
        {
            get
            {
                return Standard.Event_year;
            }
        }
        [Nest.Keyword]
        public string Event_year_display { get { return Standard.Event_year; } }
        public override int? Event_year_sortable
        {
            get
            {
                return Int32.TryParse(Standard.Event_year, out var tempSourceYear) ? tempSourceYear : (int?)null;
            }
        }
        [Nest.Keyword]
        public virtual int? Deathyear_display { get; }
        public override int? Deathyear_sortable
        {
            get
            {
                return Deathyear_searchable;
            }
        }
        public virtual string Deathplace_searchable { get; }
        [Nest.Keyword]
        public virtual string Source_type_display { get; }
        public virtual string Source_archive_display { get; }
        public virtual string Role_searchable
        {
            get
            {
                return PAStrings.ResourceManager.GetString(Standard.Role.ToLower()) ?? null;
            }
        }

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
        public override void InitKey()
        {
            Key = $"{Source_id}-{Pa_id}";
        }
    }
}

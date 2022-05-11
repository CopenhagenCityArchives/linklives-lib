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
        public string Name_searchable
        {
            get
            {
                return Standard.Name_cl.Trim();
            }
        }
        public string Name_searchable_fz {
            get
            {
                return $"{Standard.Name_cl} {Standard.Name}";
            }
        }
        public string Lastname_searchable {
            get
            {
                return Standard.Name_cl.Trim().Split(' ').Last();
            }
        }
        public string Lastname_searchable_fz
        {
            get
            {
                return string.Join(' ', new string[]
                    { Standard.Name_cl.Trim().Split(' ').Last() }
                    .Concat(Standard.Patronyms.Split(' '))
                    .Concat(Standard.Family_names.Split(' '))
                    .Concat(Standard.Maiden_names.Split(' '))
                    .Concat(Standard.All_patronyms.Split(' '))
                    .Concat(Standard.All_family_names.Split(' '))
                    .Distinct()).Replace("  ", " ").Trim();
            }
        }
        public string Firstnames_searchable
        {
            get
            {
                return string.Join(" ", Standard.Name_cl.Split(" ").SkipLast(1));
            }
        }
        public string Firstnames_searchable_fz {
            get
            {
                return string.Join(' ', new string[] { }
                .Concat(Standard.Name_cl.Split(" ").SkipLast(1))
                .Concat(Standard.First_names.Split(' '))
                .Distinct()).Trim();
            }
        }
        [Nest.Keyword]
        public int? Birthyear_searchable {
            get
            {
                return Int32.TryParse(Standard.Birth_year, out var tempBirthyear) ? tempBirthyear : (int?)null;
            }
        }
        public string Birthyear_searchable_fz
        {
            get
            {
                return IntToRangeAsStringHelper.GetRangePlusMinus3(Birthyear_searchable);
            }
        }
        public virtual string Birthplace_searchable
        {
            get
            {
                return string.Join(' ', new string[] { Standard.Birth_place, Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }.Distinct()).Trim();
            }
        }
        [Nest.Keyword]
        public int? Sourceyear_searchable {
            get
            {
                return Int32.TryParse(Standard.Event_year, out var tempSourceYear) ? tempSourceYear : (int?)null;
            }
        }
        public string Sourceyear_searchable_fz {
            get
            {
                return IntToRangeAsStringHelper.GetRangePlusMinus3(Sourceyear_searchable);
            }
        }
        public override int? Sourceyear_sortable
        {
            get
            {
                return Sourceyear_searchable;
            }
        }
        public virtual string Sourceplace_searchable
        {
            get
            {
                return string.Join(' ', new string[]
                { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }
                .Distinct()).Trim();
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
        public string Birthname_searchable
        {
            get
            {
                return Standard.Maiden_names != "" ? Standard.Maiden_names : null;
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
            get {
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
        public virtual string Source_type_wp4 { get;  }
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
        public string Name_display
        {
            get
            {
                string uppercaseWords = string.IsNullOrEmpty(Standard.Name_cl) ? Standard.Name_cl : string.Join(' ', Standard.Name_cl.Split(' ').Select(s => s.Length > 1 ? (s[0].ToString().ToUpper() + s.Substring(1)) : s.ToUpper())); //Make first letter of each word uppercase
                return Regex.Replace(uppercaseWords, @"\s+", " ");
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
        public string Birthplace_display
        {
            get
            {
                var sogn = string.IsNullOrEmpty(Standard.Birth_parish) ? null : Standard.Birth_parish;
                var places = new string[] { Standard.Birth_place, Standard.Birth_location, Standard.Birth_parish, Standard.Birth_town, Standard.Birth_county, Standard.Birth_country, Standard.Birth_foreign_place }.Distinct();
                var locations = string.Join(' ', places).Trim();  //trim and replace so we dont end up with strings of just commas
                if (string.IsNullOrEmpty(locations)) return null;
                return sogn == null ? locations : locations.Replace(sogn, sogn + " sogn");

            }
        }
        public virtual string Occupation_display { get; }
        public virtual string Sourceplace_display
        {
            get
            {
                return string.Join(' ', new string[]
                { Standard.Event_location, Standard.Event_parish, Standard.Event_district, Standard.Event_town, Standard.Event_county, Standard.Event_country }
                .Distinct().Where(s => !string.IsNullOrEmpty(s))).Trim();
            }
        }
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
        public override int? Deathyear_sortable {
            get
            {
                return Deathyear_searchable;
            }
        }
        public  virtual string Deathplace_searchable { get; }
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

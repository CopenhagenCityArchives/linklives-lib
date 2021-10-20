using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Linklives.Domain
{
    public class StandardPA
    {
        [Name("pa_id")]
        public int Pa_id { get; set; }
        //Aside from the ID we retain all other fields as strings to keep data looking as close to how we received it as possible.
        [Name("name_cl")]
        public string Name_cl { get; set; }
        [Name("name")]
        public string Name { get; set; }
        [Name("first_names")]
        public string First_names { get; set; }
        [Name("patronyms")]
        public string Patronyms { get; set; }
        [Name("family_names")]
        public string Family_names { get; set; }
        [Name("maiden_names")]
        public string Maiden_names { get; set; }
        [Name("all_patronyms")]
        public string All_patronyms { get; set; }
        [Name("all_family_names")]
        public string All_family_names { get; set; }
        [Name("uncat_names")]
        public string Uncat_names { get; set; }
        [Name("sex")]
        public string Sex { get; set; }
        [Name("marital_status")]
        public string Marital_status { get; set; }
        [Name("age")]
        public string Age { get; set; }
        [Name("birth_date")]
        public string Birth_date { get; set; }
        [Name("birth_year")]
        public string Birth_year { get; set; }
        [Name("birth_month")]
        public string Birth_month { get; set; }
        [Name("birth_day")]
        public string Birth_day { get; set; }
        [Name("event_date")]
        public string Event_date { get; set; }
        [Name("event_year")]
        public string Event_year { get; set; }
        [Name("event_month")]
        public string Event_month { get; set; }
        [Name("event_day")]
        public string Event_day { get; set; }
        [Name("birth_place_cl")]
        public string Birth_place_cl { get; set; }
        [Name("birth_place")]
        public string Birth_place { get; set; }
        [Name("birth_location")]
        public string Birth_location { get; set; }
        [Name("birth_parish")]
        public string Birth_parish { get; set; }
        [Name("birth_town")]
        public string Birth_town { get; set; }
        [Name("birth_county")]
        public string Birth_county { get; set; }
        [Name("birth_foreign_place")]
        public string Birth_foreign_place { get; set; }
        [Name("birth_country")]
        public string Birth_country { get; set; }
        [Name("event_location")]
        public string Event_location { get; set; }
        [Name("event_parish")]
        public string Event_parish { get; set; }
        [Name("event_district")]
        public string Event_district { get; set; }
        [Name("event_town")]
        public string Event_town { get; set; }
        [Name("event_county")]
        public string Event_county { get; set; }
        [Name("event_country")]
        public string Event_country { get; set; }
        [Name("household_id")]
        public string Household_id { get; set; }
        [Name("household_position")]
        public string Household_position { get; set; }
        [Name("role")]
        public string Role { get; set; }
        [Name("event_type")]
        public string Event_type { get; set; }
        [Name("book_id")]
        public string Book_id { get; set; }
        [Name("image_id")]
        public string Image_id { get; set; }
        [Name("image_appearance")]
        public string Image_appearance { get; set; }
    }
}

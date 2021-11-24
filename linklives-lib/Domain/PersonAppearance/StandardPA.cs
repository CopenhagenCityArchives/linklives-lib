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
        [Nest.Keyword]
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
        [Nest.Keyword]
        public string Event_type { get; set; }
        [Name("book_id")]
        public string Book_id { get; set; }
        [Name("image_id")]
        public string Image_id { get; set; }
        [Name("image_appearance")]
        public string Image_appearance { get; set; }

        public static StandardPA FromCommaSeparatedString(string str)
        {
            var properties = str.Split(",");
            if(properties.Length != 42)
            {
                throw new ArgumentException("Input string must have 42 values");
            }

            var standard = new StandardPA
            {
                Pa_id = Int32.Parse(properties[0]),
                Name_cl = properties[1],
                Name = properties[2],
                First_names = properties[3],
                Patronyms = properties[4],
                Family_names = properties[5],
                Maiden_names = properties[6],
                All_patronyms = properties[7],
                All_family_names = properties[8],
                Uncat_names = properties[9],
                Sex = properties[10],
                Marital_status = properties[11],
                Age = properties[12],
                Birth_date = properties[13],
                Birth_year = properties[14],
                Birth_month = properties[15],
                Birth_day = properties[16],
                Event_date = properties[17],
                Event_year = properties[18],
                Event_month = properties[19],
                Event_day = properties[20],
                Birth_place_cl = properties[21],
                Birth_place = properties[22],
                Birth_location = properties[23],
                Birth_parish = properties[24],
                Birth_town = properties[25],
                Birth_county = properties[26],
                Birth_foreign_place = properties[27],
                Birth_country = properties[28],
                Event_location = properties[29],
                Event_parish = properties[30],
                Event_district = properties[31],
                Event_town =properties[32],
                Event_county = properties[33],
                Event_country = properties[34],
                Household_id = properties[35],
                Household_position = properties[36],
                Role = properties[37],
                Event_type = properties[38],
                Book_id = properties[39],
                Image_id = properties[40],
                Image_appearance = properties[41]
            };

            return standard;
        }
    }
}

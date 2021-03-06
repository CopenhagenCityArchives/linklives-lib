using Linklives.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace linklives_lib_test
{
    public class FieldMappingsBasePA
    {
        private Source source;
        private StandardPA standardPA;

        [SetUp]
        public void Setup()
        {
            source = new Source();
            source.Source_id = 1;

            standardPA = new StandardPA();
            standardPA.Pa_id = 1;
        }

        [Test]
        [TestCase("Hans Jensen","Hans Jensen")]
        public void GetNameSearchable_ReturnNameClean(string nameClean, string nameSearchable)
        {
            standardPA.Name_cl = nameClean;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(nameSearchable, basePA.Name_searchable);
        }

        [Test]
        [TestCase("Hans Jensen", "Hans Jensen", "Hans Jensen Hans Jensen")]
        public void GetNameSearchableFz_ReturnNameCleanAndName(string nameClean, string name, string nameSearchableFz)
        {
            standardPA.Name_cl = nameClean;
            standardPA.Name = name;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(nameSearchableFz, basePA.Name_searchable_fz);
        }

        [Test]
        [TestCase("Hans Jensen", "Jensen")]
        public void GetLastNameSearchable_ReturnLastWordOfNameClean(string nameClean, string lastNameSearchableFz)
        {
            standardPA.Name_cl = nameClean;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(lastNameSearchableFz, basePA.Lastname_searchable);
        }

        [Test]
        [TestCase("nameCleanLast patronyms familyNames maidenNames allPatronyms allFamilyNames", "nameCleanFirst nameCleanLast", "patronyms", "familyNames", "maidenNames", "allPatronyms", "allFamilyNames")]
        [TestCase("jensen hansen andersen", "jensine hansen jensen", "hansen jensen", "andersen", "hansen jensen", "hansen jensen", "hansen jensen")]
        public void GetLastNameSearchableFz_ReturnUniqueValuesOfPatronymsFamilyNamesMaidenNamesAllPatronumsAllFamilyNamesAndLastWordOfNameClean(string expectedNames, string nameClean, string patronyms, string familyNames, string maidenNames, string allPatronyms, string allFamilyNames)
        {
            standardPA.Name_cl = nameClean;
            standardPA.Patronyms = patronyms;
            standardPA.Family_names = familyNames;
            standardPA.Maiden_names = maidenNames;
            standardPA.All_patronyms = allPatronyms;
            standardPA.All_family_names = allFamilyNames;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expectedNames, basePA.Lastname_searchable_fz);
        }

        [Test]
        [TestCase("Hans Jensen", "Hans")]
        public void GetFirstNamesSearchable_ReturnAllButLastWordInNameClean(string nameClean, string expected)
        {
            standardPA.Name_cl = nameClean;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Firstnames_searchable);
        }

        [Test]
        [TestCase("firstname1 firstname2", "firstname1 firstname2 lastname", "firstname1 firstname2")]
        public void GetFirstNamesSearchableFz_ReturnUniqueValuesOfFirstNamesAndAllButLastWordOfNameClean(string firstNames, string nameClean, string expected)
        {
            standardPA.Name_cl = nameClean;
            standardPA.First_names = firstNames;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Firstnames_searchable_fz);
        }

        [Test]
        [TestCase("1886", 1886)]
        public void GetBirthYearSearchable_ReturnBirthYear(string birthYear, int expected)
        {
            standardPA.Birth_year = birthYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Birthyear_searchable);
        }

        [Test]
        [TestCase("3", "0 1 2 3 4 5 6")]
        [TestCase(null, null)]
        public void GetBirthYearSearchableFz_ReturnBirthYearPlusMinus3(string birthYear, string expected)
        {
            standardPA.Birth_year = birthYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Birthyear_searchable_fz);
        }

        [Test]
        [TestCase("1886", 1886)]
        public void GetSourceYearSearchable_ReturnEventYear(string eventYear, int expected)
        {
            standardPA.Event_year = eventYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceyear_searchable);
        }

        [Test]
        [TestCase("1886", "1886")]
        [TestCase(null, null)]
        public void GetSourceYearDisplay_ReturnEventYear(string event_year, string expected)
        {
            standardPA.Event_year = event_year;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceyear_display);
        }

        [Test]
        [TestCase("1886", 1886)]
        [TestCase("", null)]
        public void GetEventYearSortable_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Event_year_sortable);
        }

        [Test]
        [TestCase("1886", "1886")]
        [TestCase("", "")]
        public void GetEventYearDisplay_ReturnEventYear(string eventYear, string expected)
        {
            standardPA.Event_year = eventYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Event_year_display);
        }

        [Test]
        [TestCase("1886", 1886)]
        [TestCase("", null)]
        public void GetSourceYearSortable_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceyear_sortable);
        }

        [Test]
        [TestCase("3", "0 1 2 3 4 5 6")]
        public void GetSourceYearSearchableFz_ReturnEventYearPlusMinus3(string eventYear, string expected)
        {
            standardPA.Event_year = eventYear;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceyear_searchable_fz);
        }

        [Test]
        [TestCase("location", "parish", "district", "town", "county", "country", "location parish district town county country")]
        [TestCase("same", "same", "same", "same", "same", "same", "same")]
        public void GetSourcePlaceSearchable_ReturnUniqueValuesFromEventLocationEventParishEventDistrictEventTownEventCountyAndEventCountry(string location, string parish, string district, string town, string county, string country, string expected)
        {
            standardPA.Event_location = location;
            standardPA.Event_parish = parish;
            standardPA.Event_district = district;
            standardPA.Event_town = town;
            standardPA.Event_county = county;
            standardPA.Event_country = country;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceplace_searchable);
        }

        [Test]
        [TestCase("", "", "", "", "", "", null)]
        [TestCase("same", "same", "same", "same", "same", "same", "same")]
        [TestCase("location", "", "district", "town", "county", "country", "location district town county country")]
        [TestCase("location", "parish", "district", "town", "county", "country", "location parish district town county country")]
        public void GetSourcePlaceDisplay_ReturnUniqueValuesFromEventLocationEventParishEventDistrictEventTownEventCountyAndEventCountry(string location, string parish, string district, string town, string county, string country, string expected)
        {
            standardPA.Event_location = location;
            standardPA.Event_parish = parish;
            standardPA.Event_district = district;
            standardPA.Event_town = town;
            standardPA.Event_county = county;
            standardPA.Event_country = country;

            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Sourceplace_display);
        }

        [Test]
        [TestCase("m")]
        [TestCase("f")]
        public void GetGenderSearchableWithKnownValue_ReturnMatchingPAString(string sex)
        {
            standardPA.Sex = sex;
            var basePA = new BasePA(standardPA, null, source);
            
            Assert.NotNull(basePA.Gender_searchable);
            Assert.AreEqual(PAStrings.ResourceManager.GetString(sex), basePA.Gender_searchable);
        }

        [Test]
        [TestCase("unknown")]
        [TestCase("another value")]

        public void GetGenderSearchableWithUnknownValue_ReturnNotGiven(string sex)
        {
            standardPA.Sex = sex;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual("not given", basePA.Gender_searchable);
        }

        [Test]
        [TestCase("marie kathrine", "marie kathrine")]
        public void GetBirthNameSearchable_ReturnMaidenNames(string maidenName, string expected)
        {
            standardPA.Maiden_names = maidenName;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Birthname_searchable);
        }

        [Test]
        [TestCase("firstname1 firstname2 lastname", "firstname1 firstname2")]
        public void GetFirstNamesSortable_ReturnAllWordsFromNameCleanExceptTheLast(string nameClean, string expected)
        {
            standardPA.Name_cl = nameClean;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.First_names_sortable);
        }

        [Test]
        [TestCase("firstname1 firstname2 lastname", "lastname")]
        public void GetFamilyNamesSortable_ReturnLastWordOfNameClean(string nameClean, string expected)
        {
            standardPA.Name_cl = nameClean;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Family_names_sortable);
        }

        [Test]
        [TestCase("1886", 1886)]
        [TestCase("", null)]
        public void GetBirthyearSortable_ReturnBirthYear(string birthYear, int? expected)
        {
            standardPA.Birth_year = birthYear;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, basePA.Birthyear_sortable);
        }

        [Test]
        [TestCase(1, null)]
        public void GetPAEntryPermaLinkWP4_ReturnURLToSpecificPost(int id, string expected)
        {
            standardPA.Pa_id = id;
            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Pa_entry_permalink_wp4);
        }

        [Test]
        public void GetLastUpdatedWP4_ReturnDateTimeValue()
        {
            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(typeof(DateTime),pa.Last_updated_wp4.GetType());
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase(null, null)]
        [TestCase("", "")]
        public void GetPaGroupingIdWp4Sortable_ReturnStandardizedImageSequence(string expected, string imageAppearance)
        { 
            standardPA.Image_appearance = imageAppearance;
            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Pa_grouping_id_wp4_sortable);
        }

        [Test]
        [TestCase("jens Simon nielsen", "Jens Simon Nielsen")]
        [TestCase("jens simon nielsen", "Jens Simon Nielsen")]
        [TestCase("firstname    lastname", "Firstname Lastname")]
        [TestCase("d    s", "D S")]
        public void GetNameDisplay_ReturnAllWordInNameCleanTrimmedWithBeginningCapitals(string nameClean, string expected)
        {
            standardPA.Name_cl = nameClean;
            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Name_display);
        }

        [Test]
        [TestCase("1886",1886)]
        public void GetBirthYearDisplay_ReturnBirthYear(string birthYear, int? expected)
        {
            standardPA.Birth_year = birthYear;
            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthyear_display);
        }

        [Test]
        [TestCase("birth_parish", "birth_parish", "birth_parish sogn")]
        [TestCase(" birth_parish ", " birth_parish ", "birth_parish sogn")]
        [TestCase("birth_parish", " birth_parish ", "birth_parish sogn")]
        [TestCase(" birth_parish ", "birth_parish", "birth_parish sogn")]
        public void GetBirthPlaceDisplay_ParishHasValueUntrimmedValueAndBirthPlaceHasSameTrimmedValue_ReturnParishWithSognOnce(string untrimmedValue, string birthParish, string expected)
        {
            standardPA.Birth_parish = birthParish;
            standardPA.Birth_location = untrimmedValue;
            standardPA.Birth_town = untrimmedValue;
            //standardPA.Birth_county = untrimmedValue;
            standardPA.Birth_country = untrimmedValue;

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthplace_display);
        }

        [Test]
        [TestCase("birth_location", "birth_parish", "birth_town", "birth_county", "birth_country", "birth_foreign_place", "birth_location, birth_parish sogn, birth_town, birth_county amt, birth_country, birth_foreign_place")]
        [TestCase("birth_location", null, "birth_town", "birth_county", "birth_country", "birth_foreign_place", "birth_location, birth_town, birth_county amt, birth_country, birth_foreign_place")]
        [TestCase("", "", "", "", "", "", null)]
        [TestCase(null, null, null, null, null, null, null)]
        public void GetBirthPlaceDisplay_ParishHasValue_ReturnParishWithSogn(string birth_location, string birth_parish, string birth_town, string birth_county, string birth_country, string birth_foreign_place, string expected)
        {
            standardPA.Birth_parish = birth_parish;
            standardPA.Birth_location = birth_location;
            standardPA.Birth_town = birth_town;
            standardPA.Birth_county = birth_county;
            standardPA.Birth_country = birth_country;
            standardPA.Birth_foreign_place = birth_foreign_place;

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthplace_display);
        }

        [Test]
        [TestCase("birth_county", "other_values",  "other_values, birth_county amt")]
        [TestCase(null, "other_values", "other_values")]
        public void GetBirthPlaceDisplay_CountyHasValue_ReturnCountyWithAmt(string birthCounty, string otherValues, string expected)
        {
            standardPA.Birth_location = otherValues;
            standardPA.Birth_town = otherValues;
            standardPA.Birth_county = birthCounty;
            standardPA.Birth_country = otherValues;
            standardPA.Birth_foreign_place = otherValues;

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthplace_display);
        }

        [Test]
        [TestCase("town", "country", "town, country")]
        [TestCase("town", null, "town")]
        public void GetBirthPlaceDisplay_SomeFieldsHaveValues_ReturnTrimmedBirthPlaceDisplay(string birthTown, string birthCountry, string expected)
        {
            standardPA.Birth_town = birthTown;
            standardPA.Birth_country = birthCountry;

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthplace_display);
        }

        [Test]
        public void GetBirthPlaceDisplay_OnlyBirthPlaceHasValue_ReturnBirthplace()
        {
            standardPA.Birth_place = "birthPlace";
            standardPA.Birth_location = "";
            standardPA.Birth_parish = "";
            standardPA.Birth_town = "";
            standardPA.Birth_county = "";
            standardPA.Birth_country = "";
            standardPA.Birth_foreign_place = ""; 

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual("birthPlace", pa.Birthplace_display);
        }

        [Test]
        public void GetBirthPlaceDisplay_OtherFieldsThanBirthPlaceHasValues_ReturnValuesWithoutBirthPlace()
        {
            standardPA.Birth_place = "Birth_place";
            standardPA.Birth_location = "Birth_location";
            standardPA.Birth_parish = "Birth_parish";
            standardPA.Birth_town = "Birth_town";
            standardPA.Birth_county = "Birth_county";
            standardPA.Birth_country = "Birth_country";
            standardPA.Birth_foreign_place = "Birth_foreign_place";

            var pa = new BasePA(standardPA, null, source);

            Assert.IsTrue(!pa.Birthplace_display.Contains("Birth_place"));
        }

        [Test]
        [TestCase("place","location","parish","town","county","country","foreignplace","location, parish sogn, town, county amt, country, foreignplace")]
        [TestCase(" place ", " location ", " parish ", " town ", " county ", " country ", " foreignplace ", "location, parish sogn, town, county amt, country, foreignplace")]
        [TestCase(null, null, null, null, null, null, null, null)]
        public void GetBirthPlaceDisplay_ReturnTrimmedBirthPlaceBirthLocationBirthParishBirthTownBirthCountyBirthCountryBirthForeignPlace(string birthPlace, string birthLocation, string birthParish, string birthTown, string birthCounty, string birthCountry, string birthForeignPlace, string expected)
        {
            standardPA.Birth_place = birthPlace;
            standardPA.Birth_location = birthLocation;
            standardPA.Birth_parish = birthParish;
            standardPA.Birth_town = birthTown;
            standardPA.Birth_county = birthCounty;
            standardPA.Birth_country = birthCountry;
            standardPA.Birth_foreign_place = birthForeignPlace;

            var pa = new BasePA(standardPA, null, source);

            Assert.AreEqual(expected, pa.Birthplace_display);
        }

        [Test]
        [TestCase("place", "location", "parish", "town", "county", "country", "foreignplace", "place location parish town county country foreignplace")]
        [TestCase("same", "same", "same", "same", "same", "same", "same", "same")]
        public void GetBirthPlaceSearchable_ReturnBirthPlaceBirthLocationBirthParishBirthTownBirthCountyBirthCountryBirthForeignPlace(string place, string location, string parish, string town, string county, string country, string foreignplace, string expected)
        {
            standardPA.Birth_place = place;
            standardPA.Birth_location = location;
            standardPA.Birth_parish = parish;
            standardPA.Birth_town = town;
            standardPA.Birth_county = county;
            standardPA.Birth_country = country;
            standardPA.Birth_foreign_place = foreignplace;

            var parishPA = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, parishPA.Birthplace_searchable);
        }

        [Test]
        [TestCase("unknown value")]
        [TestCase("father")]
        [TestCase("")]
        public void GetEventTypeDisplay_ReturnMatchingPAStringOrEventType(string eventTypeDisplay)
        {
            standardPA.Event_type = eventTypeDisplay;
            var basePA = new BasePA(standardPA, null, source);

            Assert.AreEqual(PAStrings.ResourceManager.GetString(eventTypeDisplay) ?? eventTypeDisplay, basePA.Event_type_display);
        }

        [Test]
        [TestCase("father")]
        [TestCase("confirmand")]
        public void GetRoleDisplay_ReturnMatchingPAString(string role)
        {
            standardPA.Role = role;
            var basePA = new BasePA(standardPA, null, source);
            
            Assert.NotNull(basePA.Role_display);
            Assert.AreEqual(PAStrings.ResourceManager.GetString(role), basePA.Role_display);
        }

        [Test]
        [TestCase("father")]
        [TestCase("confirmand")]
        public void GetRoleSearchable_ReturnMatchingPAString(string role)
        {
            standardPA.Role = role;
            var basePA = new BasePA(standardPA, null, source);
            
            Assert.NotNull(basePA.Role_display);
            Assert.AreEqual(PAStrings.ResourceManager.GetString(role), basePA.Role_display);
        }
    }
}
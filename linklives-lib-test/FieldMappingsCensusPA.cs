using Linklives.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace linklives_lib_test
{
    public class FieldMappingsCensusPA
    {
        private Source source;
        private StandardPA standardPA;

        [SetUp]
        public void Setup()
        {
            source = new Source();
            source.Source_id = 9;

            standardPA = new StandardPA();
            standardPA.Pa_id = 1;
        }

        [Test]
        public void GetDeathYearSearchable_ReturnNull()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, pa.Deathyear_searchable);
        }

        [Test]
        public void GetDeathYearSearchableFz_ReturnNull()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, pa.Deathyear_searchable_fz);
        }

        [Test]
        public void GetDeathYearSortable_ReturnNull()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, pa.Deathyear_sortable);
        }

        [Test]
        public void GetSourceTypeWP4_ReturnCensus()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("census", pa.Source_type_wp4);
        }

        [Test]
        [TestCase("1", "1")]
        [TestCase(null, null)]
        public void GetPaGroupingIdWp4_ReturnHouseHoldId(string household_id, string expected)
        {
            standardPA.Household_id = household_id;
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Pa_grouping_id_wp4);
        }

        [Test]
        [TestCase("", "", "", "", "", "", null)]
        [TestCase("location", "", "district", "town", "county", "country", "location, district herred, town, county amt, country")]
        [TestCase("location", "parish", "district", "town", "county", "country", "location, parish sogn, district herred, town, county amt, country")]
        [TestCase("same", "same", "same", "same", "same", "same", "same sogn, same herred, same amt")]
        [TestCase("same", "", "same", "same", "same", "same", "same herred, same amt")]
        public void GetSourcePlaceDisplay_ReturnUniqueValuesFromEventLocationEventParishEventDistrictEventTownEventCountyAndEventCountry(string location, string parish, string district, string town, string county, string country, string expected)
        {
            standardPA.Event_location = location;
            standardPA.Event_parish = parish;
            standardPA.Event_district = district;
            standardPA.Event_town = town;
            standardPA.Event_county = county;
            standardPA.Event_country = country;

            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Sourceplace_display);
        }

        [Test]
        [TestCase("father", "father")]
        public void GetRoleDisplay_ReturnHouseholdPosition(string householdPosition, string expected)
        {
            standardPA.Household_position = householdPosition;
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(pa.Role_display, expected);
        }

        [Test]
        public void GetDeathYearDisplay_ReturnNull()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, pa.Deathyear_display);
        }
        [Test]
        public void GetSourceTypeDisplay_ReturnFolketaelling()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Folketælling", pa.Source_type_display);
        }

        [Test]
        public void GetSourceArchiveDisplay_ReturnRigsarkivet()
        {
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Rigsarkivet", pa.Source_archive_display);
        }

        [Test]
        [TestCase("role", "role")]
        [TestCase(null, null)]
        public void GetRoleSearchable_ReturnHouseholdPosition(string householdPosition, string expected)
        {
            standardPA.Household_position = householdPosition;
            var pa = (CensusPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Role_searchable);
        }

        [Test]
        [TestCase("erhverv", "stilling_i_husstanden", "erhverv")]
        [TestCase("erhverv", null, "erhverv")]
        [TestCase(null, "stilling_i_husstanden", "stilling_i_husstanden")]
        [TestCase(null, null, null)]
        public void GetOccupationDisplay_ReturnTranscribedErhverv(string erhverv, string stilling_i_husstanden, string expected)
        {
            dynamic transcription = new { pa_id = "1", Erhverv = erhverv, Stilling_i_husstanden = stilling_i_husstanden };

            var transcribed = new TranscribedPA(transcription, 1);
            var pa = (CensusPA)BasePA.Create(source, standardPA, transcribed);

            Assert.AreEqual(expected, pa.Occupation_display);
        }

        [Test]
        [TestCase("erhverv", "stilling_i_husstanden", "erhverv stilling_i_husstanden")]
        [TestCase("erhverv", null, "erhverv")]
        [TestCase(null, "stilling_i_husstanden", "stilling_i_husstanden")]
        [TestCase(null, null, null)]
        public void GetOccupationSearchable_ReturnTranscribedErhvervAndStillingIHusstanden(string erhverv, string stilling_i_husstanden, string expected)
        {
            dynamic transcription = new { pa_id = "1", Erhverv = erhverv, Stilling_i_husstanden = stilling_i_husstanden };

            var transcribed = new TranscribedPA(transcription, 1);
            var pa = (CensusPA)BasePA.Create(source, standardPA, transcribed);

            Assert.AreEqual(expected, pa.Occupation_searchable);
        }
    }
}
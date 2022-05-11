using Linklives.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace linklives_lib_test
{
    public class FieldMappingsParishPA
    {
        private Source source;
        private StandardPA standardPA;

        [SetUp]
        public void Setup()
        {
            source = new Source();
            source.Source_id = 11;

            standardPA = new StandardPA();
            standardPA.Pa_id = 1;
        }

        [Test]
        [TestCase("1886", "burial", "deceased", 1886)]
        [TestCase("1886", "notburial", "deceased", null)]
        [TestCase("1886", "burial", "notdeceased", null)]
        [TestCase("1886", "notburial", "notdeceased", null)]
        public void GetDeathYearSearchable_ReturnEventYearBasedOnTypeAndRole(string eventYear, string eventType, string role, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;
            standardPA.Role = role;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable);
        }

        [Test]
        [TestCase("1886", "burial", "deceased", 1886)]
        [TestCase("1886", "notburial", "deceased", null)]
        [TestCase("1886", "burial", "notdeceased", null)]
        [TestCase("1886", "notburial", "notdeceased", null)]
        public void GetDeathYearSortable_ReturnEventYearBasedOnTypeAndRole(string eventYear, string eventType, string role, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;
            standardPA.Role = role;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_sortable);
        }

        [Test]
        [TestCase("3", "burial", "deceased", "0 1 2 3 4 5 6")]
        [TestCase("3", "burial", "notdeceased", null)]
        [TestCase("3", "notburial", "deceased", null)]
        [TestCase("3", "notburial", "notdeceased", null)]
        public void GetDeathYearSearchableFz_WithEventTypeAndRole_ReturnEventYearPlusMinus3(string eventYear, string eventType, string role, string? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;
            standardPA.Role = role;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable_fz);
        }

        [Test]
        [TestCase("1886", "burial", "deceased", 1886)]
        [TestCase("1886", "notburial", "deceased", null)]
        [TestCase("1886", "burial", "notdeceased", null)]
        [TestCase("1886", "notburial", "notdeceased", null)]
        public void GetDeathYearDisplay_WithEventTypeBurialAndRoleDeceased_ReturnEventYear(string eventYear, string eventType, string role, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;
            standardPA.Role = role;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_display);
        }

        [Test]
        [TestCase("location", "parish", "district", "town", "county", "country", "location parish district town county country")]
        public void GetDeathplaceSearchableForTypeBurial_ReturnUniqueValuesFromEventLocationEventParishEventDistrictEventTownEventCountyAndEventCountry(string location, string parish, string district, string town, string county, string country, string expected)
        {
            standardPA.Event_type = "burial";
            standardPA.Event_location = location;
            standardPA.Event_parish = parish;
            standardPA.Event_district = district;
            standardPA.Event_town = town;
            standardPA.Event_county = county;
            standardPA.Event_country = country;
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathplace_searchable);
        }

        [Test]
        [TestCase("another type")]
        public void GetDeathplaceSearchableForTypeNotBurial_ReturnNull(string type)
        {
            standardPA.Event_type = type;
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, pa.Deathplace_searchable);
        }

        [Test]
        public void GetSourceTypeWP4_ReturnParishRegister()
        {
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("parish_register", pa.Source_type_wp4);
        }

        [Test]
        [TestCase("1","1")]
        [TestCase(null, null)]
        public void GetPaGroupingIdWp4_ReturnTranscripedEventId(string event_id, string expected)
        {
            dynamic transcription = new { pa_id = "1", event_id = event_id};

            var transPA = new TranscribedPA(transcription, 1);
            var pa = (ParishPA)BasePA.Create(source, standardPA, transPA);

            Assert.AreEqual(expected, pa.Pa_grouping_id_wp4);
        }

        [Test]
        public void GetSourceTypeDisplay_ReturnKirkebog()
        {
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Kirkebog", pa.Source_type_display);
        }
        [Test]
        [TestCase("", "", "")]
        [TestCase("browselevel", "browselevel1", "browselevel, browselevel1")]
        [TestCase("browselevel", "", "browselevel")]
        public void GetSourcePlaceDisplay_ReturnTranscribedBrowselevel1Browselevel(string browselevel, string browselevel1, string expected)
        {
            dynamic transcription = new { pa_id = "1", BrowseLevel = browselevel, BrowseLevel1 = browselevel1 };
            var transPA = new TranscribedPA(transcription, 1);
            var pa = (ParishPA)BasePA.Create(source, standardPA, transPA);

            Assert.AreEqual(expected, pa.Sourceplace_display);
        }

        [Test]
        [TestCase("",  "")]
        [TestCase("1886-1887", "1886-1887")]
        [TestCase(null, null)]
        public void GetSourceYearDisplay_ReturnTranscribedBrowselevel2(string browselevel2, string expected)
        {
            dynamic transcription = new { pa_id = "1", BrowseLevel2 = browselevel2 };
            var transPA = new TranscribedPA(transcription, 1);
            var pa = (ParishPA)BasePA.Create(source, standardPA, transPA);

            Assert.AreEqual(expected, pa.Sourceyear_display);
        }

        [Test]
        public void GetSourceArchiveDisplay_ReturnRigsarkivet()
        {
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Rigsarkivet", pa.Source_archive_display);
        }
    }
}
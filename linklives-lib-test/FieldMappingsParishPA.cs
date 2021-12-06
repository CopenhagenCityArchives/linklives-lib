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
        [TestCase("1886", "burial", 1886)]
        [TestCase("1886", "anothertype", null)]
        public void GetDeathYearSearchable_ReturnEventYearIfTypeBurial(string eventYear, string eventType, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable);
        }

        [Test]
        [TestCase("1886", "burial", 1886)]
        [TestCase("1886", "anothertype", null)]
        public void GetDeathYearSortable_ReturnEventYearIfTypeBurial(string eventYear, string eventType, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_sortable);
        }

        [Test]
        [TestCase("3", "burial", "0 1 2 3 4 5 6")]
        [TestCase("3", "anothertype", null)]
        public void GetDeathYearSearchableFz_ReturnEventYearPlusMinus3IfTypeBurial(string eventYear, string eventType, string? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = eventType;

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable_fz);
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
            var transcription = new ExpandoObject();
            transcription.TryAdd("pa_id", 1);
            transcription.TryAdd("event_id",event_id);
            var transPA = new TranscribedPA(transcription, 1);
            var pa = (ParishPA)BasePA.Create(source, standardPA, transPA);

            Assert.AreEqual(expected, pa.Pa_grouping_id_wp4);
        }

        [Test]
        [TestCase("1886", 1886)]
        public void GetDeathYearDisplay_WithEventTypeBurial_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = "burial";

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_display);
        }

        [Test]
        [TestCase("1886", null)]
        public void GetDeathYearDisplay_WithEventTypeNotBurial_ReturnNull(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;
            standardPA.Event_type = "arrival";

            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_display);
        }
        [Test]
        public void GetSourceTypeDisplay_ReturnKirkebog()
        {
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Kirkebog", pa.Source_type_display);
        }

        [Test]
        public void GetSourceArchiveDisplay_ReturnRigsarkivet()
        {
            var pa = (ParishPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Rigsarkivet", pa.Source_archive_display);
        }
    }
}
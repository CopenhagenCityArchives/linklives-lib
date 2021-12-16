using Linklives.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace linklives_lib_test
{
    public class FieldMappingsBurialPA
    {
        private Source source;
        private StandardPA standardPA;

        [SetUp]
        public void Setup()
        {
            source = new Source();
            source.Source_id = 10;

            standardPA = new StandardPA();
            standardPA.Pa_id = 1;
        }

        [Test]
        public void GetBirthPlaceSearchable_ReturnNull()
        {
            var parishPA = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(null, parishPA.Birthplace_searchable);
        }
        [Test]
        public void GetSourcePlaceSearchable_ReturnKoebenhavn()
        {
            var parishPA = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("København", parishPA.Sourceplace_searchable); 
        }

        [Test]
        public void GetDeathplaceSearchable_ReturnKoebenhavn()
        {
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("København", pa.Deathplace_searchable);
        }

        [Test]
        [TestCase("firstname1 firstname2","firstname1 firstname2")]
        public void GetFirstNamesSortable_ReturnFirstNames(string firstNames, string expected)
        {
            standardPA.First_names = firstNames;
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.First_names_sortable);
        }

        [Test]
        [TestCase(1, @"https://kbharkiv.dk/permalink/post/1-1")]
        public void GetPAEntryPermaLinkWP4_ReturnURLToSpecificPostBasedOnTranscribedId(int id, string expected)
        {
            var transcription = new ExpandoObject();
            transcription.TryAdd("pa_id", 1);
            transcription.TryAdd("id",id);
            var transcribed = new TranscribedPA(transcription, 1);
            var pa = (BurialPA)BasePA.Create(source, standardPA, transcribed);

            Assert.AreEqual(expected, pa.Pa_entry_permalink_wp4);
        }

        [Test]
        [TestCase("", "", "")]
        [TestCase("relation", "erhverv", @"erhverv (relation)")]
        [TestCase("relation1,relation2", "erhverv1,erhverv2", @"erhverv1 (relation1), erhverv2 (relation2)")]
        [TestCase("Forhenværende/pensioneret,Eget erhverv","Forvalter", @"Forvalter (Forhenværende/pensioneret), Forvalter (Eget erhverv)")]
        public void GetOccupationDisplay_ReturnTranscribedRelationTilErhvervAndErhverv(string relationstypes, string positions, string expected)
        {
            var transcription = new ExpandoObject();
            transcription.TryAdd("pa_id", 1);
            transcription.TryAdd("relationtypes", relationstypes);
            transcription.TryAdd("positions", positions);
            var transcribed = new TranscribedPA(transcription, 1);
            var pa = (BurialPA)BasePA.Create(source, standardPA, transcribed);

            Assert.AreEqual(expected, pa.Occupation_display);
        }

        [Test]
        [TestCase("", "", "")]
        [TestCase("eget erhverv", "erhverv", "erhverv")]
        [TestCase("EGET ERHVERV", "erhverv", "erhverv")]
        [TestCase("ægtefælles erhverv", "erhverv", "")]
        [TestCase("eget erhverv, fars erhverv", "erhverv1,erhverv2", "erhverv1")]
        [TestCase("Forhenværende/pensioneret,Eget erhverv","Forvalter", @"Forvalter")]
        public void GetOccupationSearchable_WithRelationTypeEgetErhverv_ReturnTranscribedErhverv(string relationstypes, string positions, string expected)
        {
            var transcription = new ExpandoObject();
            transcription.TryAdd("pa_id", 1);
            transcription.TryAdd("relationtypes", relationstypes);
            transcription.TryAdd("positions", positions);
            var transcribed = new TranscribedPA(transcription, 1);
            var pa = (BurialPA)BasePA.Create(source, standardPA, transcribed);

            Assert.AreEqual(expected, pa.Occupation_searchable);
        }

        [Test]
        public void GetSourceTypeWP4_ReturnBurialProtocol()
        {
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("burial_protocol", pa.Source_type_wp4);
        }

        [Test]
        [TestCase("3", "0 1 2 3 4 5 6")]
        [TestCase("", null)]
        public void GetDeathYearSearchableFz_ReturnEventYearPlusMinus3(string eventYear, string? expected)
        {
            standardPA.Event_year = eventYear;

            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable_fz);
        }

        [Test]
        [TestCase("1886", 1886)]
        [TestCase("",  null)]
        public void GetDeathYearSearchable_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;

            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_searchable);
        }

        [Test]
        [TestCase("1886", 1886)]
        [TestCase("", null)]
        public void GetDeathYearSortable_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;

            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_sortable);
        }

        [Test]
        [TestCase("1886",1886)]
        public void GetDeathYearDisplay_ReturnEventYear(string eventYear, int? expected)
        {
            standardPA.Event_year = eventYear;
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual(expected, pa.Deathyear_display);
        }

        [Test]
        public void GetSourceTypeDisplay_ReturnBegravelsesprotokol()
        {
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Begravelsesprotokol", pa.Source_type_display);
        }

        [Test]
        public void GetSourceArchiveDisplay_ReturnKoebenhavnsStadsarkiv()
        {
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("Københavns Stadsarkiv", pa.Source_archive_display);
        }

        [Test]
        public void GetSourcePlaceDisplay_ReturnKoebenhavn()
        {
            var pa = (BurialPA)BasePA.Create(source, standardPA, null);

            Assert.AreEqual("København", pa.Sourceplace_display);
        }
    }
}
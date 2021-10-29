using Linklives.Domain;
using NUnit.Framework;

namespace linklives_lib_test
{
    public class MappingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PRBurial_MapsCorrectly()
        {
            #region input data
            //11,PR-burial,standardized_sources/PR/burial/burial.csv,transcribed_sources/PR/by_PA/burial.csv,Rigsarkivet,https://www.sa.dk/ao-soegesider/da/geo/geo-collection/5
            var source = new Source
            {
                Source_id = 11,
                Source_name = "PR-burial",
                File_reference = "standardized_sources/PR/burial/burial.csv",
                Original_data_reference = "transcribed_sources/PR/by_PA/burial.csv",
                Institution_origin = "Rigsarkivet",
                Link = "https://www.sa.dk/ao-soegesider/da/geo/geo-collection/5"
            };
            //4,bent nielsen,bent nielsen,bent,nielsen,,,nielsen,,,m,,56.0,,1759,,,1815-05-18,1815,5,18,,,,,,,,,,,,,ålborg,,,,deceased,burial,0,197,4
            var standard = new StandardPA
            {
                Pa_id = 4,
                Name_cl = "bent nielsen",
                Name = "bent nielsen",
                First_names = "bent",
                Patronyms = "nielsen",
                Family_names = "",
                Maiden_names = "",
                All_patronyms = "nielsen",
                All_family_names = "",
                Uncat_names = "",
                Sex = "m",
                Marital_status = "",
                Age = "56.0",
                Birth_date = "",
                Birth_year = "1759",
                Birth_month = "",
                Birth_day = "",
                Event_date = "1815-05-18",
                Event_year = "1815",
                Event_month = "5",
                Event_day = "18",
                Birth_place_cl = "",
                Birth_place = "",
                Birth_location = "",
                Birth_parish = "",
                Birth_town = "",
                Birth_county = "",
                Birth_foreign_place = "",
                Birth_country = "",
                Event_location = "",
                Event_parish = "",
                Event_district = "",
                Event_town = "ålborg",
                Event_county = "",
                Event_country = "",
                Household_id = "",
                Household_position = "",
                Role = "deceased",
                Event_type = "burial",
                Book_id = "0",
                Image_id = "197",
                Image_appearance = "4"
            };
            //4,3972,48438_22021000034_2058-00197,48438_22021000034_2058,3962.0,1040.0,,Kontraministerialbog. Nørresundby Sogn. Fødte kvinder. 1815 - 2003,8032839331,,1815-1833,Ålborg Amt,Nørresundby Sogn,1815-1833,,,,Bent,Nielsen,,,,,Mandlige,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,13.0,maj,1815.0,,,,,,18.0,maj,1815.0,56,,Nørresundby,,Ålborg,Ålborg Amt,Danmark,,,,,,,,,,,,18.0,maj,1815,56,,Nørresundby Sogn,,Ålborg Amt,,Ålborg,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,197,0,4.0,burial,main,1   
            dynamic transcribtion = new
            {
                pa_id = "4",
                event_id = "3972",
                ImageFileName = "48438_22021000034_2058 - 00197",
                ImageFolder = "48438_22021000034_2058",
                unique_identifier = "3962.0",
                SequenceNumber = "1040.0",
                SourceCollectionID = "",
                SourceDescription = "Kontraministerialbog.Nørresundby Sogn.Fødte kvinder. 1815 - 2003",
                SourceReferenceNumber = "8032839331",
                SourceComments = "",
                SourceYearRange = "1815 - 1833",
                BrowseLevel = "Ålborg Amt",
                BrowseLevel1 = "Nørresundby Sogn",
                BrowseLevel2 = "1815 - 1833",
                KeyedParish = "",
                Notes = "",
                NamePrefix = "",
                GivenName = "Bent",
                Surname = "Nielsen",
                NameSuffix = "",
                GivenNameAlias = "",
                SurnameAlias = "",
                MaidenName = "",
                Gender = "Mandlige",
                BirthDay = "",
                BirthMonth = "",
                BirthYear = "",
                BirthPlace = "",
                BirthParish = "",
                BirthMunicipality = "",
                BirthState = "",
                BaptismAge = "",
                BaptismDay = "",
                BaptismMonth = "",
                BaptismYear = "",
                BaptismPlace = "",
                BaptismParish = "",
                BaptismMunicipality = "",
                BaptismCounty = "",
                BaptismState = "",
                BaptismCountry = "",
                ConfirmationDay = "",
                ConfirmationMonth = "",
                ConfirmationYear = "",
                ConfirmationPlace = "",
                ConfirmationParish = "",
                ConfirmationMunicipality = "",
                ConfirmationCounty = "",
                ConfirmationState = "",
                ConfirmationCountry = "",
                ArrivalDay = "",
                ArrivalMonth = "",
                ArrivalYear = "",
                ArrivalAge = "",
                ArrivalPlace = "",
                DeparturePlace = "",
                ArrivalParish = "",
                ArrivalMunicipality = "",
                ArrivalCounty = "",
                ArrivalState = "",
                ArrivalCountry = "",
                DepartureDay = "",
                DepartureMonth = "",
                DepartureYear = "",
                DepartureAge = "",
                DepartureParish = "",
                DepartureMunicipality = "",
                DepartureCounty = "",
                DepartureState = "",
                DepartureCountry = "",
                MarriageDay = "",
                MarriageMonth = "",
                MarriageYear = "",
                MarriageAge = "",
                MarriagePlace = "",
                MarriageParish = "",
                MarriageMunicipality = "",
                MarriageCounty = "",
                MarriageState = "",
                MarriageCountry = "",
                DeathDay = "13.0",
                DeathMonth = "maj",
                DeathYear = "1815.0",
                DeathAge = "",
                DeathPlace = "",
                DeathParish = "",
                DeathMunicipality = "",
                DeathState = "",
                BurialDay = "18.0",
                BurialMonth = "maj",
                BurialYear = "1815.0",
                BurialAge = "56",
                BurialPlace = "",
                BurialParish = "Nørresundby",
                BurialMunicipality = "",
                BurialCounty = "Ålborg",
                BurialState = "Ålborg Amt",
                BurialCountry = "Danmark",
                ResidenceAge = "",
                ResidenceParish = "",
                ResidenceMunicipality = "",
                ResidenceCounty = "",
                ReligiousDay = "",
                ReligiousMonth = "",
                ReligiousYear = "",
                ReligiousAge = "",
                ReligiousParish = "",
                ReligiousMunicipality = "",
                ReligiousState = "",
                VitalDay = "18.0",
                VitalMonth = "maj",
                VitalYear = "1815",
                VitalAge = "56",
                VitalPlace = "",
                VitalParish = "Nørresundby Sogn",
                VitalMunicipality = "",
                VitalState = "Ålborg Amt",
                VitalCountry = "",
                VitalCounty = "Ålborg",
                VitalTownship = "",
                ArrivalCity = "",
                BaptismCity = "",
                BirthCountry = "",
                BurialCity = "",
                ConfirmationAge = "",
                ConfirmationCity = "",
                DepartureCity = "",
                MarriageCity = "",
                VitalCity = "",
                FatherGivenName = "",
                FatherSurname = "",
                FatherNameSuffix = "",
                FatherGivenNameAlias = "",
                FatherSurnameAlias = "",
                MotherNamePrefix = "",
                MotherGivenName = "",
                MotherSurname = "",
                MotherNameSuffix = "",
                MotherMaidenName = "",
                MotherGivenNameAlias = "",
                MotherSurnameAlias = "",
                MotherResidenceAge = "",
                SpouseNamePrefix = "",
                SpouseGivenName = "",
                SpouseSurname = "",
                SpouseNameSuffix = "",
                SpouseMaidenName = "",
                SpouseGivenNameAlias = "",
                SpouseSurnameAlias = "",
                SpouseGender = "",
                SpouseMarriageAge = "",
                SpouseBirthDay = "",
                SpouseBirthMonth = "",
                SpouseBirthYear = "",
                SpouseBirthPlace = "",
                FatherInLawGivenName = "",
                FatherInLawSurname = "",
                FatherInLawNameSuffix = "",
                FatherInLawGivenNameAlias = "",
                FatherInLawSurnameAlias = "",
                MotherInLawNamePrefix = "",
                MotherInLawGivenName = "",
                MotherInLawSurname = "",
                MotherInLawNameSuffix = "",
                MotherInLawMaidenName = "",
                MotherInLawGivenNameAlias = "",
                MotherInLawSurnameAlias = "",
                image_id = "197",
                folder_id = "0",
                image_appearance = "4.0",
                event_type = "burial",
                role = "main",
                event_persons = "1"
            };
            #endregion
            var transcribed = new TranscribedPA(transcribtion, source.Source_id);
            var finishedPa = BasePA.Create(source, standard, transcribed);

            //TODO: These asserts are based on the values found in the testing sheet, but looking at them i am not confident that that are correct to the mapping defined in the mapping sheet and consequently this test is unlikely to provide much value in its current form.
            Assert.AreEqual(finishedPa.Pa_id, 4);

            Assert.AreEqual(finishedPa.Name_searchable, "christa magdalene jensen krage");
            Assert.AreEqual(finishedPa.Name_searchable_fz, "christa magdalene jensen krage krista magdalene jensen krag");
            Assert.AreEqual(finishedPa.Lastname_searchable, "jensen");
            Assert.AreEqual(finishedPa.Lastname_searchable_fz, "jensen krista krag");
            Assert.AreEqual(finishedPa.Firstnames_searchable, null);
            Assert.AreEqual(finishedPa.Firstnames_searchable_fz, null);
            Assert.AreEqual(finishedPa.Birthyear_searchable, "1903");
            Assert.AreEqual(finishedPa.Birthyear_searchable_fz, "1900 1901 1902 1903 1904 1905 1906");
            Assert.AreEqual(finishedPa.Birthplace_searchable, null);
            Assert.AreEqual(finishedPa.Sourceyear_searchable, "1909");
            Assert.AreEqual(finishedPa.Sourceyear_searchable_fz, "");
            Assert.AreEqual(finishedPa.Sourceplace_searchable, "tversted hjørring");
            Assert.AreEqual(finishedPa.Deathyear_searchable, "1906");
            Assert.AreEqual(finishedPa.Deathyear_searchable_fz, "1903 1904 1905 1906 1907 1908 1909");
            Assert.AreEqual(finishedPa.Gender_searchable, "kvinde");
            Assert.AreEqual(finishedPa.Birthname_searchable, null);
            Assert.AreEqual(finishedPa.Occupation_searchable, null);
            //TODO: The test values sheet list a couple of extra fields that are neither implemented in BasePa nor present in the mapping sheet.

            Assert.AreEqual(finishedPa.First_names_sortable, null);
            Assert.AreEqual(finishedPa.Family_names_sortable, "jensen");
            Assert.AreEqual(finishedPa.Birthyear_sortable, "1903");

            //TODO: I dont believe these values are, but in the test values sheet they were all blank
            Assert.AreEqual(finishedPa.Pa_entry_permalink_wp4, "");
            Assert.AreEqual(finishedPa.Last_updated_wp4, "");
            Assert.AreEqual(finishedPa.Source_type_wp4, "");

            Assert.AreEqual(finishedPa.Name_display, "Christa Magdalene Jensen Krage");
            Assert.AreEqual(finishedPa.Birthyear_display, null);
            Assert.AreEqual(finishedPa.Role_display, "Afdøde");
            Assert.AreEqual(finishedPa.Birthplace_display, null);
            Assert.AreEqual(finishedPa.Occupation_display, null);
            Assert.AreEqual(finishedPa.Sourceplace_display, "Tversted sogn, Hjørring amt");
            Assert.AreEqual(finishedPa.Event_type_display, "Begravelse");
            Assert.AreEqual(finishedPa.Sourceyear_display, "1906-1921");
            Assert.AreEqual(finishedPa.Event_year_display, "1906");
            Assert.AreEqual(finishedPa.Deathyear_display, "1906");
            Assert.AreEqual(finishedPa.Source_type_display, "Kirkebog");
            Assert.AreEqual(finishedPa.Source_archive_display, "Rigsarkivet");


        }
    }
}
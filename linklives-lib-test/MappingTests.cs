using Linklives.Domain;
using NUnit.Framework;
using System.Dynamic;

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
            dynamic transcribtion = new ExpandoObject();
            transcribtion.pa_id = "4";
            transcribtion.event_id = "3972";
            transcribtion.ImageFileName = "48438_22021000034_2058 - 00197";
            transcribtion.ImageFolder = "48438_22021000034_2058";
            transcribtion.unique_identifier = "3962.0";
            transcribtion.SequenceNumber = "1040.0";
            transcribtion.SourceCollectionID = "";
            transcribtion.SourceDescription = "Kontraministerialbog.Nørresundby Sogn.Fødte kvinder. 1815 - 2003";
            transcribtion.SourceReferenceNumber = "8032839331";
            transcribtion.SourceComments = "";
            transcribtion.SourceYearRange = "1815 - 1833";
            transcribtion.BrowseLevel = "Ålborg Amt";
            transcribtion.BrowseLevel1 = "Nørresundby Sogn";
            transcribtion.BrowseLevel2 = "1815 - 1833";
            transcribtion.KeyedParish = "";
            transcribtion.Notes = "";
            transcribtion.NamePrefix = "";
            transcribtion.GivenName = "Bent";
            transcribtion.Surname = "Nielsen";
            transcribtion.NameSuffix = "";
            transcribtion.GivenNameAlias = "";
            transcribtion.SurnameAlias = "";
            transcribtion.MaidenName = "";
            transcribtion.Gender = "Mandlige";
            transcribtion.BirthDay = "";
            transcribtion.BirthMonth = "";
            transcribtion.BirthYear = "";
            transcribtion.BirthPlace = "";
            transcribtion.BirthParish = "";
            transcribtion.BirthMunicipality = "";
            transcribtion.BirthState = "";
            transcribtion.BaptismAge = "";
            transcribtion.BaptismDay = "";
            transcribtion.BaptismMonth = "";
            transcribtion.BaptismYear = "";
            transcribtion.BaptismPlace = "";
            transcribtion.BaptismParish = "";
            transcribtion.BaptismMunicipality = "";
            transcribtion.BaptismCounty = "";
            transcribtion.BaptismState = "";
            transcribtion.BaptismCountry = "";
            transcribtion.ConfirmationDay = "";
            transcribtion.ConfirmationMonth = "";
            transcribtion.ConfirmationYear = "";
            transcribtion.ConfirmationPlace = "";
            transcribtion.ConfirmationParish = "";
            transcribtion.ConfirmationMunicipality = "";
            transcribtion.ConfirmationCounty = "";
            transcribtion.ConfirmationState = "";
            transcribtion.ConfirmationCountry = "";
            transcribtion.ArrivalDay = "";
            transcribtion.ArrivalMonth = "";
            transcribtion.ArrivalYear = "";
            transcribtion.ArrivalAge = "";
            transcribtion.ArrivalPlace = "";
            transcribtion.DeparturePlace = "";
            transcribtion.ArrivalParish = "";
            transcribtion.ArrivalMunicipality = "";
            transcribtion.ArrivalCounty = "";
            transcribtion.ArrivalState = "";
            transcribtion.ArrivalCountry = "";
            transcribtion.DepartureDay = "";
            transcribtion.DepartureMonth = "";
            transcribtion.DepartureYear = "";
            transcribtion.DepartureAge = "";
            transcribtion.DepartureParish = "";
            transcribtion.DepartureMunicipality = "";
            transcribtion.DepartureCounty = "";
            transcribtion.DepartureState = "";
            transcribtion.DepartureCountry = "";
            transcribtion.MarriageDay = "";
            transcribtion.MarriageMonth = "";
            transcribtion.MarriageYear = "";
            transcribtion.MarriageAge = "";
            transcribtion.MarriagePlace = "";
            transcribtion.MarriageParish = "";
            transcribtion.MarriageMunicipality = "";
            transcribtion.MarriageCounty = "";
            transcribtion.MarriageState = "";
            transcribtion.MarriageCountry = "";
            transcribtion.DeathDay = "13.0";
            transcribtion.DeathMonth = "maj";
            transcribtion.DeathYear = "1815.0";
            transcribtion.DeathAge = "";
            transcribtion.DeathPlace = "";
            transcribtion.DeathParish = "";
            transcribtion.DeathMunicipality = "";
            transcribtion.DeathState = "";
            transcribtion.BurialDay = "18.0";
            transcribtion.BurialMonth = "maj";
            transcribtion.BurialYear = "1815.0";
            transcribtion.BurialAge = "56";
            transcribtion.BurialPlace = "";
            transcribtion.BurialParish = "Nørresundby";
            transcribtion.BurialMunicipality = "";
            transcribtion.BurialCounty = "Ålborg";
            transcribtion.BurialState = "Ålborg Amt";
            transcribtion.BurialCountry = "Danmark";
            transcribtion.ResidenceAge = "";
            transcribtion.ResidenceParish = "";
            transcribtion.ResidenceMunicipality = "";
            transcribtion.ResidenceCounty = "";
            transcribtion.ReligiousDay = "";
            transcribtion.ReligiousMonth = "";
            transcribtion.ReligiousYear = "";
            transcribtion.ReligiousAge = "";
            transcribtion.ReligiousParish = "";
            transcribtion.ReligiousMunicipality = "";
            transcribtion.ReligiousState = "";
            transcribtion.VitalDay = "18.0";
            transcribtion.VitalMonth = "maj";
            transcribtion.VitalYear = "1815";
            transcribtion.VitalAge = "56";
            transcribtion.VitalPlace = "";
            transcribtion.VitalParish = "Nørresundby Sogn";
            transcribtion.VitalMunicipality = "";
            transcribtion.VitalState = "Ålborg Amt";
            transcribtion.VitalCountry = "";
            transcribtion.VitalCounty = "Ålborg";
            transcribtion.VitalTownship = "";
            transcribtion.ArrivalCity = "";
            transcribtion.BaptismCity = "";
            transcribtion.BirthCountry = "";
            transcribtion.BurialCity = "";
            transcribtion.ConfirmationAge = "";
            transcribtion.ConfirmationCity = "";
            transcribtion.DepartureCity = "";
            transcribtion.MarriageCity = "";
            transcribtion.VitalCity = "";
            transcribtion.FatherGivenName = "";
            transcribtion.FatherSurname = "";
            transcribtion.FatherNameSuffix = "";
            transcribtion.FatherGivenNameAlias = "";
            transcribtion.FatherSurnameAlias = "";
            transcribtion.MotherNamePrefix = "";
            transcribtion.MotherGivenName = "";
            transcribtion.MotherSurname = "";
            transcribtion.MotherNameSuffix = "";
            transcribtion.MotherMaidenName = "";
            transcribtion.MotherGivenNameAlias = "";
            transcribtion.MotherSurnameAlias = "";
            transcribtion.MotherResidenceAge = "";
            transcribtion.SpouseNamePrefix = "";
            transcribtion.SpouseGivenName = "";
            transcribtion.SpouseSurname = "";
            transcribtion.SpouseNameSuffix = "";
            transcribtion.SpouseMaidenName = "";
            transcribtion.SpouseGivenNameAlias = "";
            transcribtion.SpouseSurnameAlias = "";
            transcribtion.SpouseGender = "";
            transcribtion.SpouseMarriageAge = "";
            transcribtion.SpouseBirthDay = "";
            transcribtion.SpouseBirthMonth = "";
            transcribtion.SpouseBirthYear = "";
            transcribtion.SpouseBirthPlace = "";
            transcribtion.FatherInLawGivenName = "";
            transcribtion.FatherInLawSurname = "";
            transcribtion.FatherInLawNameSuffix = "";
            transcribtion.FatherInLawGivenNameAlias = "";
            transcribtion.FatherInLawSurnameAlias = "";
            transcribtion.MotherInLawNamePrefix = "";
            transcribtion.MotherInLawGivenName = "";
            transcribtion.MotherInLawSurname = "";
            transcribtion.MotherInLawNameSuffix = "";
            transcribtion.MotherInLawMaidenName = "";
            transcribtion.MotherInLawGivenNameAlias = "";
            transcribtion.MotherInLawSurnameAlias = "";
            transcribtion.image_id = "197";
            transcribtion.folder_id = "0";
            transcribtion.image_appearance = "4.0";
            transcribtion.event_type = "burial";
            transcribtion.role = "main";
            transcribtion.event_persons = "1";
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

            //TODO: I dont believe these values are correct, but in the test values sheet they were all blank
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
using Linklives.Domain;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace linklives_lib_test
{
    public class MappingTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private ExpandoObject GetTranscriptionExpandoObjectFromCommaSeparatedStrings(string headers, string row)
        {
            var properties = headers.Split(",");
            var values = row.Split(",");

            if (properties.Length != values.Length)
            {
                throw new ArgumentException($"Number of header and row columns does not match ({properties.Length} vs {values.Length})");
            }
            var transcription = new ExpandoObject();
            IDictionary<string, object> propertyValues = (IDictionary<string, object>)transcription;
            int count = 0;
            foreach (string property in properties)
            {
                propertyValues.TryAdd(property, values[count].Replace(";",","));
                count++;
            }

            return transcription;
        }

        [Test]
        public void CBP_MapsCorrectly()
        {
            #region input data
            //10,CBP,standardized_sources/CBP/CBP.csv,transcribed_sources/CBP/CBP_20210309.csv,Københavns Stadsarkiv,https://kbharkiv.dk/brug-samlingerne/kilder-paa-nettet/begravelser-i-koebenhavn/
            var source = new Source
            {
                Source_id = 10,
                Source_name = "CBP",
                File_reference = "standardized_sources/CBP/CBP.csv",
                Original_data_reference = "transcribed_sources/CBP/CBP_20210309.csv",
                Institution_origin = "Københavns Stadsarkiv",
                Link = "https://kbharkiv.dk/brug-samlingerne/kilder-paa-nettet/begravelser-i-koebenhavn/"
            };

            // Standardized burial: LL_data_v1.0\LL_data\standardized_sources\CBP\CBP.csv
            var standardBurialStr = "54, kraa,krak,,,krak,,,krak,,m,,,,1909,,,1909-01-10,1909,1,10,,,,,,,,,,,,,,,,,deceased,burial_protocol,3,,3851";
            var standardizedPA = StandardPA.FromCommaSeparatedString(standardBurialStr);

            // Transcribed burial: transcribed_data_v1.0\transcribed_sources\CBP\CBP_20210309.csv
            var transcribedBurialHeaderStr = "pa_id,id,number,firstnames,lastname,birthname,ageYears,ageMonth,ageWeeks,ageDays,ageHours,dateOfBirth,dateOfDeath,yearOfBirth,deathplace,civilstatus,adressOutsideCph,sex,comment,cemetary,chapel,parish,street,hood,street_unique,street_number,letter,floor,institution,institution_street,institution_hood,institution_street_unique,institution_street_number,positions,relationtypes,workplaces,deathcauses";
            var transcribedBurialStr = "54,548,3851.0,Dødfødt,Kraa,,0.0,,,,,,1909-10-01,1909.0,,,,Mand,,Assistens Kirkegård,Hjemmet (bopælen),Fredens,Helgesensgade,Østerbro,Helgesensgade,,,,,,,,,\"Barber;Frisør (Friseur)\",\"Fars erhverv; Fars erhverv\",,Dødfødt";

            var transcribedPA = GetTranscriptionExpandoObjectFromCommaSeparatedStrings(transcribedBurialHeaderStr, transcribedBurialStr);

            #endregion

            var transcribed = new TranscribedPA(transcribedPA, source.Source_id);
            var finishedPa = BasePA.Create(source, standardizedPA, transcribed);

            //TODO: These asserts are based on the values found in the testing sheet, but looking at them i am not confident that that are correct to the mapping defined in the mapping sheet and consequently this test is unlikely to provide much value in its current form.
            Assert.AreEqual(finishedPa.Pa_id, 54);

            Assert.AreEqual("kraa", finishedPa.Name_searchable);
            Assert.AreEqual("kraa krak", finishedPa.Name_searchable_fz);
            Assert.AreEqual("kraa", finishedPa.Lastname_searchable);
            Assert.AreEqual("kraa krak", finishedPa.Lastname_searchable_fz);
            //Assert.AreEqual(null, finishedPa.Firstnames_searchable);
            //Assert.AreEqual(null, finishedPa.Firstnames_searchable_fz);
            Assert.AreEqual(1909, finishedPa.Birthyear_searchable);
            Assert.AreEqual("1906 1907 1908 1909 1910 1911 1912", finishedPa.Birthyear_searchable_fz);
            Assert.AreEqual(null, finishedPa.Birthplace_searchable);
            Assert.AreEqual(1909, finishedPa.Sourceyear_searchable);
            Assert.AreEqual("1906 1907 1908 1909 1910 1911 1912", finishedPa.Sourceyear_searchable_fz);
            Assert.AreEqual("København", finishedPa.Sourceplace_searchable);
            Assert.AreEqual(1909, finishedPa.Deathyear_searchable);
            Assert.AreEqual("1906 1907 1908 1909 1910 1911 1912", finishedPa.Deathyear_searchable_fz);
            Assert.AreEqual("Mand", finishedPa.Gender_searchable);
            Assert.AreEqual(null, finishedPa.Birthname_searchable);
            Assert.AreEqual(null, finishedPa.Occupation_searchable);
            //TODO: The test values sheet list a couple of extra fields that are neither implemented in BasePa nor present in the mapping sheet.
            /*
                eventyear_searchable
                eventyear_searchable_fz
                role_searchable
            */

            //Assert.AreEqual(null, finishedPa.First_names_sortable);
            Assert.AreEqual("kraa", finishedPa.Family_names_sortable);
            Assert.AreEqual(1909, finishedPa.Birthyear_sortable);

            //TODO: I dont believe these values are correct, but in the test values sheet they were all blank
            //Assert.AreEqual("", finishedPa.Pa_entry_permalink_wp4);
            //Assert.AreEqual("", finishedPa.Last_updated_wp4);
            //Assert.AreEqual("", finishedPa.Source_type_wp4);

            Assert.AreEqual("Kraa", finishedPa.Name_display);
            Assert.AreEqual(1909, finishedPa.Birthyear_display);
            Assert.AreEqual("Afdøde", finishedPa.Role_display);
            Assert.AreEqual(null, finishedPa.Birthplace_display);
            Assert.AreEqual("Fars erhverv Barber, Fars erhverv Frisør (Friseur)", finishedPa.Occupation_display);
            Assert.AreEqual("København", finishedPa.Sourceplace_display);
            Assert.AreEqual("Begravelse", finishedPa.Event_type_display);
            Assert.AreEqual("1909", finishedPa.Sourceyear_display);
            //Assert.AreEqual("1906", finishedPa.Event_year_display);
            Assert.AreEqual(1909, finishedPa.Deathyear_display);
            Assert.AreEqual("Begravelsesprotokol", finishedPa.Source_type_display);
            Assert.AreEqual("Københavns Stadsarkiv", finishedPa.Source_archive_display);
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

            // Standardized burial: LL_data_v1.0\LL_data\standardized_sources\PR\burial\burial.csv
            var standardBurialStr = "4,christa magdalene jensen krage,krista magdalene jensen krag,,jensen,krista krag,,jensen,krista krag,magdalene,f,,3.0,1903-10-24,1903,10,24,1906-02-02,1906,2,2,,,,,,,,,,tversted,,,hjørring,,,,deceased,burial,0,0,1";
            var standardizedPA = StandardPA.FromCommaSeparatedString(standardBurialStr);

            // Transcribed burial: transcribed_data_v1.0\transcribed_sources\PR\by_event\burial.csv
            var transcribedBurialHeaderStr = "pa_id,event_id,ImageFileName,ImageFolder,unique_identifier,SequenceNumber,SourceCollectionID,SourceDescription,SourceReferenceNumber,SourceComments,SourceYearRange,BrowseLevel,BrowseLevel1,BrowseLevel2,KeyedParish,Notes,NamePrefix,GivenName,Surname,NameSuffix,GivenNameAlias,SurnameAlias,MaidenName,Gender,BirthDay,BirthMonth,BirthYear,BirthPlace,BirthParish,BirthMunicipality,BirthState,BaptismAge,BaptismDay,BaptismMonth,BaptismYear,BaptismPlace,BaptismParish,BaptismMunicipality,BaptismCounty,BaptismState,BaptismCountry,ConfirmationDay,ConfirmationMonth,ConfirmationYear,ConfirmationPlace,ConfirmationParish,ConfirmationMunicipality,ConfirmationCounty,ConfirmationState,ConfirmationCountry,ArrivalDay,ArrivalMonth,ArrivalYear,ArrivalAge,ArrivalPlace,DeparturePlace,ArrivalParish,ArrivalMunicipality,ArrivalCounty,ArrivalState,ArrivalCountry,DepartureDay,DepartureMonth,DepartureYear,DepartureAge,DepartureParish,DepartureMunicipality,DepartureCounty,DepartureState,DepartureCountry,MarriageDay,MarriageMonth,MarriageYear,MarriageAge,MarriagePlace,MarriageParish,MarriageMunicipality,MarriageCounty,MarriageState,MarriageCountry,DeathDay,DeathMonth,DeathYear,DeathAge,DeathPlace,DeathParish,DeathMunicipality,DeathState,BurialDay,BurialMonth,BurialYear,BurialAge,BurialPlace,BurialParish,BurialMunicipality,BurialCounty,BurialState,BurialCountry,ResidenceAge,ResidenceParish,ResidenceMunicipality,ResidenceCounty,ReligiousDay,ReligiousMonth,ReligiousYear,ReligiousAge,ReligiousParish,ReligiousMunicipality,ReligiousState,VitalDay,VitalMonth,VitalYear,VitalAge,VitalPlace,VitalParish,VitalMunicipality,VitalState,VitalCountry,VitalCounty,VitalTownship,ArrivalCity,BaptismCity,BirthCountry,BurialCity,ConfirmationAge,ConfirmationCity,DepartureCity,MarriageCity,VitalCity,FatherGivenName,FatherSurname,FatherNameSuffix,FatherGivenNameAlias,FatherSurnameAlias,MotherNamePrefix,MotherGivenName,MotherSurname,MotherNameSuffix,MotherMaidenName,MotherGivenNameAlias,MotherSurnameAlias,MotherResidenceAge,SpouseNamePrefix,SpouseGivenName,SpouseSurname,SpouseNameSuffix,SpouseMaidenName,SpouseGivenNameAlias,SpouseSurnameAlias,SpouseGender,SpouseMarriageAge,SpouseBirthDay,SpouseBirthMonth,SpouseBirthYear,SpouseBirthPlace,FatherInLawGivenName,FatherInLawSurname,FatherInLawNameSuffix,FatherInLawGivenNameAlias,FatherInLawSurnameAlias,MotherInLawNamePrefix,MotherInLawGivenName,MotherInLawSurname,MotherInLawNameSuffix,MotherInLawMaidenName,MotherInLawGivenNameAlias,MotherInLawSurnameAlias,image_id,folder_id,image_appearance,event_type,role,event_persons";
            var transcribedBurialStr = "4,1,0211A-DK,48491_b411239,2809047.0,1010.0,,,AOdata01\\Kirkeboeger1892\\C050B\\G\\002,,1906-1921,Hjørring Amt,Tversted Sogn,1906-1921,Tversted Kirkegaard,,,Christa Magdalene,Jensen Krage,,,,,Kvinde,24.0,okt,1903.0,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,28.0,jan,1906.0,,Vester Tversted Tversted Sogn Horns Herred,,,,2.0,febr,1906.0,2,\"Tversted Kirkegaard; Denmark\",Tversted,,Hjørring,Hjørring Amt,Danmark,,,,,,,,,,,,2.0,febr,1906,2,,Tversted Sogn,,Hjørring Amt,,Hjørring,,,,,,,,,,,Christian,Jensen Krage,,,,,Martine,Larsen,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,0,0,1.0,burial,main,3";

            var transcribedPA = GetTranscriptionExpandoObjectFromCommaSeparatedStrings(transcribedBurialHeaderStr, transcribedBurialStr);

            #endregion

            var transcribed = new TranscribedPA(transcribedPA, source.Source_id);
            var finishedPa = BasePA.Create(source, standardizedPA, transcribed);


            
            Assert.AreEqual(finishedPa.Pa_id, 4);

            Assert.AreEqual("christa magdalene jensen krage", finishedPa.Name_searchable);
            Assert.AreEqual("christa magdalene jensen krage krista magdalene jensen krag", finishedPa.Name_searchable_fz);
            Assert.AreEqual("krage", finishedPa.Lastname_searchable);
            Assert.AreEqual("krage jensen krista krag", finishedPa.Lastname_searchable_fz);
            Assert.AreEqual("christa magdalene jensen", finishedPa.Firstnames_searchable);
            Assert.AreEqual("christa magdalene jensen", finishedPa.Firstnames_searchable_fz);
            Assert.AreEqual(1903, finishedPa.Birthyear_searchable);
            Assert.AreEqual("1900 1901 1902 1903 1904 1905 1906", finishedPa.Birthyear_searchable_fz);
            Assert.AreEqual(null, finishedPa.Birthplace_searchable);
            Assert.AreEqual(1906, finishedPa.Sourceyear_searchable);
            Assert.AreEqual("1903 1904 1905 1906 1907 1908 1909", finishedPa.Sourceyear_searchable_fz);
            Assert.AreEqual("tversted hjørring", finishedPa.Sourceplace_searchable);
            Assert.AreEqual(1906,finishedPa.Deathyear_searchable);
            Assert.AreEqual("1903 1904 1905 1906 1907 1908 1909", finishedPa.Deathyear_searchable_fz);
            Assert.AreEqual("kvinde", finishedPa.Gender_searchable);
            Assert.AreEqual(null, finishedPa.Birthname_searchable);
            Assert.AreEqual(null, finishedPa.Occupation_searchable);
            //TODO: The test values sheet list a couple of extra fields that are neither implemented in BasePa nor present in the mapping sheet.

            Assert.AreEqual("christa magdalene jensen", finishedPa.First_names_sortable);
            Assert.AreEqual("krage", finishedPa.Family_names_sortable);
            Assert.AreEqual(1903, finishedPa.Birthyear_sortable);

            //TODO: I dont believe these values are correct, but in the test values sheet they were all blank
            Assert.AreEqual(finishedPa.Pa_entry_permalink_wp4, "");
            Assert.AreEqual(typeof(DateTime), finishedPa.Last_updated_wp4.GetType());
            Assert.AreEqual("parish_register", finishedPa.Source_type_wp4);

            Assert.AreEqual("Christa Magdalene Jensen Krage", finishedPa.Name_display);
            Assert.AreEqual(1903, finishedPa.Birthyear_display);
            Assert.AreEqual("Afdøde", finishedPa.Role_display);
            Assert.AreEqual(null, finishedPa.Birthplace_display);
            Assert.AreEqual(null, finishedPa.Occupation_display);
            Assert.AreEqual("Tversted Sogn, Hjørring Amt", finishedPa.Sourceplace_display);
            Assert.AreEqual("Begravelse", finishedPa.Event_type_display);
            Assert.AreEqual("1906-1921", finishedPa.Sourceyear_display);
            Assert.AreEqual("1906", finishedPa.Event_year_display);
            Assert.AreEqual(1906, finishedPa.Deathyear_display);
            Assert.AreEqual("Kirkebog", finishedPa.Source_type_display);
            Assert.AreEqual("Rigsarkivet", finishedPa.Source_archive_display);
        }
    }
}
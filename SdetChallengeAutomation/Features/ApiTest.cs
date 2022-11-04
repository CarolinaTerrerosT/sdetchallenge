using Dynamitey.DynamicObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using SdetChallengeAutomation.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace SdetChallengeAutomation.Features
{
    [TestClass]
    public class ApiTest
    {
        [TestMethod]

        /*Story 1: As a user of the API I want to list all measurements taken by the station on Oak Street  in json format.
            GIVEN BEACH WEATHER STATION SENSOR “OAK STREET”
            WHEN THE USER REQUESTS STATION DATA
            THEN ALL DATA MEASUREMENTS CORRESPOND TO ONLY THAT STATION*/
        public void List_Station_ReturnsAllMeasurements()
        {
            var client = new RestClient("https://data.cityofchicago.org/resource/k7hf-8y75.json");

            var request = new RestRequest("", Method.Get)
                .AddParameter("station_name", "Oak Street")
                .AddParameter("$$app_token", "ZwPceGweiii8qzm2P4epN9yau");

            var content = client.Execute(request).Content;

            var weatherMeasurements = JsonSerializer.Deserialize<List<WeatherMeasurement>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            foreach (var weatherMeasurement in weatherMeasurements)
            {
                Assert.AreEqual(weatherMeasurement.station_name, "Oak Street");
            }
        }

        /* Story 2: As a user of the API I want to be able to page through JSON data sets of 2019 taken by
            the sensor on 63rd Street.
            GIVEN THE BEACH WEATHER STATION ON 63RD STREET’S SENSOR DATA OF 2019
            WHEN THE USER REQUESTS DATA FOR THE FIRST 10 MEASUREMENTS
            AND THE SECOND PAGE OF 10 MEASUREMENTS
            THEN THE RETURNED MEASUREMENTS OF BOTH PAGES SHOULD NOT REPEAT*/
        [TestMethod]
        public void ListMeasurements_ComparePages_DataDoesNotRepeat()
        {
            var client = new RestClient("https://data.cityofchicago.org/resource/k7hf-8y75.json");

            var request = new RestRequest("", Method.Get)
               .AddParameter("station_name", "63rd Street Weather Station")
               .AddParameter("$$app_token", "ZwPceGweiii8qzm2P4epN9yau")
               .AddParameter("$where", "measurement_timestamp>'2019-01-01T00:00:00.000' AND measurement_timestamp<'2020-01-01T00:00:00.000'")
               .AddParameter("$order", "measurement_timestamp ASC")
               .AddParameter("$limit", "10")
               .AddParameter("$offset", "0");

            var content = client.Execute(request).Content;

            var weatherMeasurementsFirstPage = JsonSerializer.Deserialize<List<WeatherMeasurement>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var requestSecondPage = new RestRequest("", Method.Get)
                .AddParameter("station_name", "63rd Street Weather Station")
                .AddParameter("$$app_token", "ZwPceGweiii8qzm2P4epN9yau")
                .AddParameter("$where", "measurement_timestamp>'2019-01-01T00:00:00.000' AND measurement_timestamp<'2020-01-01T00:00:00.000'")
                .AddParameter("$order", "measurement_timestamp ASC")
                .AddParameter("$limit", "10")
                .AddParameter("$offset", "10");

            var contentSecondPage = client.Execute(requestSecondPage).Content;

            var weatherMeasurementsSecondPage = JsonSerializer.Deserialize<List<WeatherMeasurement>>(contentSecondPage,
                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var firstPageList = weatherMeasurementsFirstPage.Except(weatherMeasurementsSecondPage).ToList();

            Assert.AreEqual(firstPageList.Count, weatherMeasurementsFirstPage.Count);

        }

    }

}       /*Story 3: As a user of the API I expect a SoQL query to fail with an error message if I
             search using a malformed query. Note: This is a negative test. We want to make sure that 
            the API throws an error when expected.
            GIVEN ALL BEACH WEATHER STATION SENSOR DATA OF THE STATION ON 63RD STREET
            WHEN THE USER REQUESTS SENSOR DATA BY QUERYING BATTERY_LIFE VALUES THAT ARE LESS THAN THE
            TEXT “FULL” ($WHERE=BATTERY_LIFE < FULL)
            THEN AN ERROR CODE “MALFORMED COMPILER” WITH MESSAGE “COULD NOT PARSE SOQL QUERY” IS
            RETURNED*/
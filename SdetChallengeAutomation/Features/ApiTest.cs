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
                Assert.AreEqual("Oak Street", weatherMeasurement.station_name);
            }
        } 

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

            Assert.AreEqual(weatherMeasurementsFirstPage.Count, firstPageList.Count);

        }


        [TestMethod]
        public void SendRequest_WithMalformedQuery_ReturnErrorMessage()
        {
            var client = new RestClient("https://data.cityofchicago.org/resource/k7hf-8y75.json");

            var request = new RestRequest("", Method.Get)

                .AddParameter("$$app_token", "ZwPceGweiii8qzm2P4epN9yau")
                .AddParameter("$where", "battery_life < full");

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {

                var error = JsonSerializer.Deserialize<Error>(response.Content,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.AreEqual("query.compiler.malformed", error.code);
                Assert.IsTrue(error.message.Contains("Could not parse SoQL query"));
            }


        }

    }
}
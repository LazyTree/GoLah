﻿using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using System.Collections.Generic;
using GoLah.Model;

namespace GoLah.Services.Tests
{
    [TestClass]
    public class LtaDataRepositoryTests
    {
        [TestMethod]
        public async Task GetBusStopssAsync()
        {
            var repository = new LtaDataRepository<BusStop>();
            var result = new List<BusStop>(await repository.QueryAsync());
            Assert.IsTrue(result.Count > 5000);
        }

        [TestMethod]
        public async Task GetBusServicesAsync()
        {
            var repository = new LtaDataRepository<BusRoute>();
            var result = new List<BusRoute>(await repository.QueryAsync());
            Assert.IsTrue(result.Count > 200);
        }

        [TestMethod]
        public async Task GetNextBusAsync()
        {
            var repository = new LtaDataRepositoryBase<ArrivalBusService>();
            var stopId = "83319";
            var result = new List<ArrivalBusService>(await repository.QueryAsync(true, stopId));
            Assert.IsTrue(result.Count == 3);
            Assert.AreEqual("13", result[0].ServiceNo);
            Assert.AreEqual("55", result[1].ServiceNo);
            Assert.AreEqual("966", result[2].ServiceNo);
        }


    }
}

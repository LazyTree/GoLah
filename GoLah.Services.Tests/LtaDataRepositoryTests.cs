using System;
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
            var repository = new LtaDataRepository();
            var result = new List<BusStop>(await repository.GetBusStopsAsync());
            Assert.AreEqual(50, result.Count);
        }

        [TestMethod]
        public async Task GetBusServicesAsync()
        {
            var repository = new LtaDataRepository();
            var result = new List<BusService>(await repository.GetBusServicesAsync());
            Assert.AreEqual(50, result.Count);
        }

        [TestMethod]
        public async Task GetNextBusAsync()
        {
            var repository = new LtaDataRepository();
            var stopId = "83319";
            var result = new List<ArrivalBusService>(await repository.GetNextBusAsync(stopId));
            Assert.IsTrue(result.Count == 3);
            Assert.AreEqual("13", result[0].ServiceNo);
            Assert.AreEqual("55", result[1].ServiceNo);
            Assert.AreEqual("966", result[2].ServiceNo);
        }


    }
}

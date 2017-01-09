using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace GoLah.Services.Tests
{
    [TestClass]
    public class LtaDataRepositoryTests
    {
        [TestMethod]
        public async Task GetBusStopssAsync()
        {
            var repository = new LtaDataRepository();
            var result = await repository.GetBusStopsAsync();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetBusServicesAsync()
        {
            var repository = new LtaDataRepository();
            var result = await repository.GetBusServicesAsync();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetNextBusAsync()
        {
            var repository = new LtaDataRepository();
            var stopId = "83319";
            var result = await repository.GetNextBusAsync(stopId);
            Assert.IsNotNull(result);
        }


    }
}

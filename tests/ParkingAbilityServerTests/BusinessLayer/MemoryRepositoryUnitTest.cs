using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ParkingAbilityServer.BusinessLayer;
using ParkingAbilityServer.Models;
using System.IO;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServerTests.BusinessLayer
{
    [TestClass]
    public class MemoryRepositoryUnitTest
    {
        private MemoryContentRepository repository;

        [TestInitialize]
        public void TestInitialize()
        {
            var environment = new Mock<IWebHostEnvironment>();

            environment
                .Setup(e => e.WebRootPath)
                .Returns(@"..\..\..\..\..\src\wwwroot")
                .Verifiable();
            repository = new MemoryContentRepository(environment.Object);
        }

        [TestMethod, TestCategory("L0")]
        [DataRow("WA")]
        [DataRow("Seattle")]
        public async Task LoadAsyncViewModelValidationAsync(string id)
        {
            var viewModel = await repository.LoadAsync(id);
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(id, viewModel.Id);
            Assert.IsNotNull(viewModel.ContentUrl);
            Assert.IsNotNull(viewModel.SourceUrl);
            Assert.IsNotNull(viewModel.ReadContent());
        }

        [TestMethod, TestCategory("L0")]
        [DataRow("Wa")]
        [DataRow("SeattlE")]
        [DataRow("")]
        [DataRow(null)]
        public async Task LoadAsyncViewModelNullAsync(string id)
        {
            LocaleViewModel viewModel = await repository.LoadAsync(id);
            Assert.IsNull(viewModel);
        }
    }
}

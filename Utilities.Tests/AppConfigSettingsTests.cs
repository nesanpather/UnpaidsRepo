using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace Utilities.Tests
{
    public class AppConfigSettingsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetAppConfigSettings_GivenKey_Returns_SettingValue()
        {
            // Arrange. 
            var config = Substitute.For<IConfiguration>();

            config["app:test"] = "Utilities";
            var appSettings = new AppConfigSettings(config);

            // Act.
            var actual = appSettings["app:test"];

            // Assert.
            Assert.AreEqual("Utilities", actual);
        }

        [Test]
        public void GetAppConfigSettings_GivenEmptyKey_Returns_Null()
        {
            // Arrange.
            var config = Substitute.For<IConfiguration>();

            config[null] = "";
            var appSettings = new AppConfigSettings(config);

            // Act.
            var actual = appSettings[""];

            // Assert.
            Assert.AreEqual(string.Empty, actual);
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UnpaidModels;

namespace DataManager.Tests
{
    public class UnpaidDataManagerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AddUnpaidAsync_GIVEN_Valid_Unpaid_RETURNS_Valid_Task()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "Test1234",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back."
                    }
                });
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(1, context.Unpaids.Count());
                Assert.AreEqual("Test1234", context.Unpaids.Single().PolicyNumber);
                Assert.AreEqual("Nesan Pather", context.Unpaids.Single().Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.Single().Message);
            }
        }

        [Test]
        public async Task AddUnpaidAsync_CalledTwice_GIVEN_Valid_Unpaid_RETURNS_Valid_Task_With2Entries()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database2")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "Test1234",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back."
                    },
                    new Unpaid
                    {
                        PolicyNumber = "Test12345",
                        Name = "Tom Smith",
                        Message = "Payment bounced. Please accept a call back."
                    }
                });
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(2, context.Unpaids.Count());
                Assert.AreEqual("Test1234", context.Unpaids.ToList()[0].PolicyNumber);
                Assert.AreEqual("Nesan Pather", context.Unpaids.ToList()[0].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.ToList()[0].Message);
                Assert.AreEqual("Test12345", context.Unpaids.ToList()[1].PolicyNumber);
                Assert.AreEqual("Tom Smith", context.Unpaids.ToList()[1].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.ToList()[1].Message);
            }
        }

        [Test]
        public async Task AddUnpaidAsync_GIVEN_Null_Unpaid_RETURNS_Valid_Task()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database3")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(null);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(0, context.Unpaids.Count());
            }
        }
    }
}
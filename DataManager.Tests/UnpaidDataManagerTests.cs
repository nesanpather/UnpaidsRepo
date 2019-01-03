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
        public async Task AddUnpaidAsync_GIVEN_Valid_Unpaid_RETURNS_Valid_Result()
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

            options = null;
        }

        [Test]
        public async Task AddUnpaidAsync_2Entries_GIVEN_Valid_Unpaid_RETURNS_Valid_Result_With2Entries()
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
        public async Task AddUnpaidAsync_GIVEN_Null_Unpaid_RETURNS_Valid_Result()
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

        [Test]
        public async Task GetSingleUnpaidAsync_GIVEN_Valid_Input_RETURNS_Valid_Unpaid()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaid")
                .Options;
            
            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsDBContext(options))
            {
                context.Unpaids.Add(new Unpaid { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", Message = "Test Message 2." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 3, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 3." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 4, PolicyNumber = "P4", Name = "Brad", Message = "Test Message 4." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetSingleUnpaidAsync(3);
                Assert.AreEqual(3, actual.UnpaidId);
                Assert.AreEqual("P1", actual.PolicyNumber);
                Assert.AreEqual("Tom", actual.Name);
                Assert.AreEqual("Test Message 3.", actual.Message);
            }
        }

        [Test]
        public async Task GetSingleUnpaidAsync_GIVEN_Invalid_Input_RETURNS_Null()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaid2")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsDBContext(options))
            {
                context.Unpaids.Add(new Unpaid { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetSingleUnpaidAsync(0);
                Assert.AreEqual(null, actual);
            }
        }

        [Test]
        public async Task GetAllUnpaidAsync_RETURNS_Valid_Unpaid_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaids")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsDBContext(options))
            {
                context.Unpaids.Add(new Unpaid { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", Message = "Test Message 2." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 3, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 3." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 4, PolicyNumber = "P4", Name = "Brad", Message = "Test Message 4." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetAllUnpaidAsync();
                Assert.AreEqual(4, actual.Count());
            }
        }

        [Test]
        public async Task GetAllUnpaidAsync_GIVEN_Valid_Input_RETURNS_Valid_Unpaid_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaids_against_policynumber")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsDBContext(options))
            {
                context.Unpaids.Add(new Unpaid { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", Message = "Test Message 2." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 3, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 3." });
                context.Unpaids.Add(new Unpaid { UnpaidId = 4, PolicyNumber = "P4", Name = "Brad", Message = "Test Message 4." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetAllUnpaidAsync("p1");
                Assert.AreEqual(2, actual.Count());
                Assert.AreEqual(1, actual.ToList()[0].UnpaidId);
                Assert.AreEqual(3, actual.ToList()[1].UnpaidId);
            }
        }
    }
}
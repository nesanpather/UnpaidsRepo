using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using DataManager.Models;
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
                var actual = await service.AddUnpaidAsync(new List<UnpaidDb>
                {
                    new UnpaidDb
                    {
                        PolicyNumber = "Test1234",
                        IdNumber = "9009165023080",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back.",
                        IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(1, context.Unpaids.Count());
                Assert.AreEqual("Test1234", context.Unpaids.Single().PolicyNumber);
                Assert.AreEqual("9009165023080", context.Unpaids.Single().IdNumber);
                Assert.AreEqual("Nesan Pather", context.Unpaids.Single().Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.Single().Message);
                Assert.AreEqual("7c9e6679-7425-40de-944b-e07fc1f90ae7", context.Unpaids.Single().IdempotencyKey);
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
                var actual = await service.AddUnpaidAsync(new List<UnpaidDb>
                {
                    new UnpaidDb
                    {
                        PolicyNumber = "Test1234",
                        IdNumber = "9009165023080",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back.",
                        IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                    },
                    new UnpaidDb
                    {
                        PolicyNumber = "Test12345",
                        IdNumber = "9009165023081",
                        Name = "Tom Smith",
                        Message = "Payment bounced. Please accept a call back.",
                        IdempotencyKey = "0f8fad5b-d9cb-469f-a165-70867728950e"
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(2, context.Unpaids.Count());
                Assert.AreEqual("Test1234", context.Unpaids.ToList()[0].PolicyNumber);
                Assert.AreEqual("9009165023080", context.Unpaids.ToList()[0].IdNumber);
                Assert.AreEqual("Nesan Pather", context.Unpaids.ToList()[0].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.ToList()[0].Message);
                Assert.AreEqual("7c9e6679-7425-40de-944b-e07fc1f90ae7", context.Unpaids.ToList()[0].IdempotencyKey);
                Assert.AreEqual("Test12345", context.Unpaids.ToList()[1].PolicyNumber);
                Assert.AreEqual("9009165023081", context.Unpaids.ToList()[1].IdNumber);
                Assert.AreEqual("Tom Smith", context.Unpaids.ToList()[1].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.Unpaids.ToList()[1].Message);
                Assert.AreEqual("0f8fad5b-d9cb-469f-a165-70867728950e", context.Unpaids.ToList()[1].IdempotencyKey);
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
                var actual = await service.AddUnpaidAsync(null, CancellationToken.None);
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
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", IdempotencyKey = "0f8fad5b-d9cb-469f-a165-70867728950e" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", IdempotencyKey = "1f9fad5b-d9cb-469f-a165-70867728950e" });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetSingleUnpaidAsync(3, CancellationToken.None);
                Assert.AreEqual(3, actual.UnpaidId);
                Assert.AreEqual("P1", actual.PolicyNumber);
                Assert.AreEqual("9009165023080", actual.IdNumber);
                Assert.AreEqual("Tom", actual.Name);
                Assert.AreEqual("Test Message 3.", actual.Message);
                Assert.AreEqual("7c9e6679-7425-40de-944b-e07fc1f90ae7", actual.IdempotencyKey);
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
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetSingleUnpaidAsync(0, CancellationToken.None);
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
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", IdempotencyKey = "0f8fad5b-d9cb-469f-a165-70867728950e" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", IdempotencyKey = "1f9fad5b-d9cb-469f-a165-70867728950e" });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetAllUnpaidAsync(CancellationToken.None);
                Assert.AreEqual(4, actual.Count());
            }
        }

        [Test]
        public async Task GetAllUnpaidAsync_GIVEN_Valid_Input_RETURNS_Valid_Unpaid_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaids_against_idempotencyKey")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsDBContext(options))
            {
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", IdempotencyKey = "0f8fad5b-d9cb-469f-a165-70867728950e" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7" });
                context.Unpaids.Add(new UnpaidDb { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", IdempotencyKey = "1f9fad5b-d9cb-469f-a165-70867728950e" });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetAllUnpaidAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);
                var unpaidDbs = actual.ToList();

                Assert.AreEqual(2, unpaidDbs.Count);
                Assert.AreEqual(1, unpaidDbs.ToList()[0].UnpaidId);
                Assert.AreEqual(3, unpaidDbs.ToList()[1].UnpaidId);
            }
        }
    }
}
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
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;
            
            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(new List<TbUnpaid>
                {
                    new TbUnpaid
                    {
                        PolicyNumber = "Test1234",
                        IdNumber = "9009165023080",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back.",
                        Title = "Test1"
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(1, context.TbUnpaid.Count());
                Assert.AreEqual("Test1234", context.TbUnpaid.Single().PolicyNumber);
                Assert.AreEqual("9009165023080", context.TbUnpaid.Single().IdNumber);
                Assert.AreEqual("Nesan Pather", context.TbUnpaid.Single().Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.TbUnpaid.Single().Message);
                Assert.AreEqual("Test1", context.TbUnpaid.Single().Title);
            }

            options = null;
        }

        [Test]
        public async Task AddUnpaidAsync_2Entries_GIVEN_Valid_Unpaid_RETURNS_Valid_Result_With2Entries()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database2")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(new List<TbUnpaid>
                {
                    new TbUnpaid
                    {
                        PolicyNumber = "Test1234",
                        IdNumber = "9009165023080",
                        Name = "Nesan Pather",
                        Message = "Payment bounced. Please accept a call back.",
                        Title = "Test1"
                    },
                    new TbUnpaid
                    {
                        PolicyNumber = "Test12345",
                        IdNumber = "9009165023081",
                        Name = "Tom Smith",
                        Message = "Payment bounced. Please accept a call back.",
                        Title = "Test2"
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(2, context.TbUnpaid.Count());
                Assert.AreEqual("Test1234", context.TbUnpaid.ToList()[0].PolicyNumber);
                Assert.AreEqual("9009165023080", context.TbUnpaid.ToList()[0].IdNumber);
                Assert.AreEqual("Nesan Pather", context.TbUnpaid.ToList()[0].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.TbUnpaid.ToList()[0].Message);
                Assert.AreEqual("Test1", context.TbUnpaid.ToList()[0].Title);
                Assert.AreEqual("Test12345", context.TbUnpaid.ToList()[1].PolicyNumber);
                Assert.AreEqual("9009165023081", context.TbUnpaid.ToList()[1].IdNumber);
                Assert.AreEqual("Tom Smith", context.TbUnpaid.ToList()[1].Name);
                Assert.AreEqual("Payment bounced. Please accept a call back.", context.TbUnpaid.ToList()[1].Message);
                Assert.AreEqual("Test2", context.TbUnpaid.ToList()[1].Title);
            }
        }

        [Test]
        public async Task AddUnpaidAsync_GIVEN_Null_Unpaid_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database3")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.AddUnpaidAsync(null, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(0, context.TbUnpaid.Count());
            }
        }

        [Test]
        public async Task GetSingleUnpaidAsync_GIVEN_Valid_Input_RETURNS_Valid_Unpaid()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaid")
                .Options;
            
            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", Title = "Test1" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", Title = "Test2" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", Title = "Test3" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", Title = "Test4" });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidDataManager(context);
                var actual = await service.GetSingleUnpaidAsync(3, CancellationToken.None);
                Assert.AreEqual(3, actual.UnpaidId);
                Assert.AreEqual("P1", actual.PolicyNumber);
                Assert.AreEqual("9009165023080", actual.IdNumber);
                Assert.AreEqual("Tom", actual.Name);
                Assert.AreEqual("Test Message 3.", actual.Message);
                Assert.AreEqual("Test3", actual.Title);
            }
        }

        [Test]
        public async Task GetSingleUnpaidAsync_GIVEN_Invalid_Input_RETURNS_Null()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaid2")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 1, PolicyNumber = "P1", Name = "Tom", Message = "Test Message 1." });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsContext(options))
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
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaids")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", Title = "Test1" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", Title = "Test2" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", Title = "Test3" });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", Title = "Test4" });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsContext(options))
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
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaids_against_idempotencyKey")
                .Options;

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidBatch.Add(new TbUnpaidBatch{BatchKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7", StatusId = (int) Status.Pending, UserName = "XX", UnpaidBatchId = 1});
                context.TbUnpaidBatch.Add(new TbUnpaidBatch { BatchKey = "7c9e6679-7425-40de-944b-e07fc1f90ae9", StatusId = (int)Status.Pending, UserName = "XX", UnpaidBatchId = 2 });
                context.SaveChanges();
            }

            // Insert seed data into the database using one instance of the context.
            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 1, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 1.", Title = "Test1", UnpaidBatchId = 1});
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 2, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081", Message = "Test Message 2.", Title = "Test2", UnpaidBatchId = 2 });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 3, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom", Message = "Test Message 3.", Title = "Test3", UnpaidBatchId = 1 });
                context.TbUnpaid.Add(new TbUnpaid { UnpaidId = 4, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad", Message = "Test Message 4.", Title = "Test4", UnpaidBatchId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            // Use a clean instance of the context to run the test.
            using (var context = new UnpaidsContext(options))
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
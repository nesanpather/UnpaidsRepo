using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using UnpaidModels;

namespace DataManager.Tests
{
    public class UnpaidResponseDataManagerTests
    {
        [Test]
        public async Task AddUnpaidResponseAsync_GIVEN_Valid_Unpaid_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.AddUnpaidResponseAsync(new List<UnpaidResponse>
                {
                    new UnpaidResponse
                    {
                        UnpaidRequestId = 41,
                        StatusId = 1,
                        ResponseId = 1,
                        Accepted = true
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(1, context.UnpaidResponses.Count());
                Assert.AreEqual(41, context.UnpaidResponses.Single().UnpaidRequestId);
                Assert.AreEqual(1, context.UnpaidResponses.Single().StatusId);
                Assert.AreEqual(1, context.UnpaidResponses.Single().ResponseId);
                Assert.IsTrue(context.UnpaidResponses.Single().Accepted);
            }
        }

        [Test]
        public async Task AddUnpaidResponseAsync_2Entries_GIVEN_Valid_UnpaidResponse_RETURNS_Valid_Result_With2Entries()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database2")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.AddUnpaidResponseAsync(new List<UnpaidResponse>
                {
                    new UnpaidResponse
                    {
                        UnpaidRequestId = 41,
                        StatusId = 1,
                        ResponseId = 1,
                        Accepted = true
                    },
                    new UnpaidResponse
                    {
                        UnpaidRequestId = 55,
                        StatusId = 1,
                        ResponseId = 1,
                        Accepted = false
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(2, context.UnpaidResponses.Count());
                Assert.AreEqual(41, context.UnpaidResponses.ToList()[0].UnpaidRequestId);
                Assert.AreEqual(1, context.UnpaidResponses.ToList()[0].StatusId);
                Assert.AreEqual(1, context.UnpaidResponses.ToList()[0].ResponseId);
                Assert.AreEqual(true, context.UnpaidResponses.ToList()[0].Accepted);
                Assert.AreEqual(55, context.UnpaidResponses.ToList()[1].UnpaidRequestId);
                Assert.AreEqual(1, context.UnpaidResponses.ToList()[1].StatusId);
                Assert.AreEqual(1, context.UnpaidResponses.ToList()[1].ResponseId);
                Assert.AreEqual(false, context.UnpaidResponses.ToList()[1].Accepted);
            }
        }

        [Test]
        public async Task AddUnpaidResponseAsync_GIVEN_Null_UnpaidResponse_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database3")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.AddUnpaidResponseAsync(null, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(0, context.UnpaidResponses.Count());
            }
        }

        [Test]
        public async Task GetSingleUnpaidResponseAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidResponse()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidResponse")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 1, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 2, UnpaidRequestId = 50, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 3, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = false });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 4, UnpaidRequestId = 25, StatusId = 1, ResponseId = 2, Accepted = true });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.GetSingleUnpaidResponseAsync(3, CancellationToken.None);
                Assert.AreEqual(3, actual.UnpaidResponseId);
                Assert.AreEqual(12, actual.UnpaidRequestId);
                Assert.AreEqual(1, actual.StatusId);
                Assert.AreEqual(1, actual.ResponseId);
                Assert.AreEqual(false, actual.Accepted);
            }
        }

        [Test]
        public async Task GetSingleUnpaidResponseAsync_GIVEN_Invalid_Input_RETURNS_Null()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidResponse2")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 1, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = true });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.GetSingleUnpaidResponseAsync(0, CancellationToken.None);
                Assert.AreEqual(null, actual);
            }
        }

        [Test]
        public async Task GetAllUnpaidResponseAsync_RETURNS_Valid_UnpaidResponse_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidResponses")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 1, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 2, UnpaidRequestId = 50, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 3, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = false });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 4, UnpaidRequestId = 25, StatusId = 1, ResponseId = 2, Accepted = true });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.GetAllUnpaidResponseAsync(CancellationToken.None);
                Assert.AreEqual(4, actual.Count());
            }
        }

        [Test]
        public async Task GetAllUnpaidResponseAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidResponse_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidResponse_against_unpaidRequestId")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 1, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 2, UnpaidRequestId = 50, StatusId = 1, ResponseId = 1, Accepted = true });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 3, UnpaidRequestId = 12, StatusId = 1, ResponseId = 1, Accepted = false });
                context.UnpaidResponses.Add(new UnpaidResponse { UnpaidResponseId = 4, UnpaidRequestId = 25, StatusId = 1, ResponseId = 2, Accepted = true });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidResponseDataManager(context);
                var actual = await service.GetAllUnpaidResponseAsync(12, CancellationToken.None);
                var unpaidResponses = actual.ToList();

                Assert.AreEqual(2, unpaidResponses.Count());
                Assert.AreEqual(1, unpaidResponses.ToList()[0].UnpaidResponseId);
                Assert.AreEqual(3, unpaidResponses.ToList()[1].UnpaidResponseId);
            }
        }
    }
}

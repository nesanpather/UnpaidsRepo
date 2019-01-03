using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UnpaidModels;

namespace DataManager.Tests
{
    public class UnpaidRequestDataManagerTests
    {
        [Test]
        public async Task AddUnpaidRequestAsync_GIVEN_Valid_UnpaidRequest_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(new List<UnpaidRequest>
                {
                    new UnpaidRequest
                    {
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                });
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(1, context.UnpaidRequests.Count());
                Assert.AreEqual(10, context.UnpaidRequests.Single().UnpaidId);
                Assert.AreEqual(1, context.UnpaidRequests.Single().StatusId);
                Assert.AreEqual(1, context.UnpaidRequests.Single().NotificationId);
            }
        }

        [Test]
        public async Task AddUnpaidRequestAsync_2Entries_GIVEN_Valid_UnpaidRequest_RETURNS_Valid_Result_With2Entries()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database2")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(new List<UnpaidRequest>
                {
                    new UnpaidRequest
                    {
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    },
                    new UnpaidRequest
                    {
                        UnpaidId = 12,
                        StatusId = 2,
                        NotificationId = 3
                    }
                });
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(2, context.UnpaidRequests.Count());
                Assert.AreEqual(10, context.UnpaidRequests.ToList()[0].UnpaidId);
                Assert.AreEqual(1, context.UnpaidRequests.ToList()[0].StatusId);
                Assert.AreEqual(1, context.UnpaidRequests.ToList()[0].NotificationId);
                Assert.AreEqual(12, context.UnpaidRequests.ToList()[1].UnpaidId);
                Assert.AreEqual(2, context.UnpaidRequests.ToList()[1].StatusId);
                Assert.AreEqual(3, context.UnpaidRequests.ToList()[1].NotificationId);
            }
        }

        [Test]
        public async Task AddUnpaidRequestAsync_GIVEN_Null_UnpaidRequest_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database3")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(null);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsDBContext(options))
            {
                Assert.AreEqual(0, context.UnpaidRequests.Count());
            }
        }

        [Test]
        public async Task GetSingleUnpaidRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidRequest()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidRequest")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetSingleUnpaidRequestAsync(3);
                Assert.AreEqual(3,actual.UnpaidRequestId);
                Assert.AreEqual(10, actual.UnpaidId);
                Assert.AreEqual(1, actual.StatusId);
                Assert.AreEqual(1, actual.NotificationId);
            }
        }

        [Test]
        public async Task GetSingleUnpaidRequestAsync_GIVEN_Invalid_Input_RETURNS_Null()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidRequest2")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetSingleUnpaidRequestAsync(0);
                Assert.AreEqual(null, actual);
            }
        }

        [Test]
        public async Task GetAllUnpaidRequestAsync_RETURNS_Valid_UnpaidRequest_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidRequests")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetAllUnpaidRequestAsync();
                Assert.AreEqual(4, actual.Count());
            }
        }

        [Test]
        public async Task GetAllUnpaidRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidRequest_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsDBContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidRequests_against_unpaidId")
                .Options;

            using (var context = new UnpaidsDBContext(options))
            {
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.UnpaidRequests.Add(new UnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsDBContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetAllUnpaidRequestAsync(10);
                var unpaidRequests = actual.ToList();

                Assert.AreEqual(2, unpaidRequests.Count());
                Assert.AreEqual(1, unpaidRequests.ToList()[0].UnpaidRequestId);
                Assert.AreEqual(3, unpaidRequests.ToList()[1].UnpaidRequestId);
            }
        }
    }
}

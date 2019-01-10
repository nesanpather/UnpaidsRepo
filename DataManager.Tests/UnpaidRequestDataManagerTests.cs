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
    public class UnpaidRequestDataManagerTests
    {
        [Test]
        public async Task AddUnpaidRequestAsync_GIVEN_Valid_UnpaidRequest_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(1, context.TbUnpaidRequest.Count());
                Assert.AreEqual(10, context.TbUnpaidRequest.Single().UnpaidId);
                Assert.AreEqual(1, context.TbUnpaidRequest.Single().StatusId);
                Assert.AreEqual(1, context.TbUnpaidRequest.Single().NotificationId);
            }
        }

        [Test]
        public async Task AddUnpaidRequestAsync_2Entries_GIVEN_Valid_UnpaidRequest_RETURNS_Valid_Result_With2Entries()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database2")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    },
                    new TbUnpaidRequest
                    {
                        UnpaidId = 12,
                        StatusId = 2,
                        NotificationId = 3
                    }
                }, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(2, context.TbUnpaidRequest.Count());
                Assert.AreEqual(10, context.TbUnpaidRequest.ToList()[0].UnpaidId);
                Assert.AreEqual(1, context.TbUnpaidRequest.ToList()[0].StatusId);
                Assert.AreEqual(1, context.TbUnpaidRequest.ToList()[0].NotificationId);
                Assert.AreEqual(12, context.TbUnpaidRequest.ToList()[1].UnpaidId);
                Assert.AreEqual(2, context.TbUnpaidRequest.ToList()[1].StatusId);
                Assert.AreEqual(3, context.TbUnpaidRequest.ToList()[1].NotificationId);
            }
        }

        [Test]
        public async Task AddUnpaidRequestAsync_GIVEN_Null_UnpaidRequest_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database3")
                .Options;

            // Act.
            // Run the test against one instance of the context
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.AddUnpaidRequestAsync(null, CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                Assert.AreEqual(0, context.TbUnpaidRequest.Count());
            }
        }

        [Test]
        public async Task GetSingleUnpaidRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidRequest()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidRequest")
                .Options;

            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetSingleUnpaidRequestAsync(3, CancellationToken.None);
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
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Find_unpaidRequest2")
                .Options;

            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetSingleUnpaidRequestAsync(0, CancellationToken.None);
                Assert.AreEqual(null, actual);
            }
        }

        [Test]
        public async Task GetAllUnpaidRequestAsync_RETURNS_Valid_UnpaidRequest_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidRequests")
                .Options;

            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                var actual = await service.GetAllUnpaidRequestAsync(CancellationToken.None);
                Assert.AreEqual(4, actual.Count());
            }
        }

        [Test]
        public async Task GetAllUnpaidRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_UnpaidRequest_List()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Get_unpaidRequests_against_unpaidId")
                .Options;

            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1 });
                context.TbUnpaidRequest.Add(new TbUnpaidRequest { UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2 });
                context.SaveChanges();
            }

            // Act and Assert.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                //var unpaids = new List<UnpaidDb>
                //{
                //    new UnpaidDb
                //    {
                //        UnpaidId = 10, PolicyNumber = "P1", IdNumber = "9009165023080", Name = "Tom",
                //        Message = "Test Message 1.", IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                //    },
                //    new UnpaidDb
                //    {
                //        UnpaidId = 12, PolicyNumber = "P2", Name = "Bob", IdNumber = "9009165023081",
                //        Message = "Test Message 2.", IdempotencyKey = "0f8fad5b-d9cb-469f-a165-70867728950e"
                //    },
                //    new UnpaidDb
                //    {
                //        UnpaidId = 55, PolicyNumber = "P4", IdNumber = "9009165023082", Name = "Brad",
                //        Message = "Test Message 4.", IdempotencyKey = "1f9fad5b-d9cb-469f-a165-70867728950e"
                //    }
                //};

                var actual = await service.GetAllUnpaidRequestAsync(10, CancellationToken.None);
                var unpaidRequests = actual.ToList();

                Assert.AreEqual(2, unpaidRequests.Count());
                Assert.AreEqual(1, unpaidRequests.ToList()[0].UnpaidRequestId);
                Assert.AreEqual(3, unpaidRequests.ToList()[1].UnpaidRequestId);
            }
        }

        [Test]
        public async Task UpdateUnpaidRequestAsync_GIVEN_Valid_Input_RETURNS_Valid_Result()
        {
            // Arrange.
            var options = new DbContextOptionsBuilder<UnpaidsContext>()
                .UseInMemoryDatabase(databaseName: "Update_unpaidRequests_against_unpaidId")
                .Options;

            using (var context = new UnpaidsContext(options))
            {
                context.TbUnpaidRequest.Add(new TbUnpaidRequest
                    {UnpaidRequestId = 1, UnpaidId = 10, StatusId = 1, NotificationId = 1});
                context.TbUnpaidRequest.Add(new TbUnpaidRequest
                    {UnpaidRequestId = 2, UnpaidId = 12, StatusId = 2, NotificationId = 3});
                context.TbUnpaidRequest.Add(new TbUnpaidRequest
                    {UnpaidRequestId = 3, UnpaidId = 10, StatusId = 1, NotificationId = 1});
                context.TbUnpaidRequest.Add(new TbUnpaidRequest
                    {UnpaidRequestId = 4, UnpaidId = 55, StatusId = 3, NotificationId = 2});
                context.SaveChanges();
            }

            // Act.
            using (var context = new UnpaidsContext(options))
            {
                var service = new UnpaidRequestDataManager(context);
                await service.UpdateUnpaidRequestAsync(3, Notification.Call, Status.Failed, "Testing.", CancellationToken.None);
            }

            // Assert.
            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new UnpaidsContext(options))
            {
                var actual = context.TbUnpaidRequest.FirstOrDefault(item => item.UnpaidRequestId == 3);

                if (actual != null)
                {
                    Assert.AreEqual(3, actual.UnpaidRequestId);
                    Assert.AreEqual(3, actual.StatusId);
                    Assert.AreEqual(4, actual.NotificationId);
                    Assert.AreEqual("Testing.", actual.StatusAdditionalInfo);
                }
            }
        }
    }
}

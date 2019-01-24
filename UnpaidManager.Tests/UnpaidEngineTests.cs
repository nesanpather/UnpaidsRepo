using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataManager.Models;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using NSubstitute;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager.Tests
{
    public class UnpaidEngineTests
    {
        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_RETURNS_True()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<TbUnpaid>
            {
                new TbUnpaid
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();

            unpaidBatchClient.UpdateUnpaidBatchAsync("GUID", Status.Success, Arg.Any<DateTime>(), CancellationToken.None).Returns(Task.FromResult(1));

            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, "GUID", CancellationToken.None);

            // Assert.
            Assert.IsTrue(actual);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_RETURNS_False()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    AdditionalErrorMessage = "Error getting a WebToken."
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Failed, "Error getting a WebToken.", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<TbUnpaid>
            {
                new TbUnpaid
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, "GUID", CancellationToken.None);

            // Assert.
            Assert.False(actual);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_BatchUnpaid_RETURNS_True()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023081", "GUID_2", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    AdditionalErrorMessage = "Error."
                }));

            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023082", "GUID_3", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 11, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 2,
                        UnpaidId = 11,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 12, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 3,
                        UnpaidId = 12,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            unpaidRequestClient.UpdateUnpaidRequestAsync(2, Notification.Push, Status.Failed, "Error.", Arg.Any<DateTime>(), "GUID_2", CancellationToken.None)
                .Returns(Task.FromResult(1));

            unpaidRequestClient.UpdateUnpaidRequestAsync(3, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_3", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<TbUnpaid>
            {
                new TbUnpaid
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test1"
                },
                new TbUnpaid
                {
                    UnpaidId = 11,
                    PolicyNumber = "P2",
                    IdNumber = "9009165023081",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test2"
                },
                new TbUnpaid
                {
                    UnpaidId = 12,
                    PolicyNumber = "P3",
                    IdNumber = "9009165023082",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test3"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, "GUID", CancellationToken.None);

            // Assert.
            Assert.IsTrue(true);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_EmptyUnpaidRequest_RETURNS_False()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>()));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<TbUnpaid>
            {
                new TbUnpaid
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test1"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, "GUID", CancellationToken.None);

            // Assert.
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_NullGetAllUnpaidRequestAsync_RETURNS_False()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(null));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<TbUnpaid>
            {
                new TbUnpaid
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    Title = "Test1"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, "GUID", CancellationToken.None);

            // Assert.
            Assert.IsFalse(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_RETURNS_Valid_UnpaidOutputList_StatusPending()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<UnpaidInput>>(), 1,
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("GUID", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaid>>(new List<TbUnpaid>
                {
                    new TbUnpaid
                    {
                        UnpaidId = 10,
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name",
                        Title = "Test1"
                    }
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.AddUnpaidRequestAsync(Arg.Any<IEnumerable<TbUnpaid>>(), Notification.Push,
                Status.Pending, CancellationToken.None).Returns(Task.FromResult(1));

            unpaidRequestClient.GetAllUnpaidRequestAsync((int) 10, (Status) Status.Pending, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaidRequest>>(new List<TbUnpaidRequest>
                {
                    new TbUnpaidRequest
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", Arg.Any<DateTime>(), "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", "GUID_1", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidsInput = new List<UnpaidInput>
            {
                new UnpaidInput
                {
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name"
                }
            };

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();

            unpaidBatchClient.AddUnpaidBatchAsync("GUID", Status.Pending, "XX", CancellationToken.None).Returns(Task.FromResult(1));

            unpaidBatchClient.GetUnpaidBatchByBatchKeyAsync("GUID", CancellationToken.None).Returns(Task.FromResult<IEnumerable<TbUnpaidBatch>>(new List<TbUnpaidBatch>
            {
                new TbUnpaidBatch{UnpaidBatchId = 1, BatchKey = "GUID", StatusId = 1, UserName = "XX"}
            }));

            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "GUID", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            Assert.AreEqual("Pending", actual.Status);
            Assert.AreEqual("", actual.ErrorMessage);

        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_InvalidAddUnpaidAsync_RETURNS_Null(int input)
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<UnpaidInput>>(), 1,
                    CancellationToken.None).Returns(Task.FromResult(input));            

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();            
            var notification = Substitute.For<INotification>();
            
            var unpaidsInput = new List<UnpaidInput>
                {
                    new UnpaidInput
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_EmptyGetUnpaidsByIdempotencyKeyAsync_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<UnpaidInput>>(), 1,
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaid>>(new List<TbUnpaid>()));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<UnpaidInput>
                {
                    new UnpaidInput
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_NullGetUnpaidsByIdempotencyKeyAsync_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<UnpaidInput>>(), 1,
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaid>>(null));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<UnpaidInput>
            {
                new UnpaidInput
                {
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name"
                }
            };

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_InvalidAddUnpaidRequestAsync_RETURNS_Null(int input)
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<UnpaidInput>>(), 1,
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<TbUnpaid>>(new List<TbUnpaid>
                {
                    new TbUnpaid
                    {
                        UnpaidId = 10,
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name",
                        Title = "Test1"
                    }
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.AddUnpaidRequestAsync(Arg.Any<IEnumerable<TbUnpaid>>(), Notification.Push,
                Status.Pending, CancellationToken.None).Returns(Task.FromResult(input));

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<UnpaidInput>
                {
                    new UnpaidInput
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task HandleUnpaidAsync_GIVEN_InvalidIdempotencyKey_RETURNS_Null(string input)
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<UnpaidInput>
            {
                new UnpaidInput
                {
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name"
                }
            };

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, input, "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Invalid_Input_NullUnpaid_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(null, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Invalid_Input_EmptyUnpaid_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<UnpaidInput>();

            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", "XX", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Valid_Input_SingleUnpaidResponseInput_RETURNS_Valid_UnpaidResponseOutput()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();

            unpaidRequestClient.GetUnpaidRequestByIdAsync(Arg.Any<UnpaidResponseInput>(), CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 10,
                UnpaidId = 2,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 10, CancellationToken.None).Returns(Task.FromResult(1));

            var unpaidResponseInputList = new List<UnpaidResponseInput>
            {
                new UnpaidResponseInput
                {
                    PolicyNumber = "P1Test",
                    IdNumber = "9009165023080",
                    Accepted = true,
                    ContactOption = ContactOption.CallMe
                }
            };

            
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();

            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(unpaidResponseInputList, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidResponseOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidResponseOutputs.Count);
            Assert.AreEqual("P1Test", unpaidResponseOutputs[0].PolicyNumber);
            Assert.AreEqual("9009165023080", unpaidResponseOutputs[0].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[0].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[0].ContactOption);
            Assert.AreEqual(HttpStatusCode.Accepted, unpaidResponseOutputs[0].HttpStatusCode);
            Assert.AreEqual(string.Empty, unpaidResponseOutputs[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Valid_Input_SingleUnpaidResponseInput_Duplicate_RETURNS_Valid_UnpaidResponseOutput()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();

            unpaidRequestClient.GetUnpaidRequestByIdAsync(Arg.Any<UnpaidResponseInput>(), CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 10,
                UnpaidId = 2,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidResponseClient.GetUnpaidResponseAsync(10, CancellationToken.None).Returns(Task.FromResult<IEnumerable<TbUnpaidResponse>>(new List<TbUnpaidResponse>
            {
                new TbUnpaidResponse
                {
                    UnpaidResponseId = 1,
                    UnpaidRequestId = 10,
                    ResponseId = 1,
                    Accepted = true,
                    StatusId = 1
                }
            }));

            var unpaidResponseInputList = new List<UnpaidResponseInput>
            {
                new UnpaidResponseInput
                {
                    PolicyNumber = "P1Test",
                    IdNumber = "9009165023080",
                    Accepted = true,
                    ContactOption = ContactOption.CallMe
                }
            };
            
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(unpaidResponseInputList, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidResponseOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidResponseOutputs.Count);
            Assert.AreEqual("P1Test", unpaidResponseOutputs[0].PolicyNumber);
            Assert.AreEqual("9009165023080", unpaidResponseOutputs[0].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[0].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[0].ContactOption);
            Assert.AreEqual(HttpStatusCode.AlreadyReported, unpaidResponseOutputs[0].HttpStatusCode);
            Assert.AreEqual("Notification response already exists.", unpaidResponseOutputs[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Invalid_Input_EmptyUnpaidResponseInput_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();

            var unpaidResponseInputList = new List<UnpaidResponseInput>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();

            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(unpaidResponseInputList, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Invalid_Input_NullUnpaidResponseInput_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();
            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();

            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(null, CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Valid_Input_BatchUnpaidResponseInput_ErrorAddPendingUnpaidResponseAsync_RETURNS_Valid_UnpaidResponseOutput()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();

            var input1 = new UnpaidResponseInput
            {
                PolicyNumber = "P1Test",
                IdNumber = "9009165023080",
                Accepted = true,
                ContactOption = ContactOption.CallMe
            };

            var input2 = new UnpaidResponseInput
            {
                PolicyNumber = "P2Test",
                IdNumber = "9009165023081",
                Accepted = false,
                ContactOption = ContactOption.EmailMe
            };

            var input3 = new UnpaidResponseInput
            {
                PolicyNumber = "P3Test",
                IdNumber = "9009165023082",
                Accepted = true,
                ContactOption = ContactOption.CallMe
            };

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input1, CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 10,
                UnpaidId = 2,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input2, CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 11,
                UnpaidId = 3,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input3, CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 12,
                UnpaidId = 5,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 10, CancellationToken.None).Returns(Task.FromResult(1));
            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 11, CancellationToken.None).Returns(Task.FromResult(1));
            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 12, CancellationToken.None).Returns(Task.FromResult(0));

            var unpaidResponseInputList = new List<UnpaidResponseInput>
            {
                input1,
                input2,
                input3
            };

            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();

            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(unpaidResponseInputList, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidResponseOutputs = actual.ToList();

            Assert.AreEqual(3, unpaidResponseOutputs.Count);
            Assert.AreEqual("P1Test", unpaidResponseOutputs[0].PolicyNumber);
            Assert.AreEqual("9009165023080", unpaidResponseOutputs[0].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[0].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[0].ContactOption);
            Assert.AreEqual(HttpStatusCode.Accepted, unpaidResponseOutputs[0].HttpStatusCode);
            Assert.AreEqual(string.Empty, unpaidResponseOutputs[0].ErrorMessage);

            Assert.AreEqual("P2Test", unpaidResponseOutputs[1].PolicyNumber);
            Assert.AreEqual("9009165023081", unpaidResponseOutputs[1].IdNumber);
            Assert.AreEqual(false, unpaidResponseOutputs[1].Accepted);
            Assert.AreEqual(ContactOption.EmailMe, unpaidResponseOutputs[1].ContactOption);
            Assert.AreEqual(HttpStatusCode.Accepted, unpaidResponseOutputs[1].HttpStatusCode);
            Assert.AreEqual(string.Empty, unpaidResponseOutputs[1].ErrorMessage);

            Assert.AreEqual("P3Test", unpaidResponseOutputs[2].PolicyNumber);
            Assert.AreEqual("9009165023082", unpaidResponseOutputs[2].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[2].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[2].ContactOption);
            Assert.AreEqual(HttpStatusCode.InternalServerError, unpaidResponseOutputs[2].HttpStatusCode);
            Assert.AreEqual("Error adding notification response.", unpaidResponseOutputs[2].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidResponseAsync_GIVEN_Valid_Input_BatchUnpaidResponseInput_ErrorGetLatestSuccessfulUnpaidRequestAsync_RETURNS_Valid_UnpaidResponseOutput()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();
            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            var notification = Substitute.For<INotification>();
            var unpaidResponseClient = Substitute.For<IUnpaidResponseClient>();

            var input1 = new UnpaidResponseInput
            {
                PolicyNumber = "P1Test",
                IdNumber = "9009165023080",
                Accepted = true,
                ContactOption = ContactOption.CallMe
            };

            var input2 = new UnpaidResponseInput
            {
                PolicyNumber = "P2Test",
                IdNumber = "9009165023081",
                Accepted = false,
                ContactOption = ContactOption.EmailMe
            };

            var input3 = new UnpaidResponseInput
            {
                PolicyNumber = "P3Test",
                IdNumber = "9009165023082",
                Accepted = true,
                ContactOption = ContactOption.CallMe
            };

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input1, CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 10,
                UnpaidId = 2,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input2, CancellationToken.None).Returns(Task.FromResult<TbUnpaidRequest>(null));

            unpaidRequestClient.GetUnpaidRequestByIdAsync(input3, CancellationToken.None).Returns(Task.FromResult(new TbUnpaidRequest
            {
                UnpaidRequestId = 12,
                UnpaidId = 5,
                NotificationId = 1,
                StatusId = 2,
                StatusAdditionalInfo = null
            }));

            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 10, CancellationToken.None).Returns(Task.FromResult(1));
            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 11, CancellationToken.None).Returns(Task.FromResult(1));
            unpaidResponseClient.AddPendingUnpaidResponseAsync(Arg.Any<UnpaidResponseInput>(), 12, CancellationToken.None).Returns(Task.FromResult(1));

            var unpaidResponseInputList = new List<UnpaidResponseInput>
            {
                input1,
                input2,
                input3
            };

            var unpaidNotificationClient = Substitute.For<IUnpaidNotificationApiClient>();
            var unpaidBatchClient = Substitute.For<IUnpaidBatchClient>();
            var logger = Substitute.For<ILogger<UnpaidEngine>>();
            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification, unpaidResponseClient, unpaidNotificationClient, unpaidBatchClient, logger);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidResponseAsync(unpaidResponseInputList, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidResponseOutputs = actual.ToList();

            Assert.AreEqual(3, unpaidResponseOutputs.Count);
            Assert.AreEqual("P1Test", unpaidResponseOutputs[0].PolicyNumber);
            Assert.AreEqual("9009165023080", unpaidResponseOutputs[0].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[0].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[0].ContactOption);
            Assert.AreEqual(HttpStatusCode.Accepted, unpaidResponseOutputs[0].HttpStatusCode);
            Assert.AreEqual(string.Empty, unpaidResponseOutputs[0].ErrorMessage);

            Assert.AreEqual("P2Test", unpaidResponseOutputs[1].PolicyNumber);
            Assert.AreEqual("9009165023081", unpaidResponseOutputs[1].IdNumber);
            Assert.AreEqual(false, unpaidResponseOutputs[1].Accepted);
            Assert.AreEqual(ContactOption.EmailMe, unpaidResponseOutputs[1].ContactOption);
            Assert.AreEqual(HttpStatusCode.NotFound, unpaidResponseOutputs[1].HttpStatusCode);
            Assert.AreEqual("Notification not found.", unpaidResponseOutputs[1].ErrorMessage);

            Assert.AreEqual("P3Test", unpaidResponseOutputs[2].PolicyNumber);
            Assert.AreEqual("9009165023082", unpaidResponseOutputs[2].IdNumber);
            Assert.AreEqual(true, unpaidResponseOutputs[2].Accepted);
            Assert.AreEqual(ContactOption.CallMe, unpaidResponseOutputs[2].ContactOption);
            Assert.AreEqual(HttpStatusCode.Accepted, unpaidResponseOutputs[2].HttpStatusCode);
            Assert.AreEqual(string.Empty, unpaidResponseOutputs[2].ErrorMessage);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NSubstitute;
using UnpaidManager.Interfaces;
using UnpaidModels;

namespace UnpaidManager.Tests
{
    public class UnpaidEngineTests
    {
        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_RETURNS_Valid_UnpaidOutputList_StatusSuccess()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                {
                    new UnpaidRequestDb
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<UnpaidDb>
            {
                new UnpaidDb
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidOutputs.Count());
            Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
            Assert.AreEqual("Success", unpaidOutputs.ToList()[0].Status);
            Assert.AreEqual("", unpaidOutputs.ToList()[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_RETURNS_Valid_UnpaidOutputList_StatusFailed()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    AdditionalErrorMessage = "Error getting a WebToken."
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                {
                    new UnpaidRequestDb
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Failed, "Error getting a WebToken.", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<UnpaidDb>
            {
                new UnpaidDb
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidOutputs.Count());
            Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
            Assert.AreEqual("Failed", unpaidOutputs.ToList()[0].Status);
            Assert.AreEqual("Error getting a WebToken.", unpaidOutputs.ToList()[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_BatchUnpaid_RETURNS_Valid_UnpaidOutputList()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023081", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    AdditionalErrorMessage = "Error."
                }));

            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023082", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                {
                    new UnpaidRequestDb
                    {
                        UnpaidRequestId = 1,
                        UnpaidId = 10,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.GetAllUnpaidRequestAsync(11, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                {
                    new UnpaidRequestDb
                    {
                        UnpaidRequestId = 2,
                        UnpaidId = 11,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.GetAllUnpaidRequestAsync(12, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                {
                    new UnpaidRequestDb
                    {
                        UnpaidRequestId = 3,
                        UnpaidId = 12,
                        StatusId = 1,
                        NotificationId = 1
                    }
                }));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", CancellationToken.None)
                .Returns(Task.FromResult(1));

            unpaidRequestClient.UpdateUnpaidRequestAsync(2, Notification.Push, Status.Failed, "Error.", CancellationToken.None)
                .Returns(Task.FromResult(1));

            unpaidRequestClient.UpdateUnpaidRequestAsync(3, Notification.Push, Status.Success, "", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<UnpaidDb>
            {
                new UnpaidDb
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                },
                new UnpaidDb
                {
                    UnpaidId = 11,
                    PolicyNumber = "P2",
                    IdNumber = "9009165023081",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                },
                new UnpaidDb
                {
                    UnpaidId = 12,
                    PolicyNumber = "P3",
                    IdNumber = "9009165023082",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidOutputs = actual.ToList();

            Assert.AreEqual(3, unpaidOutputs.Count());
            Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
            Assert.AreEqual("P1", unpaidOutputs.ToList()[0].PolicyNumber);
            Assert.AreEqual("Success", unpaidOutputs.ToList()[0].Status);
            Assert.AreEqual("", unpaidOutputs.ToList()[0].ErrorMessage);

            Assert.AreEqual("9009165023081", unpaidOutputs.ToList()[1].IdNumber);
            Assert.AreEqual("P2", unpaidOutputs.ToList()[1].PolicyNumber);
            Assert.AreEqual("Failed", unpaidOutputs.ToList()[1].Status);
            Assert.AreEqual("Error.", unpaidOutputs.ToList()[1].ErrorMessage);

            Assert.AreEqual("9009165023082", unpaidOutputs.ToList()[2].IdNumber);
            Assert.AreEqual("P3", unpaidOutputs.ToList()[2].PolicyNumber);
            Assert.AreEqual("Success", unpaidOutputs.ToList()[2].Status);
            Assert.AreEqual("", unpaidOutputs.ToList()[2].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_EmptyUnpaidRequest_RETURNS_Valid_UnpaidOutputList_StatusPending()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>()));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<UnpaidDb>
            {
                new UnpaidDb
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidOutputs.Count());
            Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
            Assert.AreEqual("Pending", unpaidOutputs.ToList()[0].Status);
            Assert.AreEqual("", unpaidOutputs.ToList()[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidRequestAsync_GIVEN_Valid_SingleUnpaid_NullGetAllUnpaidRequestAsync_RETURNS_Valid_UnpaidOutputList_StatusPending()
        {
            // Arrange.
            var notification = Substitute.For<INotification>();
            notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                .Returns(Task.FromResult(new NotificationResponse
                {
                    StatusCode = HttpStatusCode.Accepted,
                    AdditionalErrorMessage = ""
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(null));

            unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", CancellationToken.None)
                .Returns(Task.FromResult(1));

            var unpaidsInput = new List<UnpaidDb>
            {
                new UnpaidDb
                {
                    UnpaidId = 10,
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name",
                    IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                }
            };

            var unpaidClient = Substitute.For<IUnpaidClient>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidRequestAsync(unpaidsInput, CancellationToken.None);

            // Assert.
            Assert.IsNotNull(actual);
            var unpaidOutputs = actual.ToList();

            Assert.AreEqual(1, unpaidOutputs.Count());
            Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
            Assert.AreEqual("Pending", unpaidOutputs.ToList()[0].Status);
            Assert.AreEqual("", unpaidOutputs.ToList()[0].ErrorMessage);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_RETURNS_Valid_UnpaidOutputList_StatusSuccess()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<Unpaid>>(), "7c9e6679-7425-40de-944b-e07fc1f90ae7",
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidDb>>(new List<UnpaidDb>
                {
                    new UnpaidDb
                    {
                        UnpaidId = 10,
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name",
                        IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                    }
                }));

                var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
                unpaidRequestClient.AddUnpaidRequestAsync(Arg.Any<IEnumerable<UnpaidDb>>(), Notification.Push,
                    Status.Pending, CancellationToken.None).Returns(Task.FromResult(1));

                unpaidRequestClient.GetAllUnpaidRequestAsync(10, CancellationToken.None)
                    .Returns(Task.FromResult<IEnumerable<UnpaidRequestDb>>(new List<UnpaidRequestDb>
                    {
                        new UnpaidRequestDb
                        {
                            UnpaidRequestId = 1,
                            UnpaidId = 10,
                            StatusId = 1,
                            NotificationId = 1
                        }
                    }));

                unpaidRequestClient.UpdateUnpaidRequestAsync(1, Notification.Push, Status.Success, "", CancellationToken.None)
                    .Returns(Task.FromResult(1));

                var notification = Substitute.For<INotification>();
                notification.SendAsync(Arg.Any<string>(), Arg.Any<string>(), "9009165023080", CancellationToken.None)
                    .Returns(Task.FromResult(new NotificationResponse
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        AdditionalErrorMessage = ""
                    }));

                var unpaidsInput = new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"                        
                    }
                };

                var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

                // Act.
                var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

                // Assert.
                Assert.IsNotNull(actual);
                var unpaidOutputs = actual.ToList();

                Assert.AreEqual(1, unpaidOutputs.Count());
                Assert.AreEqual("9009165023080", unpaidOutputs.ToList()[0].IdNumber);
                Assert.AreEqual("Success", unpaidOutputs.ToList()[0].Status);
                Assert.AreEqual("", unpaidOutputs.ToList()[0].ErrorMessage);

        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_InvalidAddUnpaidAsync_RETURNS_Null(int input)
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<Unpaid>>(), "7c9e6679-7425-40de-944b-e07fc1f90ae7",
                    CancellationToken.None).Returns(Task.FromResult(input));            

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();            
            var notification = Substitute.For<INotification>();
            
            var unpaidsInput = new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_EmptyGetUnpaidsByIdempotencyKeyAsync_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<Unpaid>>(), "7c9e6679-7425-40de-944b-e07fc1f90ae7",
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidDb>>(new List<UnpaidDb>()));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }

        [Test]
        public async Task HandleUnpaidAsync_GIVEN_Valid_Input_SingleUnpaid_NullGetUnpaidsByIdempotencyKeyAsync_RETURNS_Null()
        {
            // Arrange.
            var unpaidClient = Substitute.For<IUnpaidClient>();

            unpaidClient
                .AddUnpaidAsync(Arg.Any<IEnumerable<Unpaid>>(), "7c9e6679-7425-40de-944b-e07fc1f90ae7",
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidDb>>(null));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<Unpaid>
            {
                new Unpaid
                {
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name"
                }
            };

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

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
                .AddUnpaidAsync(Arg.Any<IEnumerable<Unpaid>>(), "7c9e6679-7425-40de-944b-e07fc1f90ae7",
                    CancellationToken.None).Returns(Task.FromResult(1));

            unpaidClient.GetUnpaidsByIdempotencyKeyAsync("7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None)
                .Returns(Task.FromResult<IEnumerable<UnpaidDb>>(new List<UnpaidDb>
                {
                    new UnpaidDb
                    {
                        UnpaidId = 10,
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name",
                        IdempotencyKey = "7c9e6679-7425-40de-944b-e07fc1f90ae7"
                    }
                }));

            var unpaidRequestClient = Substitute.For<IUnpaidRequestClient>();
            unpaidRequestClient.AddUnpaidRequestAsync(Arg.Any<IEnumerable<UnpaidDb>>(), Notification.Push,
                Status.Pending, CancellationToken.None).Returns(Task.FromResult(input));

            var notification = Substitute.For<INotification>();

            var unpaidsInput = new List<Unpaid>
                {
                    new Unpaid
                    {
                        PolicyNumber = "P1",
                        IdNumber = "9009165023080",
                        Message = "Test Message",
                        Name = "Test Name"
                    }
                };

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

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

            var unpaidsInput = new List<Unpaid>
            {
                new Unpaid
                {
                    PolicyNumber = "P1",
                    IdNumber = "9009165023080",
                    Message = "Test Message",
                    Name = "Test Name"
                }
            };

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, input, CancellationToken.None);

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

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(null, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

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

            var unpaidsInput = new List<Unpaid>();

            var unpaidEngine = new UnpaidEngine(unpaidClient, unpaidRequestClient, notification);

            // Act.
            var actual = await unpaidEngine.HandleUnpaidAsync(unpaidsInput, "7c9e6679-7425-40de-944b-e07fc1f90ae7", CancellationToken.None);

            // Assert.
            Assert.IsNull(actual);
        }
    }
}

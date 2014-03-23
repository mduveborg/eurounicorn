using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EurounicornAPI.CouchDB;
using EurounicornAPI.Voting.Entities;
using EurounicornAPI.Voting;

namespace EurounicornAPI.Tests.Voting
{
	[TestClass]
	public class VotingServiceTest
	{
		[TestMethod]
		public void UserCanVote_UserWithNoVotes_ReturnsTrue()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new List<Vote>());

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var canVote = votingService.UserCanVote("unicorn@netlight.com");

			// Assert
			Assert.IsTrue(canVote);
		}

		[TestMethod]
		public void UserCanVote_UserWithAllVotes_ReturnsFalse()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new[]
			{
				new Vote { Username = "unicorn@netlight.com" },
				new Vote { Username = "unicorn@netlight.com" },
				new Vote { Username = "unicorn@netlight.com" }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var canVote = votingService.UserCanVote("unicorn@netlight.com");

			// Assert
			Assert.IsFalse(canVote);
		}

		[TestMethod]
		public void UserCanVote_UserWithNoPreviousVotesOnTrack_ReturnsTrue()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new[]
			{
				new Vote { Username = "unicorn@netlight.com", TrackId = 100 },
				new Vote { Username = "unicorn@netlight.com", TrackId = 200 }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var canVote = votingService.UserCanVote("unicorn@netlight.com", 300);

			// Assert
			Assert.IsTrue(canVote);
		}

		[TestMethod]
		public void UserCanVote_UserWithPreviousVotesOnTrack_ReturnsFalse()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new[]
			{
				new Vote { Username = "unicorn@netlight.com", TrackId = 100 },
				new Vote { Username = "unicorn@netlight.com", TrackId = 200 }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var canVote = votingService.UserCanVote("unicorn@netlight.com", 200);

			// Assert
			Assert.IsFalse(canVote);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CastVote_WithNonExistentUser_ThrowsException()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<User>(It.IsAny<string>())).Returns(new List<User>());

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			votingService.CastVote("unicorn@netlight.com", 1, 2);
		}

		[TestMethod]
		public void CastVote_WithUserThatCannotVote_IgnoresVote()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<User>(It.IsAny<string>())).Returns(new [] 
			{
				new User { Username = "unicorn@netlight.com" }
			});
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new[]
			{
				new Vote { Username = "unicorn@netlight.com" },
				new Vote { Username = "unicorn@netlight.com" },
				new Vote { Username = "unicorn@netlight.com" }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			votingService.CastVote("unicorn@netlight.com", 1, 2);

			// Assert
			couchDbMock.Verify(c => c.Set<Vote>(It.IsAny<Vote>()), Times.Never);
		}

		[TestMethod]
		public void CastVote_WithValidUser_SetsVote()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByUsername<User>(It.IsAny<string>())).Returns(new[] 
			{
				new User { Username = "unicorn@netlight.com" }
			});
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.IsAny<string>())).Returns(new List<Vote>());

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			votingService.CastVote("unicorn@netlight.com", 1, 2);

			// Assert
			couchDbMock.Verify(c => c.Set<Vote>(It.IsAny<Vote>()), Times.Once);
		}

		[TestMethod]
		public void GetVoterTurnout_WithNoMatchingUsers_ReturnsEmptyResult()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByLevel<User>(It.IsAny<Level>())).Returns(new List<User>());

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var turnouts = votingService.GetVoterTurnout();

			// Assert
			Assert.AreEqual(0, turnouts.Keys.Count);
		}

		[TestMethod]
		public void GetVoterTurnout_WithNoVotes_ReturnsZeroTurnout()
		{
			// Arrange
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByLevel<User>(It.Is<Level>(l => l == Level.A))).Returns(new[]
			{
				new User { Username = "unicorn1@netlight.com", Level = Level.A },
				new User { Username = "unicorn2@netlight.com", Level = Level.A },
				new User { Username = "unicorn3@netlight.com", Level = Level.A }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var turnouts = votingService.GetVoterTurnout();

			// Assert
			Assert.AreEqual(0, turnouts[Level.A]);
		}

		[TestMethod]
		public void GetVoterTurnout_WithMatchingUsersAndVotes_CalculatesCorrectTurnout()
		{
			// Arrange - create 5 distinct users, both A and AC, out of which one user from each level has
			// voted (which should give a turnout of 33.3% for A and 50% for AC).
			var couchDbMock = new Mock<ICouchDBService>();
			couchDbMock.Setup(c => c.FindByLevel<User>(It.Is<Level>(l => l == Level.A))).Returns(new[]
			{
				new User { Username = "unicorn1@netlight.com", Level = Level.A },
				new User { Username = "unicorn2@netlight.com", Level = Level.A },
				new User { Username = "unicorn3@netlight.com", Level = Level.A }
			});
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.Is<string>(n => n == "unicorn1@netlight.com"))).Returns(new[]
			{
				new Vote { Username = "unicorn1@netlight.com" },
				new Vote { Username = "unicorn1@netlight.com" },
				new Vote { Username = "unicorn1@netlight.com" }
			});

			couchDbMock.Setup(c => c.FindByLevel<User>(It.Is<Level>(l => l == Level.AC))).Returns(new[]
			{
				new User { Username = "unicorn4@netlight.com", Level = Level.AC },
				new User { Username = "unicorn5@netlight.com", Level = Level.AC }
			});
			couchDbMock.Setup(c => c.FindByUsername<Vote>(It.Is<string>(n => n == "unicorn5@netlight.com"))).Returns(new[]
			{
				new Vote { Username = "unicorn5@netlight.com" },
				new Vote { Username = "unicorn5@netlight.com" },
				new Vote { Username = "unicorn5@netlight.com" }
			});

			// Act
			var votingService = new VotingService(couchDbMock.Object);
			var turnouts = votingService.GetVoterTurnout();

			// Assert
			Assert.AreEqual(33.3, turnouts[Level.A]);
			Assert.AreEqual(50.0, turnouts[Level.AC]);
		}
	}
}

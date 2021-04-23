using IssuePilot.Models.Repositorys;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace IssuePilot.Test
{
    public class DashboardRepositoryTests : InitDbWithData
    {
        /*
         * CreateNewsfeedEntryAsync test cases
         * Did the method add DateTime.Now?
         * Exists a additional Entry in db aside from seed data?
         * Is the seen boolean initial false?
         * 
         * text is empty / null exception
         * user is null exception
         */
        [Fact]
        public async Task CreateNewsfeedEntryAsyncTest_HasEntryDataBeenCreatedByTheSystem()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            string userId = ListOfUsersWithId().First().Id;

            // Act
            await dashboardRepository.CreateNewsfeedEntryAsync(userId, "Test Message");
            var allNewsfeedEntry = await context.NewsfeedEntries.ToListAsync();
            var entry = allNewsfeedEntry[^1];

            // Assert
            Assert.NotNull(allNewsfeedEntry);
            Assert.Equal("Test Message", entry.NewsText);

            // Did the method add DateTime.Now?
            Assert.NotEqual(entry.CreateDate, DateTime.MinValue);

            // Exists a additional Entry in db aside from seed data?
            Assert.Equal(3, allNewsfeedEntry.Count);

            // Is the seen boolean initial false?
            Assert.False(entry.Seen);
        }

        [Fact]
        public async Task CreateNewsfeedEntryAsyncTest_ExceptionTextIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            string userId = ListOfUsersWithId().First().Id;

            // Act & Assert
            // If text is null.
            await Assert.ThrowsAsync<ArgumentException>(async () => await dashboardRepository.CreateNewsfeedEntryAsync(userId, null));
        }

        [Fact]
        public async Task CreateNewsfeedEntryAsyncTest_ExceptionEmptyString()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            string userId = ListOfUsersWithId().First().Id;

            // Act & Assert
            // If text is empty ("").
            await Assert.ThrowsAsync<ArgumentException>(async () => await dashboardRepository.CreateNewsfeedEntryAsync(userId, ""));
        }

        [Fact]
        public async Task CreateNewsfeedEntryAsyncTest_ExceptionUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act & Assert
            // If user is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await dashboardRepository.CreateNewsfeedEntryAsync(null, "TestString"));
        }


        /*
         * GetNewsfeedByUserIdAsync test cases
         * Have all entries been retrieved?
         * Has the data been correctly transferred from the database to the model?
         * Has list no entries?
         * 
         * userId is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task GetNewsfeedByUserIdAsyncTest_TwoEntry()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act
            var entryListByUserId = await dashboardRepository.GetNewsfeedByUserIdAsync("2301D884-221A-4E7D-B509-0113DCC043E1"); // ManagerId 

            // Assert
            // Have all entries been retrieved?
            Assert.Equal(2, entryListByUserId.Count);

            // Has the data been correctly transferred from the database to the ViewModel ?
            var entryExpected = await context.NewsfeedEntries.FindAsync(entryListByUserId[0].Id);
            Assert.Equal(entryListByUserId[0].CreateDate, entryExpected.CreateDate);
            Assert.Equal(entryListByUserId[0].NewsText, entryExpected.NewsText);
            Assert.Equal(entryListByUserId[0].Seen, entryExpected.Seen);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNewsfeedByUserIdAsyncTest_ZeroEntry()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act
            var entryListByUserId = await dashboardRepository.GetNewsfeedByUserIdAsync("2301D884-221A-4E7D-B509-0113DCC043E0"); // AdminId 

            // Assert
            // Has list no entries?
            Assert.Empty(entryListByUserId);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetNewsfeedByUserIdAsyncTest_Exception()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act & Assert
            // If user is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await dashboardRepository.GetNewsfeedByUserIdAsync(null));
        }

        /*
        * UpdateSeenStatusAsync test cases
        * Has seen boolean been updated?
        */
        [Fact]
        public async System.Threading.Tasks.Task UpdateSeenStatusAsyncTest()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            var listOfEntries = await context.NewsfeedEntries.ToListAsync();
            DashboardRepository dashboardRepository = new DashboardRepository(context);
            listOfEntries[0].Seen = false;

            // Act
            await dashboardRepository.UpdateSeenStatusAsync(listOfEntries[0].Id);

            // Assert
            // Has seen boolean been updated?
            Assert.True(listOfEntries[0].Seen);
        }

        /*
        * CreateNewsfeedEntryForAllMembers test cases
        * Has a newsfeed entry been created for each member?
        * 
        * text is empty exception
        * text is null exception
        */
        [Fact]
        public async System.Threading.Tasks.Task CreateNewsfeedEntryForAllMembersAsyncTest()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context, projectRoleRepository);

            // Target project with id = 2
            var projectMembers = ListOfProjectMemberEntries().Where(c => c.FK_ProjectId == 2);

            // Act
            await dashboardRepository.CreateNewsfeedEntryForAllMembersAsync(2, "CreateNewsfeedEntryForAllMembersAsyncTest");

            // Assert
            // Has a newsfeed entry been created for each member?
            // Newsfeed entry with message "CreateNewsfeedEntryForAllMembersAsyncTest"?
            Assert.Equal(context.NewsfeedEntries.Where(c => c.NewsText == "CreateNewsfeedEntryForAllMembersAsyncTest").Count(), projectMembers.Count());
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateNewsfeedEntryForAllMembersAsyncTest_ExceptionTextIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context, projectRoleRepository);

            // Target project with id = 2

            // Act & Assert
            // If text is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await dashboardRepository.CreateNewsfeedEntryForAllMembersAsync(2, null));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateNewsfeedEntryForAllMembersAsyncTest_ExceptionTextIsEmpty()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            DashboardRepository dashboardRepository = new DashboardRepository(context, projectRoleRepository);

            // Target project with id = 2
            // Act & Assert
            // If text is empty ("").
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await dashboardRepository.CreateNewsfeedEntryForAllMembersAsync(2, ""));
        }

        /*
        * DeleteAllEntriesOfUserAsync test cases
        * Are there any entries left by the user after the deletion?
        * 
        * userId null exception
        */
        [Fact]
        public async System.Threading.Tasks.Task DeleteAllEntriesOfUserAsyncTest_SuccessfullyDeleted()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act
            // 2301D884-221A-4E7D-B509-0113DCC043E1 = Manager with 2 entries in the sample data
            await dashboardRepository.DeleteAllEntriesOfUserAsync("2301D884-221A-4E7D-B509-0113DCC043E1");

            // Assert
            // Are there any entries left by the user after the deletion?
            Assert.Equal(0, context.NewsfeedEntries.Where(c => c.User.Id == "2301D884-221A-4E7D-B509-0113DCC043E1").Count());
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteAllEntriesOfUserAsyncTest_ExceptionUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            DashboardRepository dashboardRepository = new DashboardRepository(context);

            // Act & Assert
            // userId null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await dashboardRepository.DeleteAllEntriesOfUserAsync(null));
        }

        /* 
         * The following methods are not tested because they are based on the CreateNewsfeedEntryAsync() method, for which a test already exists.
         * 
         * CreateNewsdeedEntryForNewCommentAsync()
         * CreateNewsdeedEntryForTicketStatusChangeAsync()
        */
    }
}

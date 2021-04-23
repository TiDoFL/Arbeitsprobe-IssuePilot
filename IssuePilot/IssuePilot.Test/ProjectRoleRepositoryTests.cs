using IssuePilot.Models.Repositorys;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{
    public class ProjectRoleRepositoryTests : InitDbWithData
    {
        /*
         * AddprojectMemberEntryAsync test cases
         * Has the user been assigned to the project?
         * 
         * User id is null exception
         * User is null exception
         * Role is null exception
         * Project is null exception
         */
        [Fact]
        public async Task AddprojectMemberEntryAsyncTest_UserIsAssignedToProject()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRepository = new ProjectRoleRepository(context);
            var userId = "2301D884-221A-4E7D-B509-0113DCC043E2";
            var roleId = ListOfProjectRoles().First().Id;
            var projectId = 3;

            // Act
            await projectRepository.AddProjectMemberEntryAsync(userId, roleId, projectId);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync(e => e.FK_ProjectId == projectId && e.FK_ProjectRoleId == roleId && e.FK_UserId == userId);

            // Assert
            // Has the user been assigned to the project?
            Assert.NotNull(entry);
        }

        [Fact]
        public async Task AddprojectMemberEntryAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var roleId = ListOfProjectRoles().First().Id;
            var projectId = ListOfProjects().First().Id;

            // Act & Assert
            // If userId is null exception.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRoleRepository.AddProjectMemberEntryAsync(null, roleId, projectId));
        }

        [Fact]
        public async Task AddprojectMemberEntryAsyncTest_ExceptionUserIdIsEmpty()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var roleId = ListOfProjectRoles().First().Id;
            var projectId = ListOfProjects().First().Id;

            // Act & Assert
            // User id is empty ("")
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRoleRepository.AddProjectMemberEntryAsync("", roleId, projectId));
        }

        [Fact]
        public async Task AddprojectMemberEntryAsyncTest_ExceptionRoleIdNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsersWithId().First().Id;
            var projectId = ListOfProjects().First().Id;

            // Act & Assert
            // Role is null exception
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRoleRepository.AddProjectMemberEntryAsync(userId, 300, projectId));
        }

        [Fact]
        public async Task AddprojectMemberEntryAsyncTest_ExceptionProjectIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsersWithId().First().Id;
            var roleId = ListOfProjectRoles().First().Id;

            // Act & Assert
            // Project is null exception
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRoleRepository.AddProjectMemberEntryAsync(userId, roleId, 300));
        }

        /*
         * GetListOfProjectsOfUserByIdAsync test cases
         * Is the correct number of list entries being returned (single, none)?
         * 
         * User id is null exception
         */
        [Fact]
        public async Task GetProjectsOfUserByIdAsyncTest_SingleResult()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsersWithId()[1].Id;

            // Act
            var result = await projectRoleRepository.GetProjectsOfUserByIdAsync(userId);

            // Assert
            // Is the correct number of list entries being returned?
            Assert.Single(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetProjectsOfUserByIdAsyncTest_ZeroResult()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);

            // userID (empty) = 2301D884-221A-4E7D-B509-0113DCC043E3
            // Act
            var result = await projectRoleRepository.GetProjectsOfUserByIdAsync("2301D884-221A-4E7D-B509-0113DCC043E3");

            // Assert
            // Is the correct number of list entries being returned?
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProjectsOfUserByIdAsyncTest_Exception()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);

            // Act & Assert
            // If userId is null exception.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRoleRepository.GetProjectsOfUserByIdAsync(null));
        }

        /*
         * GetProjectRoleOfUserAsync test cases
         * Is the correct role being returned?
         * 
         * User id is null exception
         * entry is null exception
         */
        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_ReturnsCorrectRole()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsersWithId()[1].Id;
            var projectId = ListOfProjects()[1].Id;
            var expectedEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectId && r.FK_UserId == userId);

            // Act
            var projectRole = await projectRoleRepository.GetProjectRoleOfUserAsync(userId, projectId);

            // Assert
            // Is the correct role being returned?
            Assert.Equal(expectedEntry.ProjectRole.Id, projectRole.Id);
        }

        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectId = ListOfProjects()[1].Id;

            // Act & Assert
            // If userId is null exception.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRoleRepository.GetProjectRoleOfUserAsync(null, projectId));
        }

        [Fact]
        public async Task GetProjectRoleOfUserAsyncTest_ExceptionProjectIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var userId = ListOfUsersWithId()[1].Id;

            // Act & Assert
            // If project is null.
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRoleRepository.GetProjectRoleOfUserAsync(userId, 300));
        }

        /*
         * GetMembersOfProjectByIdAsync test cases
         * There are users in project.
         * There are no users in project.
         * 
         * project id does not exists.
         */
        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_ResultWithUsers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);

            // Act
            var resultWithUsers = await projectRoleRepository.GetMembersOfProjectByIdAsync(ListOfProjects()[1].Id);

            // Assert
            // There are users in project.
            Assert.Equal(3, resultWithUsers.Count);
            Assert.NotEmpty(resultWithUsers);
        }

        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_ResultWithoutUsers()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);

            // Act
            var resultWithoutUsers = await projectRoleRepository.GetMembersOfProjectByIdAsync(ListOfProjects()[2].Id);

            // Assert
            // There are no users in project.
            Assert.Empty(resultWithoutUsers);
        }

        [Fact]
        public async Task GetMembersOfProjectByIdAsyncTest_Exception()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);

            // Act & Assert
            // project id does not exists.
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRoleRepository.GetMembersOfProjectByIdAsync(300));
        }

        /*
         * GetAllNonMembersOfProjectAsync test cases
         * Are all users returned who are not part of the project?
         */
        [Fact]
        public async Task GetAllNonMembersOfProjectAsyncTest_ReturnsCorrectNumberOfItemsInList()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectId = ListOfProjects()[1].Id;
            var listOfEntries = await context.ProjectMemberEntries.Where(r => r.FK_ProjectId == projectId).ToListAsync();
            var listOfUsers = await context.Users.ToListAsync();

            // Act
            var listOfNonMembersOfProject = await projectRoleRepository.GetAllNonMembersOfProjectAsync(projectId);

            // Assert
            // Are all users returned who are not part of the project?
            Assert.Equal(listOfUsers.Count - listOfEntries.Count, listOfNonMembersOfProject.Count);
        }

        /*
         * RemoveMemberFromProjectAsync test cases
         * Has the entry been deleted?
         * 
         * userId is null exception
         * entry is null exception
         */
        [Fact]
        public async Task RemoveMemberFromProjectAsyncTest_SuccessfullyRemoved()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync();

            // Act
            await projectRoleRepository.RemoveMemberFromProjectAsync(entry.FK_ProjectId, entry.FK_UserId);

            // Assert
            // Has the entry been deleted?
            Assert.Null(await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == entry.FK_ProjectId && r.FK_UserId == entry.FK_UserId));
        }

        [Fact]
        public async Task RemoveMemberFromProjectAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var entry = await context.ProjectMemberEntries.FirstOrDefaultAsync();

            // Act & Assert
            // If userId is null exception.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRoleRepository.RemoveMemberFromProjectAsync(entry.FK_ProjectId, null));
        }

        /*
         * UpdateProjectMemberRoleAsync test cases
         * Has the role been updated?
         * 
         * userId is null exception
         * entry is null exception
         */
        [Fact]
        public async Task UpdateProjectMemberRoleAsyncTest_SuccessfullyUpdated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectMemberEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync();

            // Act
            await projectRoleRepository.UpdateProjectMemberRoleAsync(projectMemberEntry.FK_ProjectId, projectMemberEntry.FK_UserId, projectMemberEntry.FK_ProjectRoleId, 2);

            // Assert
            var updatedEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync(r => r.FK_ProjectId == projectMemberEntry.FK_ProjectId && r.FK_UserId == projectMemberEntry.FK_UserId);
            // Has the role been updated?
            Assert.Equal(2, updatedEntry.FK_ProjectRoleId);
        }

        [Fact]
        public async Task UpdateProjectMemberRoleAsyncTest_ExceptionUserIdIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            var projectMemberEntry = await context.ProjectMemberEntries.FirstOrDefaultAsync();

            // Act & Assert
            // If userId is null exception.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRoleRepository.UpdateProjectMemberRoleAsync(projectMemberEntry.FK_ProjectId, null, projectMemberEntry.FK_ProjectRoleId, 2));
        }
    }
}

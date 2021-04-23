using IssuePilot.Models;
using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels;
using IssuePilot.Test.TestData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IssuePilot.Test
{
    public class ProjectRepositoryTests : InitDbWithData
    {
        /*
         * CreateProjectAsync test cases
         * Did the method add DateTime.Now?
         * Does an additional project in db aside from seed data exist?
         * Has the project been assigned to the creator? 
         * Does an additional project in db aside from seed data exist?
         * 
         * Title already exists exception
         * userId is null exception
         * model is null exception
         * title is null exception
         */
        [Fact]
        public async Task CreateProjectAsyncTest_WithTitleAndDescription()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, userId);

            // Assert
            // Does Title and Description exists?
            Assert.Equal(resultModel.Title, modelToSave.Title);
            Assert.Equal(resultModel.Description, modelToSave.Description);
        }

        [Fact]
        public async Task CreateProjectAsyncTest_HasProjectDataBeenCreatedByTheSystem()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            var listOfTicketCategories = ListOfTicketCategories();
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, userId);
            var allProjects = await context.Projects.ToListAsync();
            var user = await context.Users.FindAsync(userId);

            // Assert
            Assert.NotNull(resultProject);
            Assert.Equal(resultProject.Title, modelToSave.Title);
            Assert.Equal(resultProject.Description, modelToSave.Description);

            // Did the method add DateTime.Now?
            Assert.NotEqual(resultProject.CreateDate, DateTime.MinValue);

            // Does an additional project in db aside from seed data exist?
            Assert.Equal(4, allProjects.Count);

            // Has the project been assigned to the creator? 
            Assert.NotNull(user.Projects.FirstOrDefault(r => r.Id == resultProject.Id));

            // Did the default categories get created?
            // seeded data + 5 default  
            var listOfCategories = context.TicketCategories.Select(p => p.Project.Id == resultProject.Id);
            Assert.Equal(listOfTicketCategories.Count + 5, listOfCategories.Count());
        }

        [Fact]
        public async Task CreateProjectAsyncTest_OnlyTitle()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle" };

            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, userId);

            // Does Title exists?
            Assert.Null(resultModel.Description);
            Assert.Equal(resultModel.Title, modelToSave.Title);
        }


        [Fact]
        public async Task CreateProjectAsyncTest_ExceptionTitleAlreadyExists()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act
            var (resultModel, resultProject) = await projectRepository.CreateProjectAsync(modelToSave, userId);
            var (resultModelFail, resultProjectFail) = await projectRepository.CreateProjectAsync(new ProjectUpdateViewModel { Title = "TestProject1" }, userId);

            // Assert
            // Title already exists
            Assert.True(resultModelFail.TitleExists);
        }

        [Fact]
        public async Task CreateProjectAsyncTest_ExceptionUserIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act & Assert
            // If userId is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.CreateProjectAsync(modelToSave, null));
        }

        [Fact]
        public async Task CreateProjectAsyncTest_ExceptionModelIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act & Assert
            // Model is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.CreateProjectAsync(null, userId));
        }

        [Fact]
        public async Task CreateProjectAsyncTest_ExceptionTitleIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            ProjectUpdateViewModel modelToSave = new ProjectUpdateViewModel() { Title = "SaveTestProjectTitle", Description = "A Test Project from a Test!" };

            // Act & Assert
            // Title is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.CreateProjectAsync(new ProjectUpdateViewModel { Title = null }, userId));
        }

        /*
         * CreateTicketCategoryAsync test cases
         * Does an additional category in db aside from seed data exist?
         * Is the category created in the database?
         * 
         * Project is null exception
         * Title is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task CreateTicketCategoryAsyncTest_IsNewTicketCategoryCreated()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = context.Projects.FirstOrDefault();
            List<TicketCategory> listOfTicketCategories = ListOfTicketCategories();

            // Act
            TicketCategory createdCategory = await projectRepository.CreateTicketCategoryAsync("TestBug", project);

            // Assert
            // Does an additional category in db aside from seed data exist?
            var allCategories = await context.TicketCategories.ToListAsync();
            Assert.Equal(listOfTicketCategories.Count + 1, allCategories.Count);

            // Is the category created in the database?
            Assert.NotNull(await context.TicketCategories.FindAsync(createdCategory.Id));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTicketCategoryAsyncTest_ExceptionProjectIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);

            // Act & Assert
            // Project is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.CreateTicketCategoryAsync("TestBug", null));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTicketCategoryAsyncTest_ExceptionTitleIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = context.Projects.FirstOrDefault();

            // Act & Assert
            // Title is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.CreateTicketCategoryAsync(null, project));
        }

        /*
         * UpdateProjectAsync test cases
         * Are title and description updated?
         * Is description updated if title is unchanged?
         * Aboard update if other project already has this title.
         * 
         * Model is null exception
         * Title is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsyncTest_SucceedTitleAndDescriptionUpdate()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = await context.Projects.FirstOrDefaultAsync();

            string newTitle = "newTitle";
            string description = "updated";

            ProjectUpdateViewModel modelSucceedTitleAndDescription = new ProjectUpdateViewModel() { Id = project.Id, Title = newTitle, Description = description };

            // Act
            var updateSucceedTitleAndDescription = await projectRepository.UpdateProjectAsync(modelSucceedTitleAndDescription);

            // Assert
            // Are title and description updated?
            Assert.Equal(description, updateSucceedTitleAndDescription.Description);
            Assert.Equal(newTitle, updateSucceedTitleAndDescription.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsyncTest_SucceedDescriptionUpdate()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            Project project = await context.Projects.FirstOrDefaultAsync();

            string description = "updated2";

            ProjectUpdateViewModel modelSucceedDescription = new ProjectUpdateViewModel() { Id = project.Id, Title = project.Title, Description = description };

            // Act
            var updateSucceedDescription = await projectRepository.UpdateProjectAsync(modelSucceedDescription);

            // Assert
            // Is description updated if title is unchanged?
            Assert.Equal(description, updateSucceedDescription.Description);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsyncTest_UpdateFailed()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            List<Project> listOfProjects = ListOfProjects();
            Project project = await context.Projects.FirstOrDefaultAsync();

            ProjectUpdateViewModel modelFailed = new ProjectUpdateViewModel() { Id = project.Id, Title = listOfProjects[1].Title };

            // Act
            var updateFailed = await projectRepository.UpdateProjectAsync(modelFailed);

            // Assert
            // Aboard update if other project already has this title.
            Assert.True(updateFailed.TitleExists);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsyncTest_ExceptionModelIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);

            // Act & Assert
            // Model is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.UpdateProjectAsync(null));

        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateProjectAsyncTest_ExceptionTitleIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);

            // Act & Assert
            // Title is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.UpdateProjectAsync(new ProjectUpdateViewModel() { Title = null }));
        }

        /*
         * UpdateTicketCategoriesAsync test cases
         * Is the category-name updated?
         * 
         * Model is null exception
         * Name is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task UpdateTicketCategoriesAsyncTest()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);
            TicketCategory ticketCategory = await context.TicketCategories.FirstOrDefaultAsync();

            // Update succeed
            CategoryCreateInputModel model = new CategoryCreateInputModel() { CategoryId = ticketCategory.Id, Name = "updated" };

            // Act
            await projectRepository.UpdateTicketCategoriesAsync(model);

            // Assert
            // Is the category-name updated?
            var dbCategory = await context.TicketCategories.FindAsync(ticketCategory.Id);
            Assert.Equal("updated", dbCategory.Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTicketCategoriesAsyncTest_ExceptionModelIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);

            // Act & Assert
            // Model is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.UpdateTicketCategoriesAsync(null));
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTicketCategoriesAsyncTest_ExceptionTitleIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRepository projectRepository = new ProjectRepository(context);

            // Act & Assert
            // Title is null exception
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await projectRepository.UpdateTicketCategoriesAsync(new CategoryCreateInputModel() { Name = null }));
        }

        /*
         * DeleteProjectAsync test cases
         * Has an entry been deleted?
         * Has the correct entry been deleted?
         * 
         * Tickets or project is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task DeleteProjectAsyncTest()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context, ticketRepository);

            List<Project> listOfProjects = ListOfProjects();

            // Act
            await projectRepository.DeleteProjectAsync(listOfProjects[0].Id);

            // Assert
            // Has an entry been deleted?
            var dbProjectList = await context.Projects.ToListAsync();
            Assert.Equal(listOfProjects.Count - 1, dbProjectList.Count);

            // Has the correct entry been deleted?
            Assert.Null(await context.Projects.FindAsync(listOfProjects[0].Id));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteProjectAsyncTest_ExceptionTicketOrProjectsIsNull()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context, ticketRepository);

            // Act & Assert
            // Tickets or projects is null exception
            await Assert.ThrowsAsync<NullReferenceException>(async () => await projectRepository.DeleteProjectAsync(300));
        }
    }
}

using IssuePilot.Models.ViewModels;
using IssuePilot.Models.Repositorys;
using IssuePilot.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using IssuePilot.Models;

namespace IssuePilot.Test
{
    public class TicketRepositoryTests : InitDbWithData
    {
        /*
         * CreateTicketAsync test cases
         * Did the method add DateTime.Now?
         * Does an  additional project in db aside from seed data exist?
         * Has the project been assigned to the creator? 
         * 
         * userId is null exception
         * model is null exception
         * title is null exception
         */
        [Fact]
        public async Task CreateTicketAsyncTest_WithDescription()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle", Description = "A Test Ticket from a Test!"};
            Project project = context.Projects.Find(1);


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            //Do Title and Description exist?
            Assert.Equal(resultTicket.Title, modelToSave.Title);
            Assert.Equal(resultTicket.Description, modelToSave.Description);
        }
        [Fact]
        public async Task CreateTicketAsyncTest_SystemData()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.Find(1);
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle", Description = "A Test Ticket from a Test!", Deadline = DateTime.Now, Weight = 1 };
            


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            //Do CreateDate, Deadline, Status and Weight exist?
            Assert.NotEqual(resultTicket.CreateDate, DateTime.MinValue);
            Assert.Equal(resultTicket.Deadline, modelToSave.Deadline);
            Assert.Equal(1, resultTicket.Status.Id);
            Assert.Equal(resultTicket.Weight, modelToSave.Weight);
        }

        [Fact]
        public async Task CreateTicketAsyncTest_IsValueNull()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.Find(1);
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle" };



            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            //Do Deadline, Description not exist?
            Assert.Null(resultTicket.Deadline);
            Assert.Null(resultTicket.Description);
        }


        [Fact]
        public async Task CreateTicketAsyncTest_UserIdIsNullException()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.Find(1);
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle" };


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            // If userId is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await ticketRepository.CreateTicketAsync(modelToSave, project, null, new List<Image>()));
        }
        [Fact]
        public async Task CreateTicketAsyncTest_ModelIsNullException()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.Find(1);
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle" };


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            // If model is null
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await ticketRepository.CreateTicketAsync(null, project, userId, new List<Image>()));
        }
        [Fact]
        public async Task CreateTicketAsyncTest_ProjectIsNullException()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.Find(1);
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle" };


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            // If project is null
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await ticketRepository.CreateTicketAsync(modelToSave, null, userId, new List<Image>()));
        }
        [Fact]
        public async Task CreateTicketAsyncTest_TitleIsNullException()
        {
            using var context = InitWithDataAndContext();
            // Arrange
            TicketRepository ticketRepository = new TicketRepository(context);
            string userId = ListOfUsersWithId().First().Id;
            Project project = context.Projects.First();
            TicketCreateInputModel modelToSave = new TicketCreateInputModel() { Title = "SaveTestTicketTitle" };


            //Act
            var resultTicket = await ticketRepository.CreateTicketAsync(modelToSave, project, userId, new List<Image>());

            //Assert
            // If title is null
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await ticketRepository.CreateTicketAsync(new TicketCreateInputModel { Title = null }, project, userId, new List<Image>()));
        }
        

    }
}

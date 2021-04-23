using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels.Statistics;
using IssuePilot.Test.TestData;
using System;
using Xunit;

namespace IssuePilot.Test
{
    public class StatisticsRepositoryTests : InitDbWithData
    {
        /*
         * StatisticsAsyncTest test cases
         * Model data is correct if no tickets exist. 
         * Model data is correct if tickets exist.
         */
        [Fact]
        public void StatisticsAsyncTest_NoTicketsExists()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository, ticketRepository);

            // Act
            var viewModelData = statisticsRepository.GetProjectStatisticDataAsync(2);
            StatisticsViewModel model = new StatisticsViewModel();
            // Assert

            // All 3 members have 0 tickets created.
            Assert.Equal(0, viewModelData.Result.ListNumberOfCreatedTicketsByUsers[0].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfCreatedTicketsByUsers[1].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfCreatedTicketsByUsers[2].NumberOfTickets);

            // AVG Processing time is 00:00:00 (TimeSpan)
            Assert.Equal(new TimeSpan(), viewModelData.Result.AVGProcessingTimeOfTicketsInDays);

            // Correct number of created tickets per user?
            Assert.Equal(0, viewModelData.Result.ListNumberOfTicketsAssignedToUser[0].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfTicketsAssignedToUser[1].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfTicketsAssignedToUser[2].NumberOfTickets);

            // Category useage
            Assert.Empty(viewModelData.Result.ListNumberOfTimesCategoryWasUsed);

            // Ticketstatus useage
            foreach (var statuses in viewModelData.Result.ListNumbersOfTicketStatus)
            {
                Assert.Equal(0, statuses.NumberOfTicketsWithStatus);
            }

            // ListOfTicketClosedDate 
            Assert.Empty(viewModelData.Result.ListOfTicketClosedDate);

            // ListOfTicketCreatedDate
            Assert.Empty(viewModelData.Result.ListOfTicketCreatedDate);

            // Number of deleted tickets
            Assert.Equal(0, viewModelData.Result.NumberOfDeletedTickets);

            // Number of members
            Assert.Equal(3, viewModelData.Result.NumberOfMembers);

            // Number of tickets created
            Assert.Equal(0, viewModelData.Result.NumberOfTicketsCreated);

            // Number of tickets over deadline
            Assert.Equal(0, viewModelData.Result.NumberOfTicketsOverDeadline);
        }

        [Fact]
        public void StatisticsAsyncTest_TicketsExists()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            ProjectRoleRepository projectRoleRepository = new ProjectRoleRepository(context);
            ProjectRepository projectRepository = new ProjectRepository(context);
            TicketRepository ticketRepository = new TicketRepository(context);
            StatisticsRepository statisticsRepository = new StatisticsRepository(context, projectRoleRepository, projectRepository, ticketRepository);

            // Act
            var viewModelData = statisticsRepository.GetProjectStatisticDataAsync(1);
            StatisticsViewModel model = new StatisticsViewModel();

            // Assert
            // Correct number of created tickets per user?
            Assert.Equal(1, viewModelData.Result.ListNumberOfCreatedTicketsByUsers[0].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfCreatedTicketsByUsers[1].NumberOfTickets);

            // All 2 members were allocated 0 tickets.
            Assert.Equal(0, viewModelData.Result.ListNumberOfTicketsAssignedToUser[0].NumberOfTickets);
            Assert.Equal(0, viewModelData.Result.ListNumberOfTicketsAssignedToUser[1].NumberOfTickets);

            // Category useage
            Assert.Empty(viewModelData.Result.ListNumberOfTimesCategoryWasUsed);

            // ListOfTicketClosedDate 
            Assert.Empty(viewModelData.Result.ListOfTicketClosedDate);

            // ListOfTicketCreatedDate
            Assert.Single(viewModelData.Result.ListOfTicketCreatedDate);

            // Number of members
            Assert.Equal(2, viewModelData.Result.NumberOfMembers);

            // Number of tickets created
            Assert.Equal(1, viewModelData.Result.NumberOfTicketsCreated);
        }

    }
}

using IssuePilot.Models;
using IssuePilot.Models.Repositorys;
using IssuePilot.Models.ViewModels;
using IssuePilot.Test.TestData;
using System;
using System.Collections.Generic;
using Xunit;

namespace IssuePilot.Test
{

    public class UserRepositoryTests : InitDbWithData
    {
        /*
         * The following method (s) are not tested because they use the methods of the identity system (.net core 3.1).
         * AddUserAsync()
         */


        /* GenerateUserName Test */
        [Fact]
        public void GenerateUserNameTest_RetrunsValidName()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);

            // Act
            var username = userRepository.GenerateUserName("Test", "er");

            // Assert
            Assert.Contains("Tester", username);
        }

        /*
         * GetUserByIdAsync test cases
         * 
         * userId is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task GetUserByIdAsyncTest_Exception()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);

            // Act & Assert
            // If userId is null.
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.GetUserByIdAsync(null));
        }

        /*
         * UpdateUserAsync test cases
         * Is the email being updated if changed?
         * Is the email being updated if not changed?
         * Is the firstname being updated?
         * Is the surname being updated?
         * Is the password being updated?
         * 
         * editUserModel is null exception
         * databaseUser is null exception
         */
        [Fact]
        public async System.Threading.Tasks.Task UpdateUserAsyncTest_updateAll()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            List<User> listOfUsers = ListOfUsersWithId();
            User userToUpdate = listOfUsers[1];

            // Create and fill model with data
            UserEditViewModel model = new UserEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = "UpdatedFirstname",
                Surname = "UpdatedSurname",
                Email = "UpdatedEmail",
                Password = "UpdatedPassword"
            };

            // Act
            User updatedUser = await userRepository.UpdateUserAsync(model);

            // Assert
            // Is the email being updated if changed ?
            Assert.NotEqual(userToUpdate.Email, updatedUser.Email);
            Assert.Equal(updatedUser.Email, model.Email);

            // Is the firstname being updated?
            Assert.NotEqual(userToUpdate.Firstname, updatedUser.Firstname);
            Assert.Equal(updatedUser.Firstname, model.Firstname);

            // Is the surname being updated?
            Assert.NotEqual(userToUpdate.Surname, updatedUser.Surname);
            Assert.Equal(updatedUser.Surname, model.Surname);

            // Is the password being updated?
            Assert.NotEqual(userToUpdate.PasswordHash, updatedUser.PasswordHash);
            Assert.Equal(updatedUser.PasswordHash, updatedUser.PasswordHash);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateUserAsyncTest_UpdateWithoutChanges()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            List<User> listOfUsers = ListOfUsersWithId();
            User userToUpdate = listOfUsers[1];

            // Create and fill model with data
            UserEditViewModel model = new UserEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = userToUpdate.Firstname,
                Surname = userToUpdate.Surname,
                Email = userToUpdate.Email
            };

            // Act
            User updatedUser = await userRepository.UpdateUserAsync(model);

            // Assert
            // Has the data changed?
            Assert.Equal(updatedUser.Email, userToUpdate.Email);
            Assert.Equal(updatedUser.Firstname, userToUpdate.Firstname);
            Assert.Equal(updatedUser.Surname, userToUpdate.Surname);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateUserAsyncTest_UpdateEmptyStringAttributes()
        {
            using var context = InitWithDataAndContext();

            // Arrange
            UserRepository userRepository = new UserRepository(context);
            List<User> listOfUsers = ListOfUsersWithId();
            User userToUpdate = listOfUsers[1];

            // Create and fill model with data
            UserEditViewModel model = new UserEditViewModel
            {
                Id = userToUpdate.Id,
                Firstname = "",
                Surname = "",
                Email = ""
            };

            // Act
            User updatedUser = await userRepository.UpdateUserAsync(model);

            // Assert
            // Has the data changed?
            Assert.Equal(updatedUser.Email, model.Email);
            Assert.Equal(updatedUser.Firstname, model.Firstname);
            Assert.Equal(updatedUser.Surname, model.Surname);
        }

        //[Fact]
        //public async System.Threading.Tasks.Task UpdateUserAsyncTest_ExceptionModelIsNull()
        //{
        //    using var context = InitWithDataAndContext();

        //    // Arrange
        //    UserRepository userRepository = new UserRepository(context);

        //    // Act & Assert
        //    // editUserModel is null exception
        //    await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.UpdateUserAsync(null));
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task UpdateUserAsyncTest_ExceptionUserIsNull()
        //{
        //    using var context = InitWithDataAndContext();

        //    // Arrange
        //    UserRepository userRepository = new UserRepository(context);

        //    // Act & Assert
        //    // databaseUser is null exception
        //    await Assert.ThrowsAsync<NullReferenceException>(async () => await userRepository.UpdateUserAsync(new UserEditViewModel { Id = "" }));
        //}

        ///*
        // * DeleteUserAsync test cases
        // * Does the username contains "gelöschterNutzer"?
        // * Has the createDate been set to MinValue?
        // * Has the email been set to null?
        // * Has the normalizedEmail been set to null?
        // * Has the firstname been set to null?
        // * Has the surname been set to null?
        // * Has the passwordHash been set to null?
        // * Has the phoneNumber been set to null?
        // * Has the user deleted been set to true?
        // * 
        // * userId is null exception
        // * databaseUser is null exception
        // */
        //[Fact]
        //public async System.Threading.Tasks.Task DeleteUserAsyncTest_PersonalDataRemoved()
        //{
        //    using var context = InitWithDataAndContext();

        //    // Arrange
        //    DashboardRepository dashboardRepository = new DashboardRepository(context);
        //    UserRepository userRepository = new UserRepository(context, dashboardRepository);
        //    List<User> listOfUsers = ListOfUsersWithId();
        //    User userToDelete = listOfUsers[1];

        //    // Act
        //    await userRepository.DeleteUserAsync(userToDelete.Id);
        //    var deletedUser = context.Users.Find(userToDelete.Id);

        //    // Assert
        //    // Does the username contains "gelöschterNutzer"?
        //    Assert.Contains("gelöschterNutzer", deletedUser.UserName);

        //    // Has the createDate been set to MinValue?
        //    Assert.Equal(deletedUser.CreateDate, DateTime.MinValue);

        //    // Has the email been set to null?
        //    Assert.Null(deletedUser.Email);

        //    // Has the normalizedEmail been set to null?
        //    Assert.Null(deletedUser.NormalizedEmail);

        //    // Has the firstname been set to null?
        //    Assert.Null(deletedUser.Firstname);

        //    // Has the surname been set to null?
        //    Assert.Null(deletedUser.Surname);

        //    // Has the passwordHash been set to null?
        //    Assert.Null(deletedUser.PasswordHash);

        //    // Has the phoneNumber been set to null?
        //    Assert.Null(deletedUser.PhoneNumber);

        //    // Has the user deleted been set to true?
        //    Assert.True(deletedUser.IsDeleted);
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteUserAsyncTest_ExceptionUserIdIsNull()
        //{
        //    using var context = InitWithDataAndContext();

        //    // Arrange
        //    DashboardRepository dashboardRepository = new DashboardRepository(context);
        //    UserRepository userRepository = new UserRepository(context, dashboardRepository);
        //    List<User> listOfUsers = ListOfUsersWithId();
        //    User userToDelete = listOfUsers[1];

        //    // Act
        //    await userRepository.DeleteUserAsync(userToDelete.Id);

        //    // Act & Assert
        //    // userId is null exception
        //    await Assert.ThrowsAsync<ArgumentNullException>(async () => await userRepository.DeleteUserAsync(null));
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteUserAsyncTest_ExceptionUserIsNull()
        //{
        //    using var context = InitWithDataAndContext();

        //    // Arrange
        //    DashboardRepository dashboardRepository = new DashboardRepository(context);
        //    UserRepository userRepository = new UserRepository(context, dashboardRepository);
        //    List<User> listOfUsers = ListOfUsersWithId();
        //    User userToDelete = listOfUsers[1];

        //    // Act
        //    await userRepository.DeleteUserAsync(userToDelete.Id);

        //    // Act & Assert
        //    // databaseUser is null exception
        //    await Assert.ThrowsAsync<NullReferenceException>(async () => await userRepository.DeleteUserAsync(""));
        //}
    }
}

using IssuePilot.Data;
using IssuePilot.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IssuePilot.Test.TestData
{
    public class InitDbWithData
    {
        public ApplicationDbContext InitWithDataAndContext()
        {
            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>()
               .EnableSensitiveDataLogging()
               .UseInMemoryDatabase(Guid.NewGuid().ToString());

            ApplicationDbContext context = new ApplicationDbContext(builder.Options);

            //--------------
            // Seed data
            //--------------

            // User
            var usersToSeed = ListOfUsersWithId();
            foreach (var user in usersToSeed)
            {
                context.Users.Add(user);
            }

            // IdentityRole
            var rolesToSeed = ListOfIdentityRolesWithId();
            foreach (var roles in rolesToSeed)
            {
                context.Roles.Add(roles);
            }

            // ProjectRole
            var projectRolesToSeed = ListOfProjectRoles();
            foreach (var roles in projectRolesToSeed)
            {
                context.ProjectRoles.Add(roles);
            }

            // Projects
            var projectToSeed = ListOfProjects();
            foreach (var project in projectToSeed)
            {
                context.Projects.Add(project);
            }

            // ProjectMemberEntries
            var projectMember = ListOfProjectMemberEntries();
            foreach (var member in projectMember)
            {
                context.ProjectMemberEntries.Add(member);
            }
            context.SaveChanges();

            // TicketCategories
            var ticketCategory = ListOfTicketCategories();
            foreach (var category in ticketCategory)
            {
                // Add category to project with id 3
                // All seeded ticket categories are assigned to the project with id = 3.
                category.Project = context.Projects.Find(3);
                context.TicketCategories.Add(category);
            }
            context.SaveChanges();

            // Newsfeed
            var newsfeedEntry = ListOfNewsfeedEntry();
            foreach (var entry in newsfeedEntry)
            {
                entry.User = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E1"); // Manager
                context.NewsfeedEntries.Add(entry);
            }
            context.SaveChanges();

            // TicketStatus
            var statuses = ListOfStatuses();
            foreach (var status in statuses)
            {
                context.TicketStatuses.Add(status);
            }
            context.SaveChanges();

            // Tickets
            var tickets = ListOfTickets();
            foreach (var ticket in tickets)
            {
                ticket.TicketCreator = context.Users.Find("2301D884-221A-4E7D-B509-0113DCC043E0");
                ticket.Status = context.TicketStatuses.Find(1);
                ticket.Project = context.Projects.Find(1);
                context.Tickets.Add(ticket);
            }
            context.SaveChanges();


            context.Database.EnsureCreated();
            return context;
        }

        //--------------
        // Data to seed
        //--------------

        // Integer Id's need to start at 1.
        public List<User> ListOfUsersWithId()
        {
            return new List<User>
            {
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E0",
                    Firstname = "ad",
                    UserName = "admin",
                    Surname = "min",
                    NormalizedUserName = "ADMIN",
                    Email = "Admin@Admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM"
                },
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E1",
                    Firstname = "man",
                    Surname = "ager",
                    UserName = "Manager",
                    NormalizedUserName = "MANAGER",
                    Email = "Manager@Admin.com",
                    NormalizedEmail = "MANAGER@ADMIN.COM"
                },
               new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E2",
                    Firstname = "Ben",
                    Surname = "utzer",
                    UserName = "Benutzer",
                    NormalizedUserName = "BENUTZER",
                    Email = "benutzer@Admin.com",
                    NormalizedEmail = "BENUTZER@ADMIN.COM"
                },
                new User
                {
                    Id = "2301D884-221A-4E7D-B509-0113DCC043E3",
                    Firstname = "empty",
                    Surname = "empty",
                    UserName = "empty",
                    NormalizedUserName = "EMPTY",
                    Email = "empty@Admin.com",
                    NormalizedEmail = "EMPTY@ADMIN.COM"
                }

            };
        }

        public List<IdentityRole> ListOfIdentityRolesWithId()
        {
            return new List<IdentityRole>
                {
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8860",
                         Name = "Admin",
                         NormalizedName="ADMIN"
                    },
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8861",
                        Name = "Projektmanager",
                        NormalizedName = "PROJEKTMANAGER"
                    },
                    new IdentityRole
                    {
                        Id = "dc78daf-e0e7-4da1-873d-7b2ef7cd8862",
                        Name = "Benutzer",
                        NormalizedName = "BENUTZER"
                    }
                };
        }

        public List<ProjectRole> ListOfProjectRoles()
        {
            return new List<ProjectRole>
                {
                    new ProjectRole
                    {
                        Id = 1,
                        Title = "Eigentümer/in"
                    },
                    new ProjectRole
                    {
                        Id = 2,
                        Title = "Teilnehmer/in"
                    }
                };
        }

        public List<Project> ListOfProjects()
        {
            return new List<Project>
                {
                    new Project
                    {
                        Id = 1,
                        Title = "TestProject1",
                        CreateDate = DateTime.Now,
                         DeletedTicketsCount = 0,
                          Description = "Erste Beschreibung!"
                    },
                    new Project
                    {
                        Id = 2,
                        Title = "TestProject2",
                        CreateDate = DateTime.Now,
                        DeletedTicketsCount = 0,
                        Description = "Zweite Beschreibung!"
                    },
                     new Project
                    {
                        Id = 3,
                        Title = "TestProject3",
                        CreateDate = DateTime.Now,
                        DeletedTicketsCount = 0,
                        Description = "dritte Beschreibung!"
                    }
                };
        }

        public List<ProjectMemberEntry> ListOfProjectMemberEntries()
        {
            return new List<ProjectMemberEntry>
                {
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsersWithId()[1].Id,
                         FK_ProjectId = ListOfProjects()[1].Id,
                         FK_ProjectRoleId = ListOfProjectRoles().First().Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsersWithId()[2].Id,
                         FK_ProjectId = ListOfProjects()[1].Id,
                         FK_ProjectRoleId = ListOfProjectRoles().First().Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsersWithId()[0].Id,
                         FK_ProjectId = ListOfProjects()[1].Id,
                         FK_ProjectRoleId = ListOfProjectRoles().First().Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsersWithId()[2].Id,
                         FK_ProjectId = ListOfProjects()[0].Id,
                         FK_ProjectRoleId = ListOfProjectRoles().First().Id
                    },
                    new ProjectMemberEntry
                    {
                         FK_UserId = ListOfUsersWithId()[0].Id,
                         FK_ProjectId = ListOfProjects()[0].Id,
                         FK_ProjectRoleId = ListOfProjectRoles().First().Id
                    }
                };
        }

        public List<TicketCategory> ListOfTicketCategories()
        {
            return new List<TicketCategory>
                {
                    new TicketCategory
                    {
                        Id = 1,
                        Name = "TestCategory"
                    },
                    new TicketCategory
                    {
                        Id = 2,
                        Name = "Category2"
                    }
                };
        }

        public List<NewsfeedEntry> ListOfNewsfeedEntry()
        {
            return new List<NewsfeedEntry>
                {
                    new NewsfeedEntry
                    {
                        Id = 1,
                        CreateDate = DateTime.Now,
                        NewsText = "Du wurdest zum Projekt - Test - hinzugefügt.",
                        Seen = false
                    },
                    new NewsfeedEntry
                    {
                        Id = 2,
                        CreateDate = DateTime.Now,
                        NewsText = "Es wurde ein Ticket erstellt.",
                        Seen = true,
                    }
                };
        }
        public List<TicketStatus> ListOfStatuses()
        {
            return new List<TicketStatus>
            {
                new TicketStatus
                {
                    Id = 1,
                    Name = "Offen"
                },
                new TicketStatus
                {
                    Id = 2,
                    Name = "Abgeschlossen"
                },
                new TicketStatus
                {
                    Id = 3,
                    Name = "Abgebrochen"
                },
                new TicketStatus
                {
                    Id = 4,
                    Name = "Pausiert"
                },
                new TicketStatus
                {
                    Id = 5,
                    Name = "In Bearbeitung"
                }
            };
        }
        public List<Ticket> ListOfTickets()
        {
            return new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    CreateDate = DateTime.Now,
                    Title = "TestTicket",
                    Description = "Ein vom System generiertes Ticket zum Testen.",
                    Weight = 1
                }
            };
        }

    }
}

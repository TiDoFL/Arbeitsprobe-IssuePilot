using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IssuePilot.Migrations
{
    public partial class finalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Firstname = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntryCase",
                columns: table => new
                {
                    EntryCaseId = table.Column<int>(nullable: false),
                    EntryCaseName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryCase", x => x.EntryCaseId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsfeedEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    NewsText = table.Column<string>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsfeedEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsfeedEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    DeletedTicketsCount = table.Column<int>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMemberEntries",
                columns: table => new
                {
                    FK_ProjectId = table.Column<int>(nullable: false),
                    FK_UserId = table.Column<string>(nullable: false),
                    FK_ProjectRoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMemberEntries", x => new { x.FK_ProjectId, x.FK_ProjectRoleId, x.FK_UserId });
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_Projects_FK_ProjectId",
                        column: x => x.FK_ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_ProjectRoles_FK_ProjectRoleId",
                        column: x => x.FK_ProjectRoleId,
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMemberEntries_AspNetUsers_FK_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketCategories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Deadline = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CloseDate = table.Column<DateTime>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    ClosedFromUserId = table.Column<string>(nullable: true),
                    TicketCreatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_ClosedFromUserId",
                        column: x => x.ClosedFromUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TicketStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_TicketCreatorId",
                        column: x => x.TicketCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(nullable: false),
                    TicketId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketHistoryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    EntryCreatorId = table.Column<string>(nullable: true),
                    TicketId = table.Column<int>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    EntryCaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketHistoryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_EntryCase_EntryCaseId",
                        column: x => x.EntryCaseId,
                        principalTable: "EntryCase",
                        principalColumn: "EntryCaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_AspNetUsers_EntryCreatorId",
                        column: x => x.EntryCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketHistoryEntries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketProjectCategories",
                columns: table => new
                {
                    FK_TicketId = table.Column<int>(nullable: false),
                    FK_TicketCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketProjectCategories", x => new { x.FK_TicketId, x.FK_TicketCategoryId });
                    table.ForeignKey(
                        name: "FK_TicketProjectCategories_TicketCategories_FK_TicketCategoryId",
                        column: x => x.FK_TicketCategoryId,
                        principalTable: "TicketCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketProjectCategories_Tickets_FK_TicketId",
                        column: x => x.FK_TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketWorkers",
                columns: table => new
                {
                    FK_TicketId = table.Column<int>(nullable: false),
                    FK_UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketWorkers", x => new { x.FK_TicketId, x.FK_UserId });
                    table.ForeignKey(
                        name: "FK_TicketWorkers_Tickets_FK_TicketId",
                        column: x => x.FK_TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketWorkers_AspNetUsers_FK_UserId",
                        column: x => x.FK_UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "78A7570F-3CE5-48BA-9461-80283ED1D94D", "25d1c5e0-4c0d-42bb-ba04-f2c72e04847e", "Benutzer", "BENUTZER" },
                    { "2301D884-221A-4E7D-B509-0113DCC043E1", "1372154d-db7b-4501-ba8c-496004da3cba", "Admin", "ADMIN" },
                    { "7D9B7113-A8F8-4035-99A7-A20DD400F6A3", "98e5d257-4bb4-4410-8c70-4a2230222402", "Projektmanager", "PROJEKTMANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateDate", "Email", "EmailConfirmed", "Firstname", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 0, "0f2c9fb8-5963-4bf1-89b2-d3077902ec4f", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "demoadmin@Admin.com", false, "demo", false, false, null, "DEMOADMIN@ADMIN.COM", "DEMOADMIN", "AQAAAAEAACcQAAAAEAbhyzhBcWsV4GWLPdFD0pr53a9CWacGaqtTkPp4sHaKudBNQ7CxoCzEmH05Snz/ug==", null, false, "00000000-0000-0000-0000-000000000000", "admin", false, "DemoAdmin" },
                    { "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", 0, "f24f0a28-caa6-464a-a02f-10648053935d", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "benutzer", false, false, null, null, "BENUTZER", "AQAAAAEAACcQAAAAEM2pDaNkVOolmr65qKKIPnQwJBr5MlhqdgxLNebs/5AmubSQAYZIeQD6xgsYdAmaeQ==", null, true, "00000000-0000-0000-0000-000000000000", null, false, "Benutzer" },
                    { "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "c7bf37f5-c5fa-424e-a4ae-61810fb0baf9", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "manager", false, false, null, null, "MANAGER", "AQAAAAEAACcQAAAAELMNxbjFAizx8engdvPllq2h7k1RPeiGFXNrIi9FOatRfn53dfm/E8mmyK2ZNTXhsw==", null, true, "00000000-0000-0000-0000-000000000000", null, false, "Manager" },
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5F7", 0, "450d20cf-5e43-4482-9a97-a558dcb23fc8", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin@Admin.com", true, "admin", false, false, null, "ADMIN@ADMIN.COM", "ADMIN", "AQAAAAEAACcQAAAAEBZGENkNWVF/4ujpbrfVXLVomPrrxMpMiQ17qu+PEB2n5ptgnMFhFGGAXpsIY0Lu+w==", null, true, "00000000-0000-0000-0000-000000000000", null, false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "EntryCase",
                columns: new[] { "EntryCaseId", "EntryCaseName" },
                values: new object[,]
                {
                    { 0, "UserAdded" },
                    { 6, "TicketInProgress" },
                    { 5, "TicketPaused" },
                    { 4, "TicketCanceled" },
                    { 3, "TicketOpened" },
                    { 2, "TicketClosed" },
                    { 1, "UserRemoved" }
                });

            migrationBuilder.InsertData(
                table: "ProjectRoles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 2, "Teilnehmer/in" },
                    { 1, "Eigentümer/in" }
                });

            migrationBuilder.InsertData(
                table: "TicketStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Offen" },
                    { 3, "Abgebrochen" },
                    { 4, "Pausiert" },
                    { 5, "In Bearbeitung" },
                    { 2, "Abgeschlossen" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5F7", "2301D884-221A-4E7D-B509-0113DCC043E1" },
                    { "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", "2301D884-221A-4E7D-B509-0113DCC043E1" },
                    { "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "7D9B7113-A8F8-4035-99A7-A20DD400F6A3" },
                    { "e28b8357-2955-4f3c-9c3d-1ec6ab1f4335", "78A7570F-3CE5-48BA-9461-80283ED1D94D" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreateDate", "CreatorId", "DeletedTicketsCount", "Description", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 3, 14, 22, 25, 43, 53, DateTimeKind.Local).AddTicks(9918), "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", 0, "Ein vom System generiertes Project zum Testen.", "Testprojekt" },
                    { 2, new DateTime(2021, 3, 14, 22, 25, 43, 59, DateTimeKind.Local).AddTicks(5406), "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", 2, "Ein vom System generiertes zweites Projekt zum Test. Ein vom System generiertes zweites Projekt zum Test.Ein vom System generiertes zweites Projekt zum Test.Ein vom System generiertes zweites Projekt zum Test.Ein vom System generiertes zweites Projekt zum Test.Ein vom System generiertes zweites Projekt zum Test.Ein vom System generiertes zweites Projekt zum Test. Ein vom System generiertes zweites Projekt zum Test.", "Projekt zum Löschen" }
                });

            migrationBuilder.InsertData(
                table: "ProjectMemberEntries",
                columns: new[] { "FK_ProjectId", "FK_ProjectRoleId", "FK_UserId" },
                values: new object[,]
                {
                    { 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1" },
                    { 2, 2, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1" },
                    { 2, 1, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5" }
                });

            migrationBuilder.InsertData(
                table: "TicketCategories",
                columns: new[] { "Id", "Name", "ProjectId" },
                values: new object[,]
                {
                    { 8, "Dokumentation", 2 },
                    { 7, "Frage", 2 },
                    { 6, "Bug", 2 },
                    { 5, "Feature", 1 },
                    { 4, "Diskussion", 1 },
                    { 3, "Dokumentation", 1 },
                    { 2, "Frage", 1 },
                    { 1, "Bug", 1 },
                    { 10, "Feature", 2 },
                    { 9, "Diskussion", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CloseDate", "ClosedFromUserId", "CreateDate", "Deadline", "Description", "ProjectId", "StatusId", "TicketCreatorId", "Title", "Weight" },
                values: new object[,]
                {
                    { 10, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1856), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 10", 1 },
                    { 9, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1850), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 9", 1 },
                    { 8, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1844), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 8", 1 },
                    { 7, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1837), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 7", 1 },
                    { 6, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1831), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 6", 1 },
                    { 5, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1824), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 5", 0 },
                    { 4, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1818), null, "Ein vom System generiertes Ticket zum Testen.", 1, 4, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", "TestTicket 4", 2 },
                    { 3, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1809), null, "Ein vom System generiertes Ticket zum Testen.", 1, 2, "B22698B8-42A2-4115-9631-1C2D1E2AC5C5", "TestTicket 3", 3 },
                    { 2, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1752), null, "Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen. Ein vom System generiertes Ticket zum Testen.", 1, 3, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 2", 2 },
                    { 1, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 61, DateTimeKind.Local).AddTicks(9572), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket", 1 },
                    { 11, null, null, new DateTime(2021, 3, 14, 22, 25, 43, 62, DateTimeKind.Local).AddTicks(1862), null, "Ein vom System generiertes Ticket zum Testen.", 1, 1, "92d45cb7-746e-46d2-ad7b-ddc551eb1ef1", "TestTicket 11", 1 }
                });

            migrationBuilder.InsertData(
                table: "TicketProjectCategories",
                columns: new[] { "FK_TicketId", "FK_TicketCategoryId" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 3 },
                    { 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorId",
                table: "Comments",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TicketId",
                table: "Comments",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TicketId",
                table: "Images",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsfeedEntries_UserId",
                table: "NewsfeedEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntries_FK_ProjectRoleId",
                table: "ProjectMemberEntries",
                column: "FK_ProjectRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMemberEntries_FK_UserId",
                table: "ProjectMemberEntries",
                column: "FK_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatorId",
                table: "Projects",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_ProjectId",
                table: "TicketCategories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_EntryCaseId",
                table: "TicketHistoryEntries",
                column: "EntryCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_EntryCreatorId",
                table: "TicketHistoryEntries",
                column: "EntryCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_TicketId",
                table: "TicketHistoryEntries",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistoryEntries_UserId",
                table: "TicketHistoryEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketProjectCategories_FK_TicketCategoryId",
                table: "TicketProjectCategories",
                column: "FK_TicketCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ClosedFromUserId",
                table: "Tickets",
                column: "ClosedFromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectId",
                table: "Tickets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_StatusId",
                table: "Tickets",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketCreatorId",
                table: "Tickets",
                column: "TicketCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketWorkers_FK_UserId",
                table: "TicketWorkers",
                column: "FK_UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "NewsfeedEntries");

            migrationBuilder.DropTable(
                name: "ProjectMemberEntries");

            migrationBuilder.DropTable(
                name: "TicketHistoryEntries");

            migrationBuilder.DropTable(
                name: "TicketProjectCategories");

            migrationBuilder.DropTable(
                name: "TicketWorkers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProjectRoles");

            migrationBuilder.DropTable(
                name: "EntryCase");

            migrationBuilder.DropTable(
                name: "TicketCategories");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

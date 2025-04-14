﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlantsRPetsProjeto.Server.Data;

#nullable disable

namespace PlantsRPetsProjeto.Server.Migrations
{
    [DbContext(typeof(PlantsRPetsProjetoServerContext))]
    [Migration("20250412183831_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Collection", b =>
                {
                    b.Property<int>("CollectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CollectionId"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CollectionId");

                    b.HasIndex("UserId");

                    b.ToTable("Collection");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.CollectionPets", b =>
                {
                    b.Property<int>("CollectionPetsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CollectionPetsId"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOwned")
                        .HasColumnType("bit");

                    b.Property<int>("PetId")
                        .HasColumnType("int");

                    b.HasKey("CollectionPetsId");

                    b.HasIndex("CollectionId");

                    b.HasIndex("PetId");

                    b.ToTable("CollectionPets");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Dashboard", b =>
                {
                    b.Property<int>("DashboardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DashboardId"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DashboardId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Dashboard");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Metric", b =>
                {
                    b.Property<int>("MetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MetricId"));

                    b.Property<int?>("DashboardId")
                        .HasColumnType("int");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlantInfoId")
                        .HasColumnType("int");

                    b.Property<int>("PlantationId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MetricId");

                    b.HasIndex("DashboardId");

                    b.ToTable("Metric");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NotificationId"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotificationId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Pet", b =>
                {
                    b.Property<int>("PetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PetId"));

                    b.Property<string>("BattleStats")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PetId");

                    b.HasIndex("UserId");

                    b.ToTable("Pet");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantInfo", b =>
                {
                    b.Property<int>("PlantInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantInfoId"));

                    b.Property<string>("CareLevel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Cuisine")
                        .HasColumnType("bit");

                    b.Property<string>("Cycle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DroughtTolerant")
                        .HasColumnType("bit");

                    b.Property<string>("Edible")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FloweringSeason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Flowers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fruits")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GrowthRate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HarvestSeason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Indoor")
                        .HasColumnType("bit");

                    b.Property<bool>("Leaf")
                        .HasColumnType("bit");

                    b.Property<string>("Maintenance")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Medicinal")
                        .HasColumnType("bit");

                    b.Property<string>("PlantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlantType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PruningCountInfoId")
                        .HasColumnType("int");

                    b.PrimitiveCollection<string>("PruningMonth")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SaltTolerant")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("ScientificName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("Sunlight")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Watering")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlantInfoId");

                    b.HasIndex("PruningCountInfoId");

                    b.ToTable("PlantInfo");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantType", b =>
                {
                    b.Property<int>("PlantTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantTypeId"));

                    b.Property<bool>("HasRecurringHarvest")
                        .HasColumnType("bit");

                    b.Property<string>("PlantTypeName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlantTypeId");

                    b.ToTable("PlantType");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plantation", b =>
                {
                    b.Property<int>("PlantationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantationId"));

                    b.Property<int>("BankedLevelUps")
                        .HasColumnType("int");

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int?>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PlantTypeId")
                        .HasColumnType("int");

                    b.Property<string>("PlantationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PlantingDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PlantationId");

                    b.HasIndex("LocationId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PlantTypeId");

                    b.ToTable("Plantation");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantationPlants", b =>
                {
                    b.Property<int>("PlantationPlantsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantationPlantsId"));

                    b.Property<string>("GrowthStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("HarvestDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastHarvested")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastWatered")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlantInfoId")
                        .HasColumnType("int");

                    b.Property<int>("PlantationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PlantingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("PlantationPlantsId");

                    b.HasIndex("PlantInfoId");

                    b.HasIndex("PlantationId");

                    b.ToTable("PlantationPlants");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProfileId"));

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("FavoritePets")
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("HighlightedPlantations")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProfileId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profile");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PruningCountInfo", b =>
                {
                    b.Property<int>("PruningCountInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PruningCountInfoId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Interval")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PruningCountInfoId");

                    b.ToTable("PruningCountInfo");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("NotificationPreferences")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrivacyOptions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.SustainabilityTip", b =>
                {
                    b.Property<int>("SustainabilityTipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SustainabilityTipId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SustainabilityTipsListId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SustainabilityTipId");

                    b.HasIndex("SustainabilityTipsListId");

                    b.ToTable("SustainabilityTip");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.SustainabilityTipsList", b =>
                {
                    b.Property<int>("SustainabilityTipsListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SustainabilityTipsListId"));

                    b.Property<int>("PlantInfoId")
                        .HasColumnType("int");

                    b.Property<string>("PlantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.PrimitiveCollection<string>("PlantScientificName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SustainabilityTipsListId");

                    b.ToTable("SustainabilityTipsList");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Tutorial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tutorial");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("NotificationFrequency")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.UserNotification", b =>
                {
                    b.Property<int>("UserNotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserNotificationId"));

                    b.Property<int>("NotificationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("isRead")
                        .HasColumnType("bit");

                    b.HasKey("UserNotificationId");

                    b.HasIndex("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("UserNotifications");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Collection", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.CollectionPets", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Collection", "ReferenceCollection")
                        .WithMany("CollectionPets")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.Pet", "ReferencePet")
                        .WithMany()
                        .HasForeignKey("PetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReferenceCollection");

                    b.Navigation("ReferencePet");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Dashboard", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithOne("Dashboard")
                        .HasForeignKey("PlantsRPetsProjeto.Server.Models.Dashboard", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Metric", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Dashboard", null)
                        .WithMany("DashboardMetrics")
                        .HasForeignKey("DashboardId");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Pet", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany("Pets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantInfo", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.PruningCountInfo", "PruningCount")
                        .WithMany()
                        .HasForeignKey("PruningCountInfoId");

                    b.Navigation("PruningCount");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plantation", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", "User")
                        .WithMany("Plantations")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.PlantType", "PlantType")
                        .WithMany()
                        .HasForeignKey("PlantTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("PlantType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantationPlants", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.PlantInfo", "ReferencePlant")
                        .WithMany()
                        .HasForeignKey("PlantInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.Plantation", "ReferencePlantation")
                        .WithMany("PlantationPlants")
                        .HasForeignKey("PlantationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReferencePlant");

                    b.Navigation("ReferencePlantation");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Profile", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithOne("Profile")
                        .HasForeignKey("PlantsRPetsProjeto.Server.Models.Profile", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.SustainabilityTip", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.SustainabilityTipsList", null)
                        .WithMany("SustainabilityTip")
                        .HasForeignKey("SustainabilityTipsListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.UserNotification", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Notification", "Notification")
                        .WithMany()
                        .HasForeignKey("NotificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Notification");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Collection", b =>
                {
                    b.Navigation("CollectionPets");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Dashboard", b =>
                {
                    b.Navigation("DashboardMetrics");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plantation", b =>
                {
                    b.Navigation("PlantationPlants");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.SustainabilityTipsList", b =>
                {
                    b.Navigation("SustainabilityTip");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.User", b =>
                {
                    b.Navigation("Dashboard");

                    b.Navigation("Notifications");

                    b.Navigation("Pets");

                    b.Navigation("Plantations");

                    b.Navigation("Profile");
                });
#pragma warning restore 612, 618
        }
    }
}

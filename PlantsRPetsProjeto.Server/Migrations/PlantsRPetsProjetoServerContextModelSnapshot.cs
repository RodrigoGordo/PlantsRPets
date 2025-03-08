﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlantsRPetsProjeto.Server.Data;

#nullable disable

namespace PlantsRPetsProjeto.Server.Migrations
{
    [DbContext(typeof(PlantsRPetsProjetoServerContext))]
    partial class PlantsRPetsProjetoServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CommunityUser", b =>
                {
                    b.Property<int>("CommunitiesCommunityId")
                        .HasColumnType("int");

                    b.Property<string>("MembersId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CommunitiesCommunityId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("CommunityUser");
                });

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

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Chat", b =>
                {
                    b.Property<int>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChatId"));

                    b.Property<int>("CommunityId")
                        .HasColumnType("int");

                    b.HasKey("ChatId");

                    b.ToTable("Chat");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Collection", b =>
                {
                    b.Property<int>("CollectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CollectionId"));

                    b.HasKey("CollectionId");

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

                    b.Property<int>("PetId")
                        .HasColumnType("int");

                    b.HasKey("CollectionPetsId");

                    b.HasIndex("CollectionId");

                    b.HasIndex("PetId");

                    b.ToTable("CollectionPets");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Community", b =>
                {
                    b.Property<int>("CommunityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommunityId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommunityId");

                    b.ToTable("Community");
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

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("ChatId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Metric", b =>
                {
                    b.Property<int>("MetricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MetricId"));

                    b.Property<double>("CarbonFootprintReduction")
                        .HasColumnType("float");

                    b.Property<int?>("DashboardId")
                        .HasColumnType("int");

                    b.Property<int>("TotalPlants")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("WaterSaved")
                        .HasColumnType("float");

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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PetId");

                    b.HasIndex("ProfileId");

                    b.HasIndex("UserId");

                    b.ToTable("Pet");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plant", b =>
                {
                    b.Property<int>("PlantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantId"));

                    b.Property<int>("GrowthTime")
                        .HasColumnType("int");

                    b.Property<string>("PlantName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("WaterFrequency")
                        .HasColumnType("int");

                    b.Property<bool>("isGrown")
                        .HasColumnType("bit");

                    b.HasKey("PlantId");

                    b.ToTable("Plant");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plantation", b =>
                {
                    b.Property<int>("PlantationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantationId"));

                    b.Property<int>("ExperiencePoints")
                        .HasColumnType("int");

                    b.Property<string>("GrowthStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HarvestDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastWatered")
                        .HasColumnType("datetime2");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlantType")
                        .HasColumnType("int");

                    b.Property<string>("PlantationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PlantingDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ProfileId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PlantationId");

                    b.HasIndex("ProfileId");

                    b.HasIndex("UserId");

                    b.ToTable("Plantation");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantationPlants", b =>
                {
                    b.Property<int>("PlantationPlantsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlantationPlantsId"));

                    b.Property<int>("PlantId")
                        .HasColumnType("int");

                    b.Property<int>("PlantationId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("PlantationPlantsId");

                    b.HasIndex("PlantId");

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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ProfileId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Profile");
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

                    b.Property<int?>("SustainabilityTipsListId")
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

            modelBuilder.Entity("CommunityUser", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Community", null)
                        .WithMany()
                        .HasForeignKey("CommunitiesCommunityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Message", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Chat", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
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
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Profile", null)
                        .WithMany("FavoritePets")
                        .HasForeignKey("ProfileId");

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany("Pets")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Plantation", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Profile", null)
                        .WithMany("HighlightedPlantations")
                        .HasForeignKey("ProfileId");

                    b.HasOne("PlantsRPetsProjeto.Server.Models.User", null)
                        .WithMany("Plantations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.PlantationPlants", b =>
                {
                    b.HasOne("PlantsRPetsProjeto.Server.Models.Plant", "ReferencePlant")
                        .WithMany()
                        .HasForeignKey("PlantId")
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
                        .HasForeignKey("SustainabilityTipsListId");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Chat", b =>
                {
                    b.Navigation("Messages");
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

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.Profile", b =>
                {
                    b.Navigation("FavoritePets");

                    b.Navigation("HighlightedPlantations");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.SustainabilityTipsList", b =>
                {
                    b.Navigation("SustainabilityTip");
                });

            modelBuilder.Entity("PlantsRPetsProjeto.Server.Models.User", b =>
                {
                    b.Navigation("Dashboard");

                    b.Navigation("Pets");

                    b.Navigation("Plantations");

                    b.Navigation("Profile");
                });
#pragma warning restore 612, 618
        }
    }
}

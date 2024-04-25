﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportsPro.DataAccess;

#nullable disable

namespace SportsPro.Migrations
{
    [DbContext(typeof(SportsProContext))]
    partial class SportsProContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
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

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
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

            modelBuilder.Entity("SportsPro.Models.Country", b =>
                {
                    b.Property<string>("CountryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryID");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            CountryID = "AU",
                            Name = "Australia"
                        },
                        new
                        {
                            CountryID = "AT",
                            Name = "Austria"
                        },
                        new
                        {
                            CountryID = "BE",
                            Name = "Belgium"
                        },
                        new
                        {
                            CountryID = "BR",
                            Name = "Brazil"
                        },
                        new
                        {
                            CountryID = "CA",
                            Name = "Canada"
                        },
                        new
                        {
                            CountryID = "CN",
                            Name = "China"
                        },
                        new
                        {
                            CountryID = "DK",
                            Name = "Denmark"
                        },
                        new
                        {
                            CountryID = "FI",
                            Name = "Finland"
                        },
                        new
                        {
                            CountryID = "FR",
                            Name = "France"
                        },
                        new
                        {
                            CountryID = "GR",
                            Name = "Greece"
                        },
                        new
                        {
                            CountryID = "GL",
                            Name = "Greenland"
                        },
                        new
                        {
                            CountryID = "HK",
                            Name = "Hong Kong"
                        },
                        new
                        {
                            CountryID = "IS",
                            Name = "Iceland"
                        },
                        new
                        {
                            CountryID = "IN",
                            Name = "India"
                        },
                        new
                        {
                            CountryID = "IE",
                            Name = "Ireland"
                        },
                        new
                        {
                            CountryID = "IL",
                            Name = "Israel"
                        },
                        new
                        {
                            CountryID = "IT",
                            Name = "Italy"
                        },
                        new
                        {
                            CountryID = "JP",
                            Name = "Japan"
                        },
                        new
                        {
                            CountryID = "LR",
                            Name = "Liberia"
                        },
                        new
                        {
                            CountryID = "MY",
                            Name = "Malaysia"
                        },
                        new
                        {
                            CountryID = "MX",
                            Name = "Mexico"
                        },
                        new
                        {
                            CountryID = "NL",
                            Name = "Netherlands"
                        },
                        new
                        {
                            CountryID = "NZ",
                            Name = "New Zealand"
                        },
                        new
                        {
                            CountryID = "NG",
                            Name = "Nigeria"
                        },
                        new
                        {
                            CountryID = "PH",
                            Name = "Philippines"
                        },
                        new
                        {
                            CountryID = "PT",
                            Name = "Portugal"
                        },
                        new
                        {
                            CountryID = "PR",
                            Name = "Puerto Rico"
                        },
                        new
                        {
                            CountryID = "QA",
                            Name = "Qatar"
                        },
                        new
                        {
                            CountryID = "SG",
                            Name = "Singapore"
                        },
                        new
                        {
                            CountryID = "ES",
                            Name = "Spain"
                        },
                        new
                        {
                            CountryID = "SE",
                            Name = "Sweden"
                        },
                        new
                        {
                            CountryID = "CH",
                            Name = "Switzerland"
                        },
                        new
                        {
                            CountryID = "TH",
                            Name = "Thailand"
                        },
                        new
                        {
                            CountryID = "TR",
                            Name = "Turkey"
                        },
                        new
                        {
                            CountryID = "UA",
                            Name = "Ukraine"
                        },
                        new
                        {
                            CountryID = "AE",
                            Name = "United Arab Emirates"
                        },
                        new
                        {
                            CountryID = "GB",
                            Name = "United Kingdom"
                        },
                        new
                        {
                            CountryID = "US",
                            Name = "United States"
                        },
                        new
                        {
                            CountryID = "VN",
                            Name = "Vietnam"
                        },
                        new
                        {
                            CountryID = "ZW",
                            Name = "Zimbabwe"
                        });
                });

            modelBuilder.Entity("SportsPro.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CountryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CustomerID");

                    b.HasIndex("CountryID");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerID = 1002,
                            Address = "PO Box 96621",
                            City = "Washington",
                            CountryID = "US",
                            Email = "ania@mma.nidc.com",
                            FirstName = "Ania",
                            LastName = "Irvin",
                            Phone = "(301) 555-8950",
                            PostalCode = "20090",
                            State = "DC"
                        },
                        new
                        {
                            CustomerID = 1004,
                            Address = "1990 Westwood Blvd Ste 260",
                            City = "Los Angeles",
                            CountryID = "US",
                            Email = "kenzie@mma.jobtrak.com",
                            FirstName = "Kenzie",
                            LastName = "Quinn",
                            Phone = "(800) 555-8725",
                            PostalCode = "90025",
                            State = "CA"
                        },
                        new
                        {
                            CustomerID = 1006,
                            Address = "3255 Ramos Cir",
                            City = "Sacramento",
                            CountryID = "US",
                            Email = "amauro@yahoo.org",
                            FirstName = "Anton",
                            LastName = "Mauro",
                            Phone = "(916) 555-6670",
                            PostalCode = "95827",
                            State = "CA"
                        },
                        new
                        {
                            CustomerID = 1008,
                            Address = "Box 52001",
                            City = "San Francisco",
                            CountryID = "US",
                            Email = "kanthoni@pge.com",
                            FirstName = "Kaitlyn",
                            LastName = "Anthoni",
                            Phone = "(800) 555-6081",
                            PostalCode = "94152",
                            State = "CA"
                        },
                        new
                        {
                            CustomerID = 1010,
                            Address = "PO Box 2069",
                            City = "Fresno",
                            CountryID = "US",
                            Email = "kmayte@fresno.ca.gov",
                            FirstName = "Kendall",
                            LastName = "Mayte",
                            Phone = "(559) 555-9999",
                            PostalCode = "93718",
                            State = "CA"
                        },
                        new
                        {
                            CustomerID = 1012,
                            Address = "4420 N. First Street, Suite 108",
                            City = "Fresno",
                            CountryID = "US",
                            Email = "marvin@expedata.com",
                            FirstName = "Marvin",
                            LastName = "Quintin",
                            Phone = "(559) 555-9586",
                            PostalCode = "93726",
                            State = "CA"
                        },
                        new
                        {
                            CustomerID = 1015,
                            Address = "27371 Valderas",
                            City = "Mission Viejo",
                            CountryID = "US",
                            Email = "",
                            FirstName = "Gonzalo",
                            LastName = "Keeton",
                            Phone = "(214) 555-3647",
                            PostalCode = "92691",
                            State = "CA"
                        });
                });

            modelBuilder.Entity("SportsPro.Models.Incident", b =>
                {
                    b.Property<int>("IncidentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IncidentID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int?>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IncidentID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.HasIndex("Id");

                    b.ToTable("Incidents");

                    b.HasData(
                        new
                        {
                            IncidentID = 1,
                            CustomerID = 1010,
                            DateClosed = new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOpened = new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Media appears to be bad.",
                            ProductID = 1,
                            TechnicianID = 11,
                            Title = "Could not install"
                        },
                        new
                        {
                            IncidentID = 2,
                            CustomerID = 1002,
                            DateOpened = new DateTime(2020, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Received error message 415 while trying to import data from previous version.",
                            ProductID = 4,
                            TechnicianID = 14,
                            Title = "Error importing data"
                        },
                        new
                        {
                            IncidentID = 3,
                            CustomerID = 1015,
                            DateClosed = new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOpened = new DateTime(2020, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Setup failed with code 104.",
                            ProductID = 6,
                            TechnicianID = 15,
                            Title = "Could not install"
                        },
                        new
                        {
                            IncidentID = 4,
                            CustomerID = 1010,
                            DateOpened = new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Program fails with error code 510, unable to open database.",
                            ProductID = 3,
                            Title = "Error launching program"
                        });
                });

            modelBuilder.Entity("SportsPro.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("YearlyPrice")
                        .HasColumnType("decimal(8,2)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = 1,
                            Name = "Draft Manager 1.0",
                            ProductCode = "DRAFT10",
                            ReleaseDate = new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 4.99m
                        },
                        new
                        {
                            ProductID = 2,
                            Name = "Draft Manager 2.0",
                            ProductCode = "DRAFT20",
                            ReleaseDate = new DateTime(2019, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 5.99m
                        },
                        new
                        {
                            ProductID = 3,
                            Name = "League Scheduler 1.0",
                            ProductCode = "LEAG10",
                            ReleaseDate = new DateTime(2016, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 4.99m
                        },
                        new
                        {
                            ProductID = 4,
                            Name = "League Scheduler Deluxe 1.0",
                            ProductCode = "LEAGD10",
                            ReleaseDate = new DateTime(2016, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 7.99m
                        },
                        new
                        {
                            ProductID = 5,
                            Name = "Team Manager 1.0",
                            ProductCode = "TEAM10",
                            ReleaseDate = new DateTime(2017, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 4.99m
                        },
                        new
                        {
                            ProductID = 6,
                            Name = "Tournament Master 1.0",
                            ProductCode = "TRNY10",
                            ReleaseDate = new DateTime(2015, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 4.99m
                        },
                        new
                        {
                            ProductID = 7,
                            Name = "Tournament Master 2.0",
                            ProductCode = "TRNY20",
                            ReleaseDate = new DateTime(2018, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            YearlyPrice = 5.99m
                        });
                });

            modelBuilder.Entity("SportsPro.Models.Registration", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegistrationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegistrationID"));

                    b.HasKey("CustomerID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("Registrations");

                    b.HasData(
                        new
                        {
                            CustomerID = 1002,
                            ProductID = 1,
                            RegistrationDate = new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RegistrationID = 1
                        },
                        new
                        {
                            CustomerID = 1002,
                            ProductID = 3,
                            RegistrationDate = new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RegistrationID = 2
                        },
                        new
                        {
                            CustomerID = 1010,
                            ProductID = 2,
                            RegistrationDate = new DateTime(2017, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RegistrationID = 3
                        });
                });

            modelBuilder.Entity("SportsPro.Models.SportsProUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Technicians");

                    b.HasData(
                        new
                        {
                            TechnicianID = 11,
                            Email = "alison@sportsprosoftware.com",
                            FirstName = "Alison",
                            LastName = "Diaz",
                            Phone = "800-555-0443"
                        },
                        new
                        {
                            TechnicianID = 12,
                            Email = "jason@sportsprosoftware.com",
                            FirstName = "Jason",
                            LastName = "Lee",
                            Phone = "800-555-0444"
                        },
                        new
                        {
                            TechnicianID = 13,
                            Email = "awilson@sportsprosoftware.com",
                            FirstName = "Andrew",
                            LastName = "Wilson",
                            Phone = "800-555-0449"
                        },
                        new
                        {
                            TechnicianID = 14,
                            Email = "gunter@sportsprosoftware.com",
                            FirstName = "Gunter",
                            LastName = "Wendt",
                            Phone = "800-555-0400"
                        },
                        new
                        {
                            TechnicianID = 15,
                            Email = "gfiori@sportsprosoftware.com",
                            FirstName = "Gina",
                            LastName = "Fiori",
                            Phone = "800-555-0459"
                        });
                });

            modelBuilder.Entity("SportsPro.Models.SportsProUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("SportsProUser");
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
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
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

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SportsPro.Models.Customer", b =>
                {
                    b.HasOne("SportsPro.Models.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("SportsPro.Models.Incident", b =>
                {
                    b.HasOne("SportsPro.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SportsPro.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SportsPro.Models.SportsProUser", "SportsProUser")
                        .WithMany()
                        .HasForeignKey("Id");

                    b.Navigation("Customer");

                    b.Navigation("Product");

                    b.Navigation("SportsProUser");
                });

            modelBuilder.Entity("SportsPro.Models.Registration", b =>
                {
                    b.HasOne("SportsPro.Models.Customer", "Customer")
                        .WithMany("Registrations")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SportsPro.Models.Product", "Product")
                        .WithMany("Registrations")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SportsPro.Models.Customer", b =>
                {
                    b.Navigation("Registrations");
                });

            modelBuilder.Entity("SportsPro.Models.Product", b =>
                {
                    b.Navigation("Registrations");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PropertyAds.WebApp.Data;

namespace PropertyAds.WebApp.Data.Migrations
{
    [DbContext(typeof(PropertyAdsDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Conversation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RecipientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PropertyId");

                    b.HasIndex("RecipientId");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.District", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ConversationId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ConversationId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Property", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Area")
                        .HasColumnType("decimal(6,2)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("DistrictId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("TotalFloors")
                        .HasColumnType("int");

                    b.Property<string>("TypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("UsableArea")
                        .HasColumnType("decimal(6,2)");

                    b.Property<int>("VisitedCount")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("TypeId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.PropertyAggregate", b =>
                {
                    b.Property<string>("DistrictId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AveragePrice")
                        .HasColumnType("int");

                    b.Property<int>("AveragePricePerSqM")
                        .HasColumnType("int");

                    b.HasKey("DistrictId", "PropertyTypeId");

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("PropertyAggregates");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.PropertyImage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("Bytes")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PropertyId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyImages");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.PropertyType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int>("SortRank")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PropertyTypes");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.User", b =>
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

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Watchlist", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("LastViewedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OwnerId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Watchlists");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.WatchlistProperty", b =>
                {
                    b.Property<string>("WatchlistId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WatchlistId", "PropertyId");

                    b.HasIndex("PropertyId");

                    b.ToTable("WatchlistProperties");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.WatchlistPropertySegment", b =>
                {
                    b.Property<string>("WatchlistId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PropertyTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DistrictId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("WatchlistId", "PropertyTypeId", "DistrictId");

                    b.HasIndex("DistrictId");

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("WatchlistPropertySegments");
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
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", null)
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

                    b.HasOne("PropertyAds.WebApp.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Conversation", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", "Owner")
                        .WithMany("OwnConversations")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.Property", "Property")
                        .WithMany("Conversations")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.User", "Recipient")
                        .WithMany("RecipientConversations")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Property");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Message", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.Conversation", "Conversation")
                        .WithMany("Messages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Property", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");

                    b.HasOne("PropertyAds.WebApp.Data.Models.User", "Owner")
                        .WithMany("Properties")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.PropertyType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");

                    b.Navigation("District");

                    b.Navigation("Owner");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.PropertyAggregate", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");

                    b.Navigation("PropertyType");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.PropertyImage", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.Property", "Property")
                        .WithMany("Images")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Watchlist", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.WatchlistProperty", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.Watchlist", "Watchlist")
                        .WithMany("WatchlistProperties")
                        .HasForeignKey("WatchlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");

                    b.Navigation("Watchlist");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.WatchlistPropertySegment", b =>
                {
                    b.HasOne("PropertyAds.WebApp.Data.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.PropertyType", "PropertyType")
                        .WithMany()
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PropertyAds.WebApp.Data.Models.Watchlist", "Watchlist")
                        .WithMany("WatchlistPropertySegments")
                        .HasForeignKey("WatchlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");

                    b.Navigation("PropertyType");

                    b.Navigation("Watchlist");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Conversation", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Property", b =>
                {
                    b.Navigation("Conversations");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.User", b =>
                {
                    b.Navigation("OwnConversations");

                    b.Navigation("Properties");

                    b.Navigation("RecipientConversations");
                });

            modelBuilder.Entity("PropertyAds.WebApp.Data.Models.Watchlist", b =>
                {
                    b.Navigation("WatchlistProperties");

                    b.Navigation("WatchlistPropertySegments");
                });
#pragma warning restore 612, 618
        }
    }
}

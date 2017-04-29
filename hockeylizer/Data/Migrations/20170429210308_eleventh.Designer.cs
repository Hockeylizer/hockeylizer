using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using hockeylizer.Data;

namespace hockeylizer.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170429210308_eleventh")]
    partial class eleventh
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("hockeylizer.Models.AnalysisResult", b =>
                {
                    b.Property<int>("AnalysisId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HitGoal");

                    b.Property<int?>("VideoId");

                    b.HasKey("AnalysisId");

                    b.HasIndex("VideoId");

                    b.ToTable("AnalysisResults");
                });

            modelBuilder.Entity("hockeylizer.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("hockeylizer.Models.AppTeam", b =>
                {
                    b.Property<Guid>("TeamId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("TeamId");

                    b.ToTable("AppTeams");
                });

            modelBuilder.Entity("hockeylizer.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TeamId");

                    b.Property<Guid>("containerId");

                    b.HasKey("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("hockeylizer.Models.PlayerVideo", b =>
                {
                    b.Property<int>("VideoId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Deleted");

                    b.Property<string>("FileName");

                    b.Property<int>("Interval");

                    b.Property<int>("NumberOfTargets");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Rounds");

                    b.Property<int>("Shots");

                    b.Property<string>("VideoPath");

                    b.Property<bool>("VideoRemoved");

                    b.HasKey("VideoId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("hockeylizer.Models.ShotTimestamp", b =>
                {
                    b.Property<int>("TimestampId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("VideoId");

                    b.Property<long>("end");

                    b.Property<long>("start");

                    b.HasKey("TimestampId");

                    b.HasIndex("VideoId");

                    b.ToTable("Timestamps");
                });

            modelBuilder.Entity("hockeylizer.Models.Target", b =>
                {
                    b.Property<int>("TargetId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Order");

                    b.Property<int>("TargetNumber");

                    b.Property<int>("VideoId");

                    b.HasKey("TargetId");

                    b.HasIndex("VideoId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("hockeylizer.Models.TargetCoord", b =>
                {
                    b.Property<int>("CoordinateId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("VideoId");

                    b.Property<int?>("xCoord");

                    b.Property<int?>("yCoord");

                    b.HasKey("CoordinateId");

                    b.HasIndex("VideoId");

                    b.ToTable("TargetCoordinates");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("hockeylizer.Models.AnalysisResult", b =>
                {
                    b.HasOne("hockeylizer.Models.PlayerVideo", "Video")
                        .WithMany("AnalysisResults")
                        .HasForeignKey("VideoId");
                });

            modelBuilder.Entity("hockeylizer.Models.Player", b =>
                {
                    b.HasOne("hockeylizer.Models.AppTeam", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("hockeylizer.Models.PlayerVideo", b =>
                {
                    b.HasOne("hockeylizer.Models.Player", "Player")
                        .WithMany("Videos")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("hockeylizer.Models.ShotTimestamp", b =>
                {
                    b.HasOne("hockeylizer.Models.PlayerVideo", "Video")
                        .WithMany("Timestamps")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("hockeylizer.Models.Target", b =>
                {
                    b.HasOne("hockeylizer.Models.PlayerVideo", "RelatedVideo")
                        .WithMany("Targets")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("hockeylizer.Models.TargetCoord", b =>
                {
                    b.HasOne("hockeylizer.Models.PlayerVideo", "Video")
                        .WithMany("TargetCoords")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("hockeylizer.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("hockeylizer.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("hockeylizer.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

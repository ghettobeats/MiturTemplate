﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiturNetInfrastructure.DBContext;

#nullable disable

namespace MiturNETInfrastructure.Migrations
{
    [DbContext(typeof(MiturNetContext))]
    [Migration("20240923134845_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetRoleClaims", b =>
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

                    b.ToTable("AspNetRoleClaims", t =>
                        {
                            t.HasTrigger("AspNetRoleClaims_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IndexPage")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)");

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
                        .HasFilter("([NormalizedName] IS NOT NULL)");

                    b.ToTable("AspNetRoles", t =>
                        {
                            t.HasTrigger("AspNetRoles_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserClaims", b =>
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

                    b.ToTable("AspNetUserClaims", t =>
                        {
                            t.HasTrigger("AspNetUserClaims_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserLogins", b =>
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

                    b.ToTable("AspNetUserLogins", t =>
                        {
                            t.HasTrigger("AspNetUserLogins_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", t =>
                        {
                            t.HasTrigger("AspNetUserRoles_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserTokens", b =>
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

                    b.ToTable("AspNetUserTokens", t =>
                        {
                            t.HasTrigger("AspNetUserTokens_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsers", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

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
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers", t =>
                        {
                            t.HasTrigger("AspNetUsers_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersLoginHistory", b =>
                {
                    b.Property<string>("VUlhid")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)")
                        .HasColumnName("vULHID");

                    b.Property<DateTime>("DLogIn")
                        .HasColumnType("datetime")
                        .HasColumnName("dLogIn");

                    b.Property<DateTime?>("DLogOut")
                        .HasColumnType("datetime")
                        .HasColumnName("dLogOut");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NvIpaddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvIPAddress");

                    b.HasKey("VUlhid");

                    b.HasIndex("Id");

                    b.ToTable("AspNetUsersLoginHistory", t =>
                        {
                            t.HasTrigger("AspNetUsersLoginHistory_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersMenu", b =>
                {
                    b.Property<string>("VMenuId")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("vMenuID");

                    b.Property<int>("ISerialNo")
                        .HasColumnType("int")
                        .HasColumnName("iSerialNo");

                    b.Property<string>("NvFabIcon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvFabIcon");

                    b.Property<string>("NvMenuName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvMenuName");

                    b.Property<string>("NvPageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvPageUrl");

                    b.Property<string>("VParentMenuId")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("vParentMenuID");

                    b.HasKey("VMenuId");

                    b.HasIndex("VParentMenuId");

                    b.ToTable("AspNetUsersMenus", t =>
                        {
                            t.HasTrigger("AspNetUsersMenus_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersMenuPermission", b =>
                {
                    b.Property<string>("VMenuPermissionId")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vMenuPermissionID");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VMenuId")
                        .IsRequired()
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("vMenuID");

                    b.HasKey("VMenuPermissionId");

                    b.HasIndex("Id");

                    b.HasIndex("VMenuId", "Id")
                        .IsUnique()
                        .HasDatabaseName("IX_AspNetUsersMenuPermission");

                    b.ToTable("AspNetUsersMenuPermission", t =>
                        {
                            t.HasTrigger("AspNetUsersMenuPermission_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersPageVisited", b =>
                {
                    b.Property<string>("VPageVisitedId")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("vPageVisitedID");

                    b.Property<DateTime>("DDateVisited")
                        .HasColumnType("datetime")
                        .HasColumnName("dDateVisited");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NvIpaddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvIPAddress");

                    b.Property<string>("NvPageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("nvPageName");

                    b.HasKey("VPageVisitedId");

                    b.HasIndex("Id");

                    b.ToTable("AspNetUsersPageVisited", t =>
                        {
                            t.HasTrigger("AspNetUsersPageVisited_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersProfile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("VCountry")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("vCountry");

                    b.Property<string>("VFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vFirstName");

                    b.Property<string>("VGender")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("vGender");

                    b.Property<string>("VLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("vLastName");

                    b.Property<string>("VPhoto")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("vPhoto");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsersProfile", t =>
                        {
                            t.HasTrigger("AspNetUsersProfile_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.Setting", b =>
                {
                    b.Property<string>("VSettingId")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("vSettingID");

                    b.Property<string>("VSettingName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("vSettingName");

                    b.Property<string>("VSettingOption")
                        .HasMaxLength(250)
                        .IsUnicode(false)
                        .HasColumnType("varchar(250)")
                        .HasColumnName("vSettingOption");

                    b.HasKey("VSettingId");

                    b.ToTable("Setting", t =>
                        {
                            t.HasTrigger("Setting_Trigger");
                        });
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetRoleClaims", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserClaims", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "User")
                        .WithMany("AspNetUserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserLogins", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "User")
                        .WithMany("AspNetUserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserRoles", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "User")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUserTokens", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersLoginHistory", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "IdNavigation")
                        .WithMany("AspNetUsersLoginHistory")
                        .HasForeignKey("Id")
                        .IsRequired();

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersMenu", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsersMenu", "VParentMenu")
                        .WithMany("InverseVParentMenu")
                        .HasForeignKey("VParentMenuId")
                        .HasConstraintName("FK_AspNetUsersMenu_AspNetUsersMenu");

                    b.Navigation("VParentMenu");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersMenuPermission", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetRoles", "IdNavigation")
                        .WithMany("AspNetUsersMenuPermission")
                        .HasForeignKey("Id")
                        .IsRequired()
                        .HasConstraintName("FK_AspNetUsersMenuPermission_AspNetRoles");

                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsersMenu", "VMenu")
                        .WithMany("AspNetUsersMenuPermission")
                        .HasForeignKey("VMenuId")
                        .IsRequired()
                        .HasConstraintName("FK_AspNetUsersMenuPermission_AspNetUsersMenu");

                    b.Navigation("IdNavigation");

                    b.Navigation("VMenu");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersPageVisited", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "IdNavigation")
                        .WithMany("AspNetUsersPageVisited")
                        .HasForeignKey("Id")
                        .IsRequired()
                        .HasConstraintName("FK_AspNetUsersPageVisited_AspNetUsers");

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersProfile", b =>
                {
                    b.HasOne("MiturNetDomain.Entities.UserManagement.AspNetUsers", "IdNavigation")
                        .WithOne("AspNetUsersProfile")
                        .HasForeignKey("MiturNetDomain.Entities.UserManagement.AspNetUsersProfile", "Id")
                        .IsRequired()
                        .HasConstraintName("FK_AspNetUsersProfile_AspNetUsers");

                    b.Navigation("IdNavigation");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetRoles", b =>
                {
                    b.Navigation("AspNetRoleClaims");

                    b.Navigation("AspNetUserRoles");

                    b.Navigation("AspNetUsersMenuPermission");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsers", b =>
                {
                    b.Navigation("AspNetUserClaims");

                    b.Navigation("AspNetUserLogins");

                    b.Navigation("AspNetUserRoles");

                    b.Navigation("AspNetUserTokens");

                    b.Navigation("AspNetUsersLoginHistory");

                    b.Navigation("AspNetUsersPageVisited");

                    b.Navigation("AspNetUsersProfile");
                });

            modelBuilder.Entity("MiturNetDomain.Entities.UserManagement.AspNetUsersMenu", b =>
                {
                    b.Navigation("AspNetUsersMenuPermission");

                    b.Navigation("InverseVParentMenu");
                });
#pragma warning restore 612, 618
        }
    }
}

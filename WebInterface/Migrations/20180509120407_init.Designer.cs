﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WebInterface.Models;

namespace WebInterface.Migrations
{
    [DbContext(typeof(MenuCardDBContext))]
    [Migration("20180509120407_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
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
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WebInterface.Models.ApplicationUser", b =>
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
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WebInterface.Models.Bill", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("PayerGuestID");

                    b.HasKey("BillID");

                    b.HasIndex("PayerGuestID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("WebInterface.Models.Dish", b =>
                {
                    b.Property<int>("DishID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CourseDishTypeID");

                    b.Property<int?>("OrderID");

                    b.Property<int?>("OrderID1");

                    b.HasKey("DishID");

                    b.HasIndex("CourseDishTypeID");

                    b.HasIndex("OrderID");

                    b.HasIndex("OrderID1");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("WebInterface.Models.DishType", b =>
                {
                    b.Property<int>("DishTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Course");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double?>("Price")
                        .IsRequired();

                    b.Property<string>("Recipe")
                        .IsRequired();

                    b.Property<int?>("SubDishTypeID");

                    b.HasKey("DishTypeID");

                    b.HasIndex("SubDishTypeID");

                    b.ToTable("DishTypes");
                });

            modelBuilder.Entity("WebInterface.Models.Group", b =>
                {
                    b.Property<int>("GroupID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HidePrice");

                    b.Property<int?>("TableID");

                    b.HasKey("GroupID");

                    b.HasIndex("TableID")
                        .IsUnique()
                        .HasFilter("[TableID] IS NOT NULL");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("WebInterface.Models.Guest", b =>
                {
                    b.Property<int>("GuestID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .HasMaxLength(128);

                    b.Property<int?>("GroupID");

                    b.Property<string>("Name");

                    b.HasKey("GuestID");

                    b.HasIndex("GroupID");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("WebInterface.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DishID");

                    b.Property<int?>("DishTypeID");

                    b.Property<double>("Quantity");

                    b.Property<int?>("TypeIngredientTypeID");

                    b.HasKey("IngredientID");

                    b.HasIndex("DishID");

                    b.HasIndex("DishTypeID");

                    b.HasIndex("TypeIngredientTypeID");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("WebInterface.Models.IngredientType", b =>
                {
                    b.Property<int>("IngredientTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("UnitPlural");

                    b.Property<string>("UnitSingular");

                    b.HasKey("IngredientTypeID");

                    b.ToTable("IngredientTypes");
                });

            modelBuilder.Entity("WebInterface.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("OwnerGuestID");

                    b.HasKey("OrderID");

                    b.HasIndex("OwnerGuestID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebInterface.Models.SubDishType", b =>
                {
                    b.Property<int>("SubDishTypeID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("SubType");

                    b.HasKey("SubDishTypeID");

                    b.ToTable("SubDishTypes");
                });

            modelBuilder.Entity("WebInterface.Models.Table", b =>
                {
                    b.Property<int>("TableID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Capacity");

                    b.HasKey("TableID");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebInterface.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebInterface.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebInterface.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebInterface.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebInterface.Models.Bill", b =>
                {
                    b.HasOne("WebInterface.Models.Guest", "Payer")
                        .WithMany()
                        .HasForeignKey("PayerGuestID");
                });

            modelBuilder.Entity("WebInterface.Models.Dish", b =>
                {
                    b.HasOne("WebInterface.Models.DishType", "Course")
                        .WithMany()
                        .HasForeignKey("CourseDishTypeID");

                    b.HasOne("WebInterface.Models.Order")
                        .WithMany("Finalized")
                        .HasForeignKey("OrderID");

                    b.HasOne("WebInterface.Models.Order")
                        .WithMany("Selected")
                        .HasForeignKey("OrderID1");
                });

            modelBuilder.Entity("WebInterface.Models.DishType", b =>
                {
                    b.HasOne("WebInterface.Models.SubDishType", "SubDishType")
                        .WithMany()
                        .HasForeignKey("SubDishTypeID");
                });

            modelBuilder.Entity("WebInterface.Models.Group", b =>
                {
                    b.HasOne("WebInterface.Models.Table", "Table")
                        .WithOne("CurrentGroup")
                        .HasForeignKey("WebInterface.Models.Group", "TableID");
                });

            modelBuilder.Entity("WebInterface.Models.Guest", b =>
                {
                    b.HasOne("WebInterface.Models.Group", "Group")
                        .WithMany("CurrentGuests")
                        .HasForeignKey("GroupID");
                });

            modelBuilder.Entity("WebInterface.Models.Ingredient", b =>
                {
                    b.HasOne("WebInterface.Models.Dish")
                        .WithMany("Ingredients")
                        .HasForeignKey("DishID");

                    b.HasOne("WebInterface.Models.DishType")
                        .WithMany("DefaultIngredients")
                        .HasForeignKey("DishTypeID");

                    b.HasOne("WebInterface.Models.IngredientType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeIngredientTypeID");
                });

            modelBuilder.Entity("WebInterface.Models.Order", b =>
                {
                    b.HasOne("WebInterface.Models.Guest", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerGuestID");
                });
#pragma warning restore 612, 618
        }
    }
}

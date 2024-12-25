﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using real_estate_api.Data;

#nullable disable

namespace real_estate_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241225164754_Add_Chat_Message_Model")]
    partial class Add_Chat_Message_Model
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("real_estate_api.Models.Chat", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastMessage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("real_estate_api.Models.ChatSeenBy", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("SeenAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatSeenBy");
                });

            modelBuilder.Entity("real_estate_api.Models.ChatUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatUsers");
                });

            modelBuilder.Entity("real_estate_api.Models.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("real_estate_api.Models.Post", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Bathroom")
                        .HasColumnType("int");

                    b.Property<int>("Bedroom")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Images")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Property")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("real_estate_api.Models.PostDetail", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncomeRequirement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NearbyBusStops")
                        .HasColumnType("int");

                    b.Property<int?>("NearbyRestaurants")
                        .HasColumnType("int");

                    b.Property<int?>("NearbySchools")
                        .HasColumnType("int");

                    b.Property<string>("PetPolicy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Size")
                        .HasColumnType("int");

                    b.Property<string>("Utilities")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .IsUnique();

                    b.ToTable("PostDetails");
                });

            modelBuilder.Entity("real_estate_api.Models.SavedPost", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId", "PostId")
                        .IsUnique();

                    b.ToTable("SavedPosts");
                });

            modelBuilder.Entity("real_estate_api.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("real_estate_api.Models.ChatSeenBy", b =>
                {
                    b.HasOne("real_estate_api.Models.Chat", "Chat")
                        .WithMany("SeenByUsers")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("real_estate_api.Models.User", "User")
                        .WithMany("ChatSeenByUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("real_estate_api.Models.ChatUser", b =>
                {
                    b.HasOne("real_estate_api.Models.Chat", "Chat")
                        .WithMany("ChatUsers")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("real_estate_api.Models.User", "User")
                        .WithMany("ChatUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("real_estate_api.Models.Message", b =>
                {
                    b.HasOne("real_estate_api.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("real_estate_api.Models.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("real_estate_api.Models.Post", b =>
                {
                    b.HasOne("real_estate_api.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("real_estate_api.Models.PostDetail", b =>
                {
                    b.HasOne("real_estate_api.Models.Post", "Post")
                        .WithOne("PostDetail")
                        .HasForeignKey("real_estate_api.Models.PostDetail", "PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");
                });

            modelBuilder.Entity("real_estate_api.Models.SavedPost", b =>
                {
                    b.HasOne("real_estate_api.Models.Post", "Post")
                        .WithMany("SavedPosts")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("real_estate_api.Models.User", "User")
                        .WithMany("SavedPosts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("real_estate_api.Models.Chat", b =>
                {
                    b.Navigation("ChatUsers");

                    b.Navigation("Messages");

                    b.Navigation("SeenByUsers");
                });

            modelBuilder.Entity("real_estate_api.Models.Post", b =>
                {
                    b.Navigation("PostDetail");

                    b.Navigation("SavedPosts");
                });

            modelBuilder.Entity("real_estate_api.Models.User", b =>
                {
                    b.Navigation("ChatSeenByUsers");

                    b.Navigation("ChatUsers");

                    b.Navigation("Messages");

                    b.Navigation("Posts");

                    b.Navigation("SavedPosts");
                });
#pragma warning restore 612, 618
        }
    }
}

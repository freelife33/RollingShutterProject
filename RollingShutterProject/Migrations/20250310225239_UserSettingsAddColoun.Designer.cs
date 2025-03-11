﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RollingShutterProject.Data;

#nullable disable

namespace RollingShutterProject.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250310225239_UserSettingsAddColoun")]
    partial class UserSettingsAddColoun
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RollingShutterProject.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("RollingShutterProject.Models.SensorData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceIdString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "deviceId");

                    b.Property<string>("SensorType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "sensorType");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<float>("Value")
                        .HasColumnType("real")
                        .HasAnnotation("Relational:JsonPropertyName", "value");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("SensorData");
                });

            modelBuilder.Entity("RollingShutterProject.Models.SystemSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsConfigured")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("SystemSettings");
                });

            modelBuilder.Entity("RollingShutterProject.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RollingShutterProject.Models.UserCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Command")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserCommands");
                });

            modelBuilder.Entity("RollingShutterProject.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AutoOpenShutter")
                        .HasColumnType("bit");

                    b.Property<bool>("AutoOpenShutterOnHighTemperature")
                        .HasColumnType("bit");

                    b.Property<bool>("AutoOpenShutterOnPoorAirQuality")
                        .HasColumnType("bit");

                    b.Property<bool>("DetectAnomalies")
                        .HasColumnType("bit");

                    b.Property<float>("HighTemperatureThreshold")
                        .HasColumnType("real");

                    b.Property<int>("LoggingIntervalHours")
                        .HasColumnType("int");

                    b.Property<string>("MqttClientId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MqttPayload")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MqttServer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MqttTopic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("NotifyOnHighTemperature")
                        .HasColumnType("bit");

                    b.Property<bool>("NotifyOnPoorAirQuality")
                        .HasColumnType("bit");

                    b.Property<float>("PoorAirQualityThreshold")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("RollingShutterProject.Models.SensorData", b =>
                {
                    b.HasOne("RollingShutterProject.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");
                });
#pragma warning restore 612, 618
        }
    }
}

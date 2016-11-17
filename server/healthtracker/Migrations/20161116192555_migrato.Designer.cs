﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using healthtracker.Model;

namespace healthtracker.Migrations
{
    [DbContext(typeof(HealthtrackerContext))]
    [Migration("20161116192555_migrato")]
    partial class migrato
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("healthtracker.Model.LogDay", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("LogDays");
                });

            modelBuilder.Entity("healthtracker.Model.LogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("LogDayId");

                    b.Property<long?>("LogTypeId");

                    b.Property<int>("NumberValue");

                    b.Property<string>("TextValue");

                    b.HasKey("Id");

                    b.HasIndex("LogDayId");

                    b.HasIndex("LogTypeId");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("healthtracker.Model.LogType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DataType");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LogTypes");
                });

            modelBuilder.Entity("healthtracker.Model.LogEntry", b =>
                {
                    b.HasOne("healthtracker.Model.LogDay")
                        .WithMany("LogEntries")
                        .HasForeignKey("LogDayId");

                    b.HasOne("healthtracker.Model.LogType", "LogType")
                        .WithMany()
                        .HasForeignKey("LogTypeId");
                });
        }
    }
}

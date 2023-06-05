﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Tenant.Infrastructure.EfCore;

#nullable disable

namespace Tenant.Infrastructure.EfCore.Migrations
{
    [DbContext(typeof(TenantDbContext))]
    [Migration("20230506223023_add_user_and_tenant_tables_to_out_of_master_schema")]
    partial class add_user_and_tenant_tables_to_out_of_master_schema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence<int>("expedition_no_sequence");

            modelBuilder.Entity("Tenant.Domain.Expedition.Expedition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ArrivalPoint")
                        .HasColumnType("text");

                    b.Property<Guid?>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DepartureDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeparturePoint")
                        .HasColumnType("text");

                    b.Property<string>("ExpeditionNo")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("SeatCount")
                        .HasColumnType("integer");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("VehicleNo")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("TenantId");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("expeditions", (string)null);
                });

            modelBuilder.Entity("Tenant.Domain.Passenger.Passenger", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ExpeditionId")
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Identity")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("SeatNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ExpeditionId");

                    b.HasIndex("TenantId");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("passengers", (string)null);
                });

            modelBuilder.Entity("Tenant.Domain.Tenant.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("tenants", (string)null);
                });

            modelBuilder.Entity("Tenant.Domain.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("TenantId");

                    b.HasIndex("UpdatedBy");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Tenant.Domain.Expedition.Expedition", b =>
                {
                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.Tenant.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("UpdatedBy");
                });

            modelBuilder.Entity("Tenant.Domain.Passenger.Passenger", b =>
                {
                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.Expedition.Expedition", "Expedition")
                        .WithMany("Passengers")
                        .HasForeignKey("ExpeditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.Tenant.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("UpdatedBy");

                    b.Navigation("Expedition");
                });

            modelBuilder.Entity("Tenant.Domain.User.User", b =>
                {
                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.Tenant.Tenant", null)
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tenant.Domain.User.User", null)
                        .WithMany()
                        .HasForeignKey("UpdatedBy");
                });

            modelBuilder.Entity("Tenant.Domain.Expedition.Expedition", b =>
                {
                    b.Navigation("Passengers");
                });
#pragma warning restore 612, 618
        }
    }
}

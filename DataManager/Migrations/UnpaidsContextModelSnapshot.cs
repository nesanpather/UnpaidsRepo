﻿// <auto-generated />
using System;
using DataManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataManager.Migrations
{
    [DbContext(typeof(UnpaidsContext))]
    partial class UnpaidsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataManager.Models.TbNotification", b =>
                {
                    b.Property<int>("NotificationId");

                    b.Property<string>("Notification")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("NotificationId");

                    b.ToTable("tb_Notification");
                });

            modelBuilder.Entity("DataManager.Models.TbResponse", b =>
                {
                    b.Property<int>("ResponseId");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ResponseId");

                    b.ToTable("tb_Response");
                });

            modelBuilder.Entity("DataManager.Models.TbStatus", b =>
                {
                    b.Property<int>("StatusId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("StatusId");

                    b.ToTable("tb_Status");
                });

            modelBuilder.Entity("DataManager.Models.TbUnpaid", b =>
                {
                    b.Property<int>("UnpaidId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<string>("IdNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("IdempotencyKey")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PolicyNumber")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("UnpaidId");

                    b.ToTable("tb_Unpaid");
                });

            modelBuilder.Entity("DataManager.Models.TbUnpaidRequest", b =>
                {
                    b.Property<int>("UnpaidRequestId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<int>("NotificationId");

                    b.Property<string>("StatusAdditionalInfo")
                        .HasMaxLength(200);

                    b.Property<int>("StatusId");

                    b.Property<int>("UnpaidId");

                    b.HasKey("UnpaidRequestId");

                    b.HasIndex("NotificationId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UnpaidId");

                    b.ToTable("tb_UnpaidRequest");
                });

            modelBuilder.Entity("DataManager.Models.TbUnpaidResponse", b =>
                {
                    b.Property<int>("UnpaidResponseId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Accepted");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<int>("ResponseId");

                    b.Property<int>("StatusId");

                    b.Property<int>("UnpaidRequestId");

                    b.HasKey("UnpaidResponseId");

                    b.HasIndex("ResponseId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UnpaidRequestId");

                    b.ToTable("tb_UnpaidResponse");
                });

            modelBuilder.Entity("DataManager.Models.TbUnpaidRequest", b =>
                {
                    b.HasOne("DataManager.Models.TbNotification", "Notification")
                        .WithMany("TbUnpaidRequest")
                        .HasForeignKey("NotificationId")
                        .HasConstraintName("FK_tb_UnpaidRequest_tb_Notification");

                    b.HasOne("DataManager.Models.TbStatus", "Status")
                        .WithMany("TbUnpaidRequest")
                        .HasForeignKey("StatusId")
                        .HasConstraintName("FK_tb_UnpaidRequest_tb_Status");

                    b.HasOne("DataManager.Models.TbUnpaid", "Unpaid")
                        .WithMany("TbUnpaidRequest")
                        .HasForeignKey("UnpaidId")
                        .HasConstraintName("FK_tb_UnpaidRequest_tb_Unpaid");
                });

            modelBuilder.Entity("DataManager.Models.TbUnpaidResponse", b =>
                {
                    b.HasOne("DataManager.Models.TbResponse", "Response")
                        .WithMany("TbUnpaidResponse")
                        .HasForeignKey("ResponseId")
                        .HasConstraintName("FK_tb_UnpaidResponse_tb_Response");

                    b.HasOne("DataManager.Models.TbStatus", "Status")
                        .WithMany("TbUnpaidResponse")
                        .HasForeignKey("StatusId")
                        .HasConstraintName("FK_tb_UnpaidResponse_tb_Status");

                    b.HasOne("DataManager.Models.TbUnpaidRequest", "UnpaidRequest")
                        .WithMany("TbUnpaidResponse")
                        .HasForeignKey("UnpaidRequestId")
                        .HasConstraintName("FK_tb_UnpaidResponse_tb_UnpaidRequest");
                });
#pragma warning restore 612, 618
        }
    }
}

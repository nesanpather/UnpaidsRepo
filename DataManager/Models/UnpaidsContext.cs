﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataManager.Models
{
    public partial class UnpaidsContext : DbContext
    {
        public UnpaidsContext()
        {
        }

        public UnpaidsContext(DbContextOptions<UnpaidsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbAccessToken> TbAccessToken { get; set; }
        public virtual DbSet<TbNotification> TbNotification { get; set; }
        public virtual DbSet<TbResponse> TbResponse { get; set; }
        public virtual DbSet<TbStatus> TbStatus { get; set; }
        public virtual DbSet<TbUnpaid> TbUnpaid { get; set; }
        public virtual DbSet<TbUnpaidBatch> TbUnpaidBatch { get; set; }
        public virtual DbSet<TbUnpaidRequest> TbUnpaidRequest { get; set; }
        public virtual DbSet<TbUnpaidResponse> TbUnpaidResponse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Persist Security Info=False;User ID=UnpaidsUser;Password=Password1234$;Initial Catalog=Unpaids;Server=localhost");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<TbAccessToken>(entity =>
            {
                entity.HasKey(e => e.AccessTokenId);

                entity.ToTable("tb_AccessToken");

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.DateExpires).HasColumnType("datetime");

                entity.Property(e => e.DateIssued).HasColumnType("datetime");

                entity.Property(e => e.TokenType)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<TbNotification>(entity =>
            {
                entity.HasKey(e => e.NotificationId);

                entity.ToTable("tb_Notification");

                entity.Property(e => e.NotificationId).ValueGeneratedNever();

                entity.Property(e => e.Notification)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbResponse>(entity =>
            {
                entity.HasKey(e => e.ResponseId);

                entity.ToTable("tb_Response");

                entity.Property(e => e.ResponseId).ValueGeneratedNever();

                entity.Property(e => e.Response)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("tb_Status");

                entity.Property(e => e.StatusId).ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TbUnpaid>(entity =>
            {
                entity.HasKey(e => e.UnpaidId);

                entity.ToTable("tb_Unpaid");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IdNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PolicyNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.UnpaidBatch)
                    .WithMany(p => p.TbUnpaid)
                    .HasForeignKey(d => d.UnpaidBatchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_Unpaid_tb_UnpaidBatch");
            });

            modelBuilder.Entity<TbUnpaidBatch>(entity =>
            {
                entity.HasKey(e => e.UnpaidBatchId);

                entity.ToTable("tb_UnpaidBatch");

                entity.Property(e => e.BatchKey)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TbUnpaidBatch)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidBatch_tb_Status");
            });

            modelBuilder.Entity<TbUnpaidRequest>(entity =>
            {
                entity.HasKey(e => e.UnpaidRequestId);

                entity.ToTable("tb_UnpaidRequest");

                entity.Property(e => e.CorrelationId)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DateModified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.StatusAdditionalInfo).HasMaxLength(200);

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.TbUnpaidRequest)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidRequest_tb_Notification");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TbUnpaidRequest)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidRequest_tb_Status");

                entity.HasOne(d => d.Unpaid)
                    .WithMany(p => p.TbUnpaidRequest)
                    .HasForeignKey(d => d.UnpaidId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidRequest_tb_Unpaid");
            });

            modelBuilder.Entity<TbUnpaidResponse>(entity =>
            {
                entity.HasKey(e => e.UnpaidResponseId);

                entity.ToTable("tb_UnpaidResponse");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Response)
                    .WithMany(p => p.TbUnpaidResponse)
                    .HasForeignKey(d => d.ResponseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidResponse_tb_Response");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TbUnpaidResponse)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidResponse_tb_Status");

                entity.HasOne(d => d.UnpaidRequest)
                    .WithMany(p => p.TbUnpaidResponse)
                    .HasForeignKey(d => d.UnpaidRequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tb_UnpaidResponse_tb_UnpaidRequest");
            });
        }
    }
}

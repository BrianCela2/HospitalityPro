using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Entities.Models
{
    public partial class HospitalityProContext : DbContext
    {
        public HospitalityProContext()
        {
        }

        public HospitalityProContext(DbContextOptions<HospitalityProContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HotelService> HotelServices { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Reservation> Reservations { get; set; } = null!;
        public virtual DbSet<ReservationRoom> ReservationRooms { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<RoomPhoto> RoomPhotos { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserHistory> UserHistories { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HospitalityPro;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HotelService>(entity =>
            {
                entity.HasKey(e => e.ServiceId)
                    .HasName("PK__HotelSer__C51BB0EAD7664AAD");

                entity.Property(e => e.ServiceId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.NotificationId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.NotificationReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Notificat__Recei__5629CD9C");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.NotificationSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK__Notificat__Sende__571DF1D5");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.ReservationId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Reservati__UserI__440B1D61");

                entity.HasMany(d => d.Services)
                    .WithMany(p => p.Reservations)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReservationService",
                        l => l.HasOne<HotelService>().WithMany().HasForeignKey("ServiceId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_S"),
                        r => r.HasOne<Reservation>().WithMany().HasForeignKey("ReservationId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Reserv"),
                        j =>
                        {
                            j.HasKey("ReservationId", "ServiceId");

                            j.ToTable("ReservationService");

                            j.IndexerProperty<Guid>("ReservationId").HasColumnName("ReservationID").HasDefaultValueSql("(newid())");

                            j.IndexerProperty<Guid>("ServiceId").HasColumnName("ServiceID");
                        });
            });

            modelBuilder.Entity<ReservationRoom>(entity =>
            {
                entity.HasKey(e => new { e.ReservationId, e.RoomId })
                    .HasName("PK_ReservationRoom");

                entity.Property(e => e.ReservationId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.ReservationRooms)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Res");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.ReservationRooms)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_R");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<RoomPhoto>(entity =>
            {
                entity.HasKey(e => e.PhotoId)
                    .HasName("PK__Room_Pho__21B7B582BE3A9A0C");

                entity.Property(e => e.PhotoId).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.RoomPhotos)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__Room_Phot__RoomI__52593CB8");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Roles })
                    .HasName("PK_EMAIL");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__User_Role__UserI__59FA5E80");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

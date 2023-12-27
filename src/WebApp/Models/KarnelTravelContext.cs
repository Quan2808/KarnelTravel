using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApp.Models
{
    public partial class KarnelTravelContext : DbContext
    {
        public KarnelTravelContext()
        {
        }

        public KarnelTravelContext(DbContextOptions<KarnelTravelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Resort> Resorts { get; set; } = null!;
        public virtual DbSet<Restaurant> Restaurants { get; set; } = null!;
        public virtual DbSet<TouristSpot> TouristSpots { get; set; } = null!;
        public virtual DbSet<TravelInfo> TravelInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.,1433;Database=KarnelTravel;User=sa;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.BookingTime).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TravelInfoId).HasColumnName("TravelInfoID");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.TravelInfo)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.TravelInfoId)
                    .HasConstraintName("FK__Booking__TravelI__5AEE82B9");

          
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");

                entity.Property(e => e.Comment)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.HasOne(d => d.Rating)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.RatingId)
                    .HasConstraintName("FK__Feedback__Rating__66603565");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK__Payment__Booking__5DCAEF64");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.RatingId).HasColumnName("RatingID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.ResortId).HasColumnName("ResortID");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__Rating__HotelID__60A75C0F");

                entity.HasOne(d => d.Resort)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.ResortId)
                    .HasConstraintName("FK__Rating__ResortID__619B8048");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__Rating__Restaura__628FA481");

            });

            modelBuilder.Entity<Resort>(entity =>
            {
                entity.ToTable("Resort");

                entity.Property(e => e.ResortId).HasColumnName("ResortID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurant");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TouristSpot>(entity =>
            {
                entity.ToTable("TouristSpot");

                entity.Property(e => e.TouristSpotId).HasColumnName("TouristSpotID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ResortId).HasColumnName("ResortID");

                entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.TouristSpots)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK__TouristSp__Hotel__52593CB8");

                entity.HasOne(d => d.Resort)
                    .WithMany(p => p.TouristSpots)
                    .HasForeignKey(d => d.ResortId)
                    .HasConstraintName("FK__TouristSp__Resor__534D60F1");

                entity.HasOne(d => d.Restaurant)
                    .WithMany(p => p.TouristSpots)
                    .HasForeignKey(d => d.RestaurantId)
                    .HasConstraintName("FK__TouristSp__Resta__5441852A");
            });

            modelBuilder.Entity<TravelInfo>(entity =>
            {
                entity.ToTable("TravelInfo");

                entity.Property(e => e.TravelInfoId).HasColumnName("TravelInfoID");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EndingTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StartingTime).HasColumnType("datetime");

                entity.Property(e => e.TouristSpotId).HasColumnName("TouristSpotID");

                entity.HasOne(d => d.TouristSpot)
                    .WithMany(p => p.TravelInfos)
                    .HasForeignKey(d => d.TouristSpotId)
                    .HasConstraintName("FK__TravelInf__Touri__571DF1D5");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

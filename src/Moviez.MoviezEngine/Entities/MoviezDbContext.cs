using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Moviez.MoviezEngine.Entities
{
    public partial class MoviezDbContext : DbContext
    {
        public MoviezDbContext()
        {
        }

        public MoviezDbContext(DbContextOptions<MoviezDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<Casting> Castings { get; set; }
        public virtual DbSet<LkpGender> LkpGenders { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actors");

                entity.Property(e => e.ActorId)
                    .HasColumnName("actor_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ActorName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("actor_name");

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000)
                    .HasColumnName("bio");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.GenderCode)
                    .HasMaxLength(15)
                    .HasColumnName("gender_code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.HasOne(d => d.GenderCodeNavigation)
                    .WithMany(p => p.Actors)
                    .HasForeignKey(d => d.GenderCode)
                    .HasConstraintName("actors_lkp_genders_fk");
            });

            modelBuilder.Entity<Casting>(entity =>
            {
                entity.HasKey(e => e.CastId)
                    .HasName("castings_pkey");

                entity.ToTable("castings");

                entity.Property(e => e.CastId)
                    .HasColumnName("cast_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.ActorId).HasColumnName("actor_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Castings)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("castings_actors_fk");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Castings)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("castings_movies_fk");
            });

            modelBuilder.Entity<LkpGender>(entity =>
            {
                entity.HasKey(e => e.GenderCode)
                    .HasName("lkp_genders_pk");

                entity.ToTable("lkp_genders");

                entity.Property(e => e.GenderCode)
                    .HasMaxLength(15)
                    .HasColumnName("gender_code");

                entity.Property(e => e.GenderDesc)
                    .HasMaxLength(15)
                    .HasColumnName("gender_desc");

                entity.Property(e => e.IsActive).HasColumnName("is_active");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movies");

                entity.HasIndex(e => e.MovieName, "movies_name_un")
                    .IsUnique();

                entity.Property(e => e.MovieId)
                    .HasColumnName("movie_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.DateOfRelease).HasColumnName("date_of_release");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.MovieName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("movie_name");

                entity.Property(e => e.Plot)
                    .HasMaxLength(1000)
                    .HasColumnName("plot");

                entity.Property(e => e.PosterLink).HasColumnName("poster_link");

                entity.Property(e => e.ProducerId).HasColumnName("producer_id");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.ProducerId)
                    .HasConstraintName("movies_producers_fk");
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("producers");

                entity.Property(e => e.ProducerId)
                    .HasColumnName("producer_id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000)
                    .HasColumnName("bio");

                entity.Property(e => e.Company)
                    .HasMaxLength(100)
                    .HasColumnName("company");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.GenderCode)
                    .HasMaxLength(15)
                    .HasColumnName("gender_code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ProducerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("producer_name");

                entity.HasOne(d => d.GenderCodeNavigation)
                    .WithMany(p => p.Producers)
                    .HasForeignKey(d => d.GenderCode)
                    .HasConstraintName("producers_lkp_genders_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

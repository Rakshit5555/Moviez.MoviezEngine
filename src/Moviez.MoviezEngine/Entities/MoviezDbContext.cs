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
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actors");

                entity.Property(e => e.ActorId)
                    .ValueGeneratedNever()
                    .HasColumnName("actor_id");

                entity.Property(e => e.ActorName)
                    .HasMaxLength(50)
                    .HasColumnName("actor_name");

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000)
                    .HasColumnName("bio");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.Gender)
                    .HasMaxLength(15)
                    .HasColumnName("gender");
            });

            modelBuilder.Entity<Casting>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("castings");

                entity.Property(e => e.ActorId).HasColumnName("actor_id");

                entity.Property(e => e.MovieId).HasColumnName("movie_id");

                entity.HasOne(d => d.Actor)
                    .WithMany()
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("castings_actors_fk");

                entity.HasOne(d => d.Movie)
                    .WithMany()
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("castings_movies_fk");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movies");

                entity.Property(e => e.MovieId)
                    .ValueGeneratedNever()
                    .HasColumnName("movie_id");

                entity.Property(e => e.DateOfRelease).HasColumnName("date_of_release");

                entity.Property(e => e.MovieName)
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
                    .ValueGeneratedNever()
                    .HasColumnName("producer_id");

                entity.Property(e => e.Bio)
                    .HasMaxLength(1000)
                    .HasColumnName("bio");

                entity.Property(e => e.Company)
                    .HasMaxLength(100)
                    .HasColumnName("company");

                entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");

                entity.Property(e => e.Gender)
                    .HasMaxLength(15)
                    .HasColumnName("gender");

                entity.Property(e => e.ProducerName)
                    .HasMaxLength(50)
                    .HasColumnName("producer_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

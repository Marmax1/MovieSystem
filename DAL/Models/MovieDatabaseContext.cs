using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class MovieDatabaseContext : DbContext
{
    public MovieDatabaseContext()
    {
    }

    public MovieDatabaseContext(DbContextOptions<MovieDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Movieactor> Movieactors { get; set; }

    public virtual DbSet<Moviegenre> Moviegenres { get; set; }

    public virtual DbSet<Studio> Studios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=MovieDatabase;Username=postgres;Password=mocco#skl23mm");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("actors_pkey");

            entity.ToTable("actors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("directors_pkey");

            entity.ToTable("directors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movies_pkey");

            entity.ToTable("movies");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Directorid).HasColumnName("directorid");
            entity.Property(e => e.Filepath)
                .HasMaxLength(255)
                .HasColumnName("filepath");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Releaseyear).HasColumnName("releaseyear");
            entity.Property(e => e.Studioid).HasColumnName("studioid");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Director).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Directorid)
                .HasConstraintName("movies_directorid_fkey");

            entity.HasOne(d => d.Studio).WithMany(p => p.Movies)
                .HasForeignKey(d => d.Studioid)
                .HasConstraintName("movies_studioid_fkey");
        });

        modelBuilder.Entity<Movieactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("movieactors_pkey");

            entity.ToTable("movieactors");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actorid).HasColumnName("actorid");
            entity.Property(e => e.Movieid).HasColumnName("movieid");

            entity.HasOne(d => d.Actor).WithMany(p => p.Movieactors)
                .HasForeignKey(d => d.Actorid)
                .HasConstraintName("movieactors_actorid_fkey");

            entity.HasOne(d => d.Movie).WithMany(p => p.Movieactors)
                .HasForeignKey(d => d.Movieid)
                .HasConstraintName("movieactors_movieid_fkey");
        });

        modelBuilder.Entity<Moviegenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("moviegenres_pkey");

            entity.ToTable("moviegenres");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Genreid).HasColumnName("genreid");
            entity.Property(e => e.Movieid).HasColumnName("movieid");

            entity.HasOne(d => d.Genre).WithMany(p => p.Moviegenres)
                .HasForeignKey(d => d.Genreid)
                .HasConstraintName("moviegenres_genreid_fkey");

            entity.HasOne(d => d.Movie).WithMany(p => p.Moviegenres)
                .HasForeignKey(d => d.Movieid)
                .HasConstraintName("moviegenres_movieid_fkey");
        });

        modelBuilder.Entity<Studio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("studios_pkey");

            entity.ToTable("studios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

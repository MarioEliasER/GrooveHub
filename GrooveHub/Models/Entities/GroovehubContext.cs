using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GrooveHub.Models.Entities;

public partial class GroovehubContext : DbContext
{
    public GroovehubContext()
    {
    }

    public GroovehubContext(DbContextOptions<GroovehubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Album { get; set; }

    public virtual DbSet<Artista> Artista { get; set; }

    public virtual DbSet<Cancion> Cancion { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("album");

            entity.Property(e => e.FechaLanzamiento).HasColumnType("datetime");
            entity.Property(e => e.TituloAlbum).HasMaxLength(50);
        });

        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("artista");

            entity.Property(e => e.Biografia).HasMaxLength(500);
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Cancion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cancion");

            entity.HasIndex(e => e.IdAlbum, "cancion_album_idx");

            entity.Property(e => e.AñoLanzamiento).HasColumnType("datetime");
            entity.Property(e => e.Duracion).HasColumnType("time");
            entity.Property(e => e.Nombre).HasMaxLength(45);

            entity.HasOne(d => d.IdAlbumNavigation).WithMany(p => p.Cancion)
                .HasForeignKey(d => d.IdAlbum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cancion_album");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Contrasena)
                .HasMaxLength(128)
                .IsFixedLength();
            entity.Property(e => e.CorreoElectronico).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

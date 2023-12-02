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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;password=root;user=root;database=groovehub", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("album");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fechalanzamiento)
                .HasColumnType("datetime")
                .HasColumnName("fechalanzamiento");
            entity.Property(e => e.Tituloalbum)
                .HasMaxLength(50)
                .HasColumnName("tituloalbum");
        });

        modelBuilder.Entity<Artista>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("artista");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Biografia)
                .HasMaxLength(500)
                .HasColumnName("biografia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cancion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cancion");

            entity.HasIndex(e => e.Idalbum, "cancion_album_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Añolanzamiento)
                .HasColumnType("datetime")
                .HasColumnName("añolanzamiento");
            entity.Property(e => e.Duracion)
                .HasColumnType("time")
                .HasColumnName("duracion");
            entity.Property(e => e.Idalbum).HasColumnName("idalbum");
            entity.Property(e => e.Nombre)
                .HasMaxLength(45)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdalbumNavigation).WithMany(p => p.Cancion)
                .HasForeignKey(d => d.Idalbum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cancion_album");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(128)
                .IsFixedLength()
                .HasColumnName("contrasena");
            entity.Property(e => e.Correoelectronico)
                .HasMaxLength(255)
                .HasColumnName("correoelectronico");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Rol).HasColumnName("rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApi1.Models;

public partial class ApiCrudContext : DbContext
{
    public ApiCrudContext()
    {
    }

    public ApiCrudContext(DbContextOptions<ApiCrudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ciudades> Ciudades { get; set; }

    public virtual DbSet<Conferencia> Conferencia { get; set; }

    public virtual DbSet<Departamentos> Departamentos { get; set; }

    public virtual DbSet<Ponentes> Ponentes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ciudades>(entity =>
        {
            entity.HasKey(e => e.IdCiudades).HasName("PK__Ciudades__7C524AB263CE1B4B");

            entity.Property(e => e.DepartamentoId).HasColumnName("Departamento_id");
            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.Departamento).WithMany(p => p.Ciudades)
                .HasForeignKey(d => d.DepartamentoId)
                .HasConstraintName("FK__Ciudades__Depart__4E88ABD4");
        });

        modelBuilder.Entity<Conferencia>(entity =>
        {
            entity.HasKey(e => e.IdConferencias).HasName("PK__Conferen__73DF9BDE20D2911D");

            entity.Property(e => e.Espacio).HasMaxLength(100);
            entity.Property(e => e.NumeroPersonas).HasColumnName("Numero_personas");
            entity.Property(e => e.NumeroPonentes).HasColumnName("Numero_ponentes");
            entity.Property(e => e.Temas).HasMaxLength(255);
        });

        modelBuilder.Entity<Departamentos>(entity =>
        {
            entity.HasKey(e => e.IdDepartamentos).HasName("PK__Departam__87FA9BFF318B23EC");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Ponentes>(entity =>
        {
            entity.HasKey(e => e.IdPonentes).HasName("PK__Ponentes__897C57D8426461A4");

            entity.Property(e => e.Apellidos).HasMaxLength(50);
            entity.Property(e => e.Celular).HasMaxLength(15);
            entity.Property(e => e.Ciudad).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Departamento).HasMaxLength(50);
            entity.Property(e => e.Documento).HasMaxLength(20);
            entity.Property(e => e.Empresa).HasMaxLength(100);
            entity.Property(e => e.Nombres).HasMaxLength(50);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .HasColumnName("Tipo_documento");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210B91F61B4");

            entity.ToTable("Producto");

            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A6C0862CEB");

            entity.ToTable("Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

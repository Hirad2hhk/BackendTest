using System;
using System.Collections.Generic;
using Factor.Persistence.Entities.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Factor.Infrastructure.Persistence.Models;

public partial class FactorDbContext : DbContext {
    public FactorDbContext() { }

    public FactorDbContext(DbContextOptions<FactorDbContext> options)
        : base(options) { }

    public virtual DbSet<Factor.Persistence.Entities.Persistence.Models.Factor> Factors { get; set; }

    public virtual DbSet<FactorDetail> FactorDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Factor;Username=postgres;Password=backendtest@1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Factor.Persistence.Entities.Persistence.Models.Factor>(entity => {
            entity.HasKey(e => e.FactorId).HasName("Factor_pkey");

            entity.ToTable("Factor");

            entity.Property(e => e.Customer).HasMaxLength(100);
            entity.Property(e => e.FactorNo).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<FactorDetail>(entity => {
            entity.HasKey(e => e.FactorDetailId).HasName("FactorDetail_pkey");

            entity.ToTable("FactorDetail");

            entity.Property(e => e.ProductDescription).HasMaxLength(200);
            entity.Property(e => e.SumPrice)
                .HasPrecision(18, 2)
                .HasComputedColumnSql("((\"Count\")::numeric * \"UnitPrice\")", true);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            entity.HasOne(d => d.Factor).WithMany(p => p.FactorDetails)
                .HasForeignKey(d => d.FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FactorDetail_FactorId_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.FactorDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FactorDetail_ProductId_fkey");
        });

        modelBuilder.Entity<Product>(entity => {
            entity.HasKey(e => e.ProductId).HasName("Product_pkey");

            entity.ToTable("Product");

            entity.Property(e => e.ProductCode).HasMaxLength(50);
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Unit).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
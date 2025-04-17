using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Factor.Persistence.Entities.Persistence.Models;

public partial class FactorContext : DbContext
{
    public FactorContext()
    {
    }

    public FactorContext(DbContextOptions<FactorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Factor> Factors { get; set; }

    public virtual DbSet<FactorDetail> FactorDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Factor;Username=postgres;Password=backendtest@1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Factor>(entity =>
        {
            entity.HasKey(e => e.FactorId).HasName("factor_pkey");

            entity.ToTable("factor");

            entity.Property(e => e.FactorId).HasColumnName("factor_id");
            entity.Property(e => e.Customer)
                .HasMaxLength(50)
                .HasColumnName("customer");
            entity.Property(e => e.DelivaryType).HasColumnName("delivary_type");
            entity.Property(e => e.FactorDate).HasColumnName("factor_date");
            entity.Property(e => e.FactorNo).HasColumnName("factor_no");
            entity.Property(e => e.TotalPrice).HasColumnName("total_price");
        });

        modelBuilder.Entity<FactorDetail>(entity =>
        {
            entity.HasKey(e => e.FactorDetailId).HasName("factor_detail_pkey");

            entity.ToTable("factor_detail");

            entity.Property(e => e.FactorDetailId).HasColumnName("factor_detail_id");
            entity.Property(e => e.Count)
                .HasPrecision(18, 3)
                .HasColumnName("count");
            entity.Property(e => e.FactorId).HasColumnName("factor_id");
            entity.Property(e => e.ProductDescription)
                .HasMaxLength(50)
                .HasColumnName("product_description");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.SumPrice).HasColumnName("sum_price");
            entity.Property(e => e.UnitPrice).HasColumnName("unit_price");

            entity.HasOne(d => d.Factor).WithMany(p => p.FactorDetails)
                .HasForeignKey(d => d.FactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factor_detail_factor_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.FactorDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("factor_detail_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ChangeDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("change_date");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(13)
                .HasColumnName("product_code");
            entity.Property(e => e.ProductName)
                .HasMaxLength(50)
                .HasColumnName("product_name");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

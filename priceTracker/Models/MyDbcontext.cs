using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace priceTracker.Models;

public partial class MyDbcontext : DbContext
{
    public MyDbcontext()
    {
    }

    public MyDbcontext(DbContextOptions<MyDbcontext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employe> Employes { get; set; }

    public virtual DbSet<Entry> Entrys { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\;User Id=sa;Password=momo;Database=priceTracker;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employe__3213E83F0B679910");

            entity.ToTable("employes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpType)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("empType");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("mail");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("password");
        });

        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Entry__3213E83F69EE01BF");

            entity.ToTable("entrys");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("empId");
            entity.Property(e => e.FinishDate).HasColumnName("finishDate");
            entity.Property(e => e.RecordDate).HasColumnName("recordDate");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Url1)
                .HasMaxLength(500)
                .HasColumnName("url1");
            entity.Property(e => e.Url2)
                .HasMaxLength(500)
                .HasColumnName("url2");
            entity.Property(e => e.Url3)
                .HasMaxLength(500)
                .HasColumnName("url3");
            entity.Property(e => e.Url4)
                .HasMaxLength(500)
                .HasColumnName("url4");
            entity.Property(e => e.Url5)
                .HasMaxLength(500)
                .HasColumnName("url5");

            entity.HasOne(d => d.Emp).WithMany(p => p.Entries)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK_Entry_Employe");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F59A3E981");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProdId).HasColumnName("prodId");
            entity.Property(e => e.ProductName)
                .HasMaxLength(500)
                .HasColumnName("productName");
            entity.Property(e => e.SiteName)
                .HasMaxLength(500)
                .HasColumnName("siteName");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");
            entity.Property(e => e.UrlNumber).HasColumnName("urlNumber");

            entity.HasOne(d => d.Prod).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdId)
                .HasConstraintName("fk_prod_entry");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

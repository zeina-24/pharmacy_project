using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Pharmacy.Models;

public partial class PharmacyDbContext : DbContext
{
    public PharmacyDbContext()
    {
    }

    public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminLog> AdminLogs { get; set; }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GF59DLG;Database=Pharma;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admins__43AA41414187E7C9");

            entity.ToTable("admins");

            entity.HasIndex(e => e.Email, "UQ__admins__AB6E6164197A7DBD").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__admins__F3DBC5729AC55C96").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(512)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

        modelBuilder.Entity<AdminLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__admin_lo__9E2397E02FDA9C0F");

            entity.ToTable("admin_logs");

            entity.Property(e => e.LogId).HasColumnName("log_id");
            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("action_time");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .HasColumnName("action_type");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");

            entity.HasOne(d => d.Admin).WithMany(p => p.AdminLogs)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("fk_admin_logs_admin");
        });

        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.DrugId).HasName("PK__drugs__73F2330C19712C29");

            entity.ToTable("drugs");

            entity.HasIndex(e => e.Barcode, "UQ__drugs__C16E36F8532FAB6D").IsUnique();

            entity.Property(e => e.DrugId).HasColumnName("drug_id");
            entity.Property(e => e.Barcode)
                .HasMaxLength(255)
                .HasColumnName("barcode");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.DescriptionBeforeUse).HasColumnName("description_before_use");
            entity.Property(e => e.DescriptionHowToUse).HasColumnName("description_how_to_use");
            entity.Property(e => e.DescriptionSideEffects).HasColumnName("description_side_effects");
            entity.Property(e => e.DrugType)
                .HasMaxLength(50)
                .HasColumnName("drug_type");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1000)
                .HasColumnName("image_url");
            entity.Property(e => e.LowAmount)
                .HasDefaultValue(10)
                .HasColumnName("low_amount");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(255)
                .HasColumnName("manufacturer");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.PurchasingPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("purchasing_price");
            entity.Property(e => e.RequiresPrescription).HasColumnName("requires_prescription");
            entity.Property(e => e.SellingPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("selling_price");
            entity.Property(e => e.ShelfAmount).HasColumnName("shelf_amount");
            entity.Property(e => e.StoredAmount).HasColumnName("stored_amount");
            entity.Property(e => e.SubAmountQuantity).HasColumnName("sub_amount_quantity");

            entity.HasMany(d => d.Tags).WithMany(p => p.Drugs)
                .UsingEntity<Dictionary<string, object>>(
                    "DrugTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_dt_tag"),
                    l => l.HasOne<Drug>().WithMany()
                        .HasForeignKey("DrugId")
                        .HasConstraintName("fk_dt_drug"),
                    j =>
                    {
                        j.HasKey("DrugId", "TagId").HasName("PK__drug_tag__07DB59279E52B030");
                        j.ToTable("drug_tags");
                        j.IndexerProperty<int>("DrugId").HasColumnName("drug_id");
                        j.IndexerProperty<int>("TagId").HasColumnName("tag_id");
                    });
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceId).HasName("PK__invoices__F58DFD498FCB2449");

            entity.ToTable("invoices");

            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.ChangeAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("change_amount");
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.InvoiceTime)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("invoice_time");
            entity.Property(e => e.TaxAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("tax_amount");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Admin).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_invoice_admin");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__invoice___52020FDDF2778E3A");

            entity.ToTable("invoice_items");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.DrugId).HasColumnName("drug_id");
            entity.Property(e => e.InvoiceId).HasColumnName("invoice_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Drug).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("fk_ii_drug");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("fk_ii_invoice");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__tags__4296A2B62CA1F6A4");

            entity.ToTable("tags");

            entity.HasIndex(e => e.Name, "UQ__tags__72E12F1BADD7F662").IsUnique();

            entity.Property(e => e.TagId).HasColumnName("tag_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

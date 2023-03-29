using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResoPayment.Infrastructure.Models
{
    public partial class PosPaymentContext : DbContext
    {
        public PosPaymentContext()
        {
        }

        public PosPaymentContext(DbContextOptions<PosPaymentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<BrandPaymentProviderMapping> BrandPaymentProviderMappings { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<PaymentProvider> PaymentProviders { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("Brand");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AccessToken).IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DesKey).IsUnicode(false);

                entity.Property(e => e.DesVector).IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Pgppassword)
                    .IsUnicode(false)
                    .HasColumnName("PGPPassword");

                entity.Property(e => e.PgpprivateKey)
                    .IsUnicode(false)
                    .HasColumnName("PGPPrivateKey");

                entity.Property(e => e.PgppublicKey)
                    .IsUnicode(false)
                    .HasColumnName("PGPPublicKey");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TaxCode).IsUnicode(false);

                entity.Property(e => e.Vatcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("VATCode");
            });

            modelBuilder.Entity<BrandPaymentProviderMapping>(entity =>
            {
                entity.ToTable("BrandPaymentProviderMapping");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Config).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.BrandPaymentProviderMappings)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandPaymentProviderMapping_Brand");

                entity.HasOne(d => d.PaymentProvider)
                    .WithMany(p => p.BrandPaymentProviderMappings)
                    .HasForeignKey(d => d.PaymentProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BrandPaymentProviderMapping_PaymentProvider");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.InvoiceId, "UQ_Order_InvoiceId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.InvoiceId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vat).HasColumnName("VAT");

                entity.Property(e => e.Vatamount).HasColumnName("VATAmount");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Store");
            });

            modelBuilder.Entity<PaymentProvider>(entity =>
            {
                entity.ToTable("PaymentProvider");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.PicUrl).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Stores)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Store_Brand");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CurrencyCode).HasMaxLength(10);

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Brand");

                entity.HasOne(d => d.PaymentProvider)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentProviderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_PaymentProvider");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Store");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

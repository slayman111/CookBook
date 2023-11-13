using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace CookBookApp.Models.Entities
{
    public partial class CookBookDbContext : DbContext
    {
        public CookBookDbContext()
        {
        }

        public CookBookDbContext(DbContextOptions<CookBookDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Ingredient> Ingredients { get; set; } = null!;
        public virtual DbSet<Instruction> Instructions { get; set; } = null!;
        public virtual DbSet<Receipt> Receipts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("Ingredient");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Instruction>(entity =>
            {
                entity.ToTable("Instruction");

                entity.Property(e => e.Name).HasColumnType("text");
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.ToTable("Receipt");

                entity.Property(e => e.Caloric).HasMaxLength(50);

                entity.Property(e => e.Carbohydrates).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Fatness).HasMaxLength(50);

                entity.Property(e => e.Image).HasColumnType("image");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Squirrels).HasMaxLength(50);

                entity.HasMany(d => d.Categories)
                    .WithMany(p => p.Receipts)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReceiptHasCategory",
                        l => l.HasOne<Category>().WithMany().HasForeignKey("CategoryId")
                            .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ReceiptHasCategory_Category"),
                        r => r.HasOne<Receipt>().WithMany().HasForeignKey("ReceiptId")
                            .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_ReceiptHasCategory_Receipt"),
                        j =>
                        {
                            j.HasKey("ReceiptId", "CategoryId");

                            j.ToTable("ReceiptHasCategory");
                        });

                entity.HasMany(d => d.Ingridients)
                    .WithMany(p => p.Receips)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReceiptHasIngridient",
                        l => l.HasOne<Ingredient>().WithMany().HasForeignKey("IngridientId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ReceiptHasIngridient_Ingredient"),
                        r => r.HasOne<Receipt>().WithMany().HasForeignKey("ReceipId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ReceiptHasIngridient_Receipt"),
                        j =>
                        {
                            j.HasKey("ReceipId", "IngridientId");

                            j.ToTable("ReceiptHasIngridient");
                        });

                entity.HasMany(d => d.Instructions)
                    .WithMany(p => p.Receipts)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReceiptHasInstruction",
                        l => l.HasOne<Instruction>().WithMany().HasForeignKey("InstructionId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ReceiptHasInstruction_Instruction"),
                        r => r.HasOne<Receipt>().WithMany().HasForeignKey("ReceiptId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_ReceiptHasInstruction_Receipt"),
                        j =>
                        {
                            j.HasKey("ReceiptId", "InstructionId");

                            j.ToTable("ReceiptHasInstruction");
                        });

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.Receipts)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasReceipt",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK_UserHasReceipts_User"),
                        r => r.HasOne<Receipt>().WithMany().HasForeignKey("ReceiptId")
                            .OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_UserHasReceipts_Receipt"),
                        j =>
                        {
                            j.HasKey("ReceiptId", "UserId");

                            j.ToTable("UserHasReceipts");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

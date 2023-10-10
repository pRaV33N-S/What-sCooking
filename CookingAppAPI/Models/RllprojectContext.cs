using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CookingAppAPI.Models;

namespace CookingAppAPI.Models;

public partial class RllprojectContext : DbContext
{
    public RllprojectContext()
    {
    }

    public RllprojectContext(DbContextOptions<RllprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("server=DESKTOP-CHNJ5UD;database=Rllproject;trusted_connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK_Admin_719FE4E8E2B8766B");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Adminname).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Recipes_3214EC27854E1C96");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageUrl).HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.SubmissionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Users_1788CCACD4A5940B");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackID).HasName("PK_Feedback");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackID).HasColumnName("FeedbackID");
            entity.Property(e => e.UserName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DishName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Comment).HasColumnType("nvarchar(MAX)");
            entity.Property(e => e.Rating).HasColumnType("float");
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
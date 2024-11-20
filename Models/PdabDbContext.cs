using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PDAB.Models;

public partial class PdabDbContext : DbContext
{
    public PdabDbContext()
    {
    }

    public PdabDbContext(DbContextOptions<PdabDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountProduct> DiscountProducts { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderPayment> OrderPayments { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;User ID=sa;Password=password123;Database=pdab_db;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.ToTable("Discount");

            entity.Property(e => e.DiscountId)
                .ValueGeneratedNever()
                .HasColumnName("DiscountID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DiscountName).HasMaxLength(100);
            entity.Property(e => e.DiscountPercentage).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<DiscountProduct>(entity =>
        {
            entity.ToTable("DiscountProduct");

            entity.Property(e => e.DiscountProductId)
                .ValueGeneratedNever()
                .HasColumnName("DiscountProductID");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountProducts)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountProduct_Discount");

            entity.HasOne(d => d.Product).WithMany(p => p.DiscountProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountProduct_Product");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Position).HasMaxLength(50);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturer");

            entity.Property(e => e.ManufacturerId)
                .ValueGeneratedNever()
                .HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Customer");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderStatus");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId)
                .ValueGeneratedNever()
                .HasColumnName("OrderDetailID");
            entity.Property(e => e.DiscountId).HasColumnName("DiscountID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Discount).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Discount");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.ToTable("OrderPayment");

            entity.Property(e => e.OrderPaymentId)
                .ValueGeneratedNever()
                .HasColumnName("OrderPaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethodId).HasColumnName("PaymentMethodID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPayments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderPayment_Order");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.OrderPayments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderPayment_PaymentMethod");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId)
                .ValueGeneratedNever()
                .HasColumnName("OrderStatusID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("PaymentMethod");

            entity.Property(e => e.PaymentMethodId)
                .ValueGeneratedNever()
                .HasColumnName("PaymentMethodID");
            entity.Property(e => e.MethodName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Manufacturer");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImage");

            entity.Property(e => e.ProductImageId)
                .ValueGeneratedNever()
                .HasColumnName("ProductImageID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(2000)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImage_Product");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.ReviewId)
                .ValueGeneratedNever()
                .HasColumnName("ReviewID");
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ReviewDate).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Review_Product1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.Users)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Employee");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

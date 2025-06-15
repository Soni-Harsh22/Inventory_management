using System.Numerics;
using InventoryManagementSystem.Common.Enums;
using InventoryManagementSystem.Data.Configurations;
using InventoryManagementSystem.Models.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Data.Context
{
    /// <summary>
    /// Represents the application's database context, which manages the connection to the database.
    /// It includes DbSets for Users and Roles.
    /// </summary>
    /// <param name="options">The options to configure the DbContext.</param>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// DbSet representing the Users table in the database.
        /// </summary>
        public DbSet<Users> Users { get; set; }

        /// <summary>
        /// DbSet representing the Roles table in the database.
        /// </summary>
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<StocksMovement> StocksMovement { get; set; }
        public DbSet<StockStatus> StockStatus { get; set; }
        public DbSet<StockMovementType> StockMovementType { get; set; }
        public DbSet<StockPurchaseOrders> StockPurchaseOrders { get; set; }
        public DbSet<StockPurchaseOrderItems> StockPurchaseOrderItems { get; set; }
        public DbSet<VendorDetails> VendorDetails { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PaymentType> PaymentType { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        /// <summary>
        /// Configures the model by applying the entity relationships.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().HasData(
                new Users { UserId = 1, Name = "System", RoleId = 1, Phone = "0000000000", Email = "system@gmail.com", Password = "System", CreatedBy = 0, UpdatedBy = 0, IsDeleted = false }

            );
            
            modelBuilder.Entity<PaymentMethod>().HasData(
                new PaymentMethod { PaymentMethodId = 1, Name = "Online" },
                new PaymentMethod { PaymentMethodId = 2, Name = "Cash" },
                new PaymentMethod { PaymentMethodId = 3, Name = "Card" },
                new PaymentMethod { PaymentMethodId = 4, Name = "Cheque" }
            );

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { StatusId = 1, Name = "Pending" },
                new OrderStatus { StatusId = 2, Name = "Recived" }, 
                new OrderStatus { StatusId = 3, Name = "Completed" },
                new OrderStatus { StatusId = 4, Name = "Incomplete" },
                new OrderStatus { StatusId = 5, Name = "Cancelled" },
                new OrderStatus { StatusId = 6, Name = "Returned" },
                new OrderStatus { StatusId = 7, Name = "ParcialReturn" } 
            );

            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { PaymentStatusId = 1,Name = "Pending" },
                new PaymentStatus { PaymentStatusId = 2,Name = "Paid" },
                new PaymentStatus { PaymentStatusId = 3,Name = "Incomplete" }
            );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { PaymentTypeId = 1,Name = "Prepaid" },
                new PaymentType { PaymentTypeId = 2,Name = "Postpaid" }
            );

            
            modelBuilder.Entity<StockMovementType>().HasData(
                new StockMovementType { MovementTypeId = 1 ,Name = "Restock" },
                new StockMovementType { MovementTypeId = 2,Name = "SellOut" },
                new StockMovementType { MovementTypeId = 3,Name = "Return" },
                new StockMovementType { MovementTypeId = 4,Name = "NewlyAdded" }
            );

            modelBuilder.Entity<StockStatus>().HasData(
                new StockStatus { StockStatusId = 1 ,Name = "Available" },
                new StockStatus { StockStatusId = 2 ,Name = "OutOfStock" },
                new StockStatus { StockStatusId = 3 ,Name = "NewAdded" },
                new StockStatus { StockStatusId = 4 ,Name = "LessStock" }
            );

            modelBuilder.Entity<Roles>().HasData(
                new Roles { RoleId =1 , Name = "System" },
                new Roles { RoleId = 2, Name = "Admin" },
                new Roles { RoleId = 3, Name = "Employee" },
                new Roles { RoleId = 4, Name = "Customer" }
            );

            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new StocksMovementConfiguration());
            modelBuilder.ApplyConfiguration(new StockPurchaseOrdersConfiguration());
            modelBuilder.ApplyConfiguration(new VendorDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new StockPurchaseOrderItemsConfiguration());
            modelBuilder.ApplyConfiguration(new OrdersConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemsConfiguration());

        }
    }
}
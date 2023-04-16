using API.Carts.Application.Dto;
using API.Carts.Domain.Entities;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Carts.Infrastructure
{
    public class CartContext : DbContext, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        public const string Schema = "ms_cart";

        public CartContext(DbContextOptions<CartContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<ProductDto> Products { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cartTableBuilder = modelBuilder.Entity<Cart>().ToTable("Carts", Schema);

            cartTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            cartTableBuilder
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(20,5)");

            var cartItemTableBuilder = modelBuilder.Entity<CartItem>().ToTable("CartItems", Schema);

            cartItemTableBuilder
                .Property(p => p.UnitPrice)
                .HasColumnType("decimal(20,5)");

            cartItemTableBuilder
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal(20,5)");

            var productTableBuilder = modelBuilder.Entity<ProductDto>().ToTable("Products", Schema);

            productTableBuilder
                .Property(p => p.Id)
                .ValueGeneratedNever();

            productTableBuilder
                .Property(p => p.Price)
                .HasColumnType("decimal(20,5)");

            productTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);
            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);
            requestEntryTableBuilder.HasKey(x => x.Id);
        }
    }
}
